using BESM3CAData.Control;
using BESM3CAData.Listings;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;

namespace BESM3CAData.Model
{
    public class DataNode : BaseNode
    {
        //Fields:           
        protected DataListing _dataListing;
        
        //Properties:
        public override string DisplayText
        {
            get
            {
                if (_dataListing != null)
                {
                    if (AttributeType == "Special")
                    {
                        return Name;
                    }
                    else
                    {
                        return $"{Name} ({GetPoints()} Points)";
                    }
                }
                else
                {
                    return "";
                }
            }
        }

        public override bool HasCharacterStats
        {
            get
            {
                return false;
            }
        }

        public override bool HasLevelStats
        {
            get
            {
                return false;
            }
        }

        public override bool HasPointsStats
        {
            get
            {
                return false;
            }
        }

        public virtual string baseDescription
        {
            get
            {
                string result = _dataListing.Description;
                return result;
            }
        }

        public string AttributeDescription
        {
            get
            {
                //Need to process attribute description to calculate numeric components
                string description = baseDescription;

                string completedDescription = "";

                while (description != null)
                {
                    string[] pieces = description.Split('[', 2);
                    completedDescription += pieces[0];
                    if (pieces.Length > 1)
                    {
                        description = pieces[1];
                        pieces = description.Split(']', 2);

                        completedDescription += ProcessDescriptionValue(pieces[0]);

                        if (pieces.Length > 1)
                        {
                            description = pieces[1];
                        }
                        else
                        {
                            description = null;
                        }
                    }
                    else
                    {
                        description = null;
                    }
                }

                return completedDescription;
            }
        }

        public string AttributeType
        {
            get
            {
                return _dataListing.Type;
            }
        }

        public override List<DataListing> PotentialChildren
        {
            get
            {
                return _dataListing.Children;
            }
        }

        public virtual bool HasVariants
        {
            get
            {
                return false;
            }
        }

        public virtual bool HasLevel
        {
            get
            {
                return false;
            }
        }

        public virtual int BaseCost
        {
            get
            {
                return 0;
            }
        }

        //Constructors:   
        public DataNode(DataController controller, string Notes = "") : base("", 0, Notes, controller)
        {
            //Default constructor for data loading only
        }

        public DataNode(DataListing attribute, string notes, DataController controller, int level = 1, int pointAdj = 0) : base(attribute.Name, attribute.ID, notes, controller)
        {
            Debug.Assert(controller.SelectedListingData != null);  //Check if we have listing data...

            _dataListing = attribute;            

            if (attribute.Name == "Companion")
            {
                AddChild(new CharacterNode(AssociatedController));
            }
            if (attribute.Name == "Mind Control")
            {
                AddChild(


                        

                        AssociatedController.SelectedListingData.AttributeList.Find(n => n.Name == "Range").CreateNode("", AssociatedController, 3, -3)


                    ); ;
            }
        }

        //Methods:
        protected virtual string ProcessDescriptionValue(string valueToParse)
        {
            return valueToParse;
        }

        public List<VariantListing> GetVariants()
        {
            if (_dataListing is LevelableWithVariantDataListing variantDataListing)
            {
                //LINQ Version:
                return variantDataListing.Variants.OrderByDescending(v => v.DefaultVariant).ThenBy(v => v.Name).ToList();
            }
            else
            {
                return null;
            }
        }

        public override int GetPoints()
        {
            if (PointsUpToDate == false || FirstChild == null)
            {
                bool isItem = Name == "Item";
                bool isCompanion = Name == "Companion";
                bool isAlternateAttack = false;
                

                int VariablesOrRestrictions = 0;
                int ChildPoints = 0;

                BaseNode temp = FirstChild;
                while (temp != null)
                {
                    if (temp is DataNode tempAttribute)
                    {
                        if (tempAttribute.AttributeType == "Restriction" || tempAttribute.AttributeType == "Variable")
                        {
                            VariablesOrRestrictions += temp.GetPoints();
                        }
                        else
                        {
                            ChildPoints += temp.GetPoints();
                        }
                    }
                    else
                    {
                        ChildPoints += temp.GetPoints();
                    }

                    temp = temp.Next;
                }

                //Points should equal BaseCost +- any restrictions or variables
                _points = BaseCost;
                _points += VariablesOrRestrictions;
                                
                if (isItem)
                {
                    //item point cost calc:
                    if (ChildPoints < 2)
                    {
                        _points += 0;
                    }
                    else
                    {
                        _points += ChildPoints / 2;
                    }
                }

                //if alternate weapon attack half points:
                if (isAlternateAttack)
                {
                    _points /= 2;
                }

                PointsUpToDate = true;
            }

            return _points;
        }

        public override CalcStats GetStats()
        {
            CalcStats stats;

            stats = new CalcStats(0, 0, 0, 0);                    

            if (stats.ACV > 0 || stats.DCV > 0 || stats.Energy > 0 || stats.Health > 0)
            {
                BaseNode child = FirstChild;
                while (child != null)
                {
                    if (child is DataNode childAttribute && childAttribute.AttributeType == "Restriction")
                    {
                        stats = new CalcStats(0, 0, 0, 0);
                        break;
                    }
                    child = child.Next;
                }
            }

            return stats;
        }


        //XML:
        public override void SaveAdditionalXML(XmlTextWriter textWriter)
        {
            textWriter.WriteStartElement("AttributeStats");           
            textWriter.WriteAttributeString("HasLevel", HasLevel.ToString());
            textWriter.WriteEndElement();
        }

        public override void LoadAdditionalXML(XmlTextReader reader)
        {
            if (AssociatedController.SelectedListingData != null)
            {
                _dataListing = AssociatedController.SelectedListingData.AttributeList.Find(n => n.ID == ID);
            }

            while (reader.NodeType != XmlNodeType.None)
            {
                reader.Read();

                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.Name == "AttributeStats")
                    {
                        // loading node attributes
                        int attributeCount = reader.AttributeCount;
                        if (attributeCount > 0)
                        {
                            for (int i = 0; i < attributeCount; i++)
                            {
                                reader.MoveToAttribute(i);
                                switch (reader.Name)
                                {
                                    default:
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }
                else if (reader.NodeType == XmlNodeType.EndElement)
                {
                    if (reader.Name == "AttributeStats")
                    {
                        break;
                    }
                }
            }

        }
    }
}

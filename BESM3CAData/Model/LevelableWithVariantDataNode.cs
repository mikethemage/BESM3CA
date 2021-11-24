using BESM3CAData.Control;
using BESM3CAData.Listings;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;

namespace BESM3CAData.Model
{
    public class LevelableWithVariantDataNode : LevelableDataNode, IVariantDataNode
    {
        //Fields:
        protected VariantListing _variantListing;


        //Properties:
        protected override string BaseDescription
        {
            get
            {
                string result = AssociatedListing.Description;

                if (result == "Custom")
                {
                    if (Level >= 1 && AssociatedListing is LevelableDataListing levelableDataListing && Level <= levelableDataListing.CustomProgression.Count)
                    {
                        result = levelableDataListing.CustomProgression[(Level - 1)];
                    }
                }
                else if (result == "Variant" && _variantListing != null && _variantListing.Desc != "")
                {
                    result = _variantListing.Desc;
                }

                return result;
            }
        }

        public VariantListing Variant
        {
            get
            {
                return _variantListing;
            }
            set
            {
                if (value != null)
                {
                    _variantListing = value;
                    Name = _variantListing.FullName;
                    PointsPerLevel = _variantListing.CostperLevel;
                }
                else
                {
                    _variantListing = null;
                    Name = AssociatedListing.Name;

                    if (AssociatedListing is LevelableDataListing levelableDataListing)
                    {
                        PointsPerLevel = levelableDataListing.CostperLevel;
                    }
                    else
                    {
                        PointsPerLevel = 0;
                    }

                }
               

            }
        }

        public int VariantID
        {
            get
            {
                if (_variantListing == null)
                {
                    return 0;
                }
                else
                {
                    return _variantListing.ID;
                }
            }
            set
            {
                if (AssociatedListing is LevelableWithVariantDataListing variantDataListing && variantDataListing.Variants != null && value > 0)
                {
                    Variant = variantDataListing.Variants.First(n => n.ID == value);
                }

                
            }
        }


        //Constructors:
        public LevelableWithVariantDataNode(RPGEntity controller, string Notes = "") : base(controller, Notes)
        {
            //Default constructor for data loading only
        }

        public LevelableWithVariantDataNode(LevelableDataListing attribute, string notes, RPGEntity controller, int level = 1, int pointAdj = 0) : base(attribute, notes, controller, level, pointAdj)
        {
            Debug.Assert(controller.SelectedListingData != null);  //Check if we have listing data...

            _variantListing = null;
        }


        //Methods:
        public List<VariantListing> GetVariants()
        {
            if (AssociatedListing is IVariantDataListing variantDataListing)
            {
                //LINQ Version:
                return variantDataListing.Variants.OrderByDescending(v => v.DefaultVariant).ThenBy(v => v.Name).ToList();
            }
            else
            {
                return null;
            }
        }

        


        //XML:
        public override void SaveAdditionalXML(XmlTextWriter textWriter)
        {
            textWriter.WriteStartElement("AttributeStats");
            textWriter.WriteAttributeString("Level", Level.ToString());
            textWriter.WriteAttributeString("Variant", VariantID.ToString());
            textWriter.WriteAttributeString("Points", PointsPerLevel.ToString());
            textWriter.WriteAttributeString("PointAdj", PointAdj.ToString());
            textWriter.WriteEndElement();
        }

        public override void LoadAdditionalXML(XmlTextReader reader)
        {   
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
                                    case "Level":
                                        Level = int.Parse(reader.Value);
                                        break;
                                    case "Variant":
                                        VariantID = int.Parse(reader.Value);
                                        break;
                                    case "Points":
                                        PointsPerLevel = int.Parse(reader.Value);
                                        break;
                                    case "PointAdj":
                                        PointAdj = int.Parse(reader.Value);
                                        break;
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

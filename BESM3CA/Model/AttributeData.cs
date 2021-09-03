using System.Xml;
using System.Linq;
using BESM3CA.Templates;
using System.Diagnostics;
using System.Collections.Generic;

namespace BESM3CA.Model
{
    class AttributeData : NodeData
    {
        //Fields:
        int _Level;
        bool _HasLevel;
        int _Variant = 0;
        int _PointAdj = 0;
        int _SpecialPointsUsed = 0;
        private AttributeListing _attributeListing;
        private VariantListing _variantListing;

        public override string DisplayText
        {
            get
            {
                if (_attributeListing != null)
                {

                    if (_attributeListing.SpecialContainer || Name == "Alternate Form")
                    {
                        return Name + " (" + GetSpecialPoints() + " Left)" + " (" + GetPoints() + " Points)";
                    }
                    else
                    {
                        if (AttributeType == "Special")
                        {
                            return Name;
                        }
                        else
                        {
                            return Name + " (" + GetPoints() + " Points)";
                        }
                    }
                }
                else
                {
                    return "";
                }
            }
        }

        //Properties:
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
                if (Name == "Item")
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        public override bool HasPointsStats
        {
            get
            {
                if (Name == "Item")
                {
                    return false;
                }
                else
                {
                    if (Name == "Companion")
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
        }

        public string AttributeDescription
        {
            get
            {
                return _attributeListing.Description;
            }
        }

        public string AttributeType
        {
            get
            {
                return _attributeListing.Type;
            }
        }

        public override List<AttributeListing> PotentialChildren
        {
            get
            {
                return _asscTemplate.AttributeList.Where(n => n.ID == ID).First().Children;
            }
        }

        public bool HasVariants
        {
            get
            {
                if (_Variant != 0)
                {
                    return true;
                }

                if (_attributeListing != null && _attributeListing.RequiresVariant)
                {
                    return true;
                }

                return false;
            }
        }

        public bool HasLevel
        {
            get
            {
                return _HasLevel;
            }
        }

        public int Variant
        {
            get
            {
                return _Variant;
            }
            set
            {
                if (_asscTemplate != null)
                {
                    _variantListing = _asscTemplate.VariantList.Where(n => n.ID == value).First();
                    PointsPerLevel = _variantListing.CostperLevel;
                }
                else
                {
                    _variantListing = null;
                }

                PointsUpToDate = false;
                _Variant = value;
            }
        }

        public int AttributeID
        {
            get
            {
                return ID;
            }
        }

        public int Level
        {
            get
            {
                return _Level;
            }
        }

        public int PointsPerLevel { get; set; }

        public int PointAdj
        {
            get
            {
                return _PointAdj;
            }
            set
            {
                _PointAdj = value;
            }
        }

        public int BaseCost
        {
            get
            {
                return (PointsPerLevel * Level) + _PointAdj;
            }
        }


        //Constructors:         
        public AttributeData(string AttributeName, int AttributeID, string Notes, int Points, TemplateData useTemplate, int Level = 1, int PointAdj = 0) : base(AttributeName, AttributeID, Notes, useTemplate)
        {
            Debug.Assert(useTemplate != null);  //Check if we have a template...

            if (AttributeName == "Item")
            {
                _HasLevel = false;
            }
            else
            {
                _HasLevel = true;
                if (AttributeName == "Weapon")
                {
                    _Level = 0;
                }
            }

            PointsPerLevel = Points;
            _PointAdj = PointAdj;
            _Level = Level;

            if (AttributeName == "Companion")
            {
                AddChild(new CharacterData("", _asscTemplate));
            }
            if (AttributeName == "Mind Control")
            {
                AddChild(new AttributeData("Range", 167, "", 1, _asscTemplate, 3, -3));
            }

            _attributeListing = _asscTemplate.AttributeList.Find(n => n.ID == AttributeID);
            _variantListing = null;
        }

        public AttributeData() : base(null, 0, null, null)
        {
            //Default Constructor - currently needed for loading code
            //Todo: refactor
            
        }


        //Member Functions:
        public int GetSpecialPoints()
        {
            bool altform = false;
            if (_name == "Alternate Form")
            {
                altform = true;
            }

            AttributeListing SelectedAttribute = _asscTemplate.AttributeList.Where(n => n.ID == ID).First();

            int specialpoints = 0;

            if (SelectedAttribute.SpecialContainer || altform)
            {
                if (PointsUpToDate == false)
                {
                    GetPoints();
                }

                if (altform)
                {
                    specialpoints = Level * 10;
                }
                else
                {
                    specialpoints = Level;
                }

                specialpoints -= _SpecialPointsUsed;
            }

            return specialpoints;
        }

        public bool RaiseLevel()
        {
            if (_attributeListing.EnforceMaxLevel == false || (_attributeListing.MaxLevel != int.MaxValue && _attributeListing.MaxLevel > Level)) //need to check maxlevel
            {
                if (_HasLevel == true)
                {
                    _Level++;
                    PointsUpToDate = false;
                }
                return _HasLevel;
            }
            else
            {
                return false;
            }
        }

        public bool LowerLevel()
        {
            if (Level > 1 || (Level > 0 && _attributeListing.Name == "Weapon"))
            {
                if (_HasLevel == true)
                {
                    if (_PointAdj < 0)
                    {
                        if ((_Level * PointsPerLevel) + _PointAdj > 0)
                        {
                            _Level--;
                            PointsUpToDate = false;
                        }
                    }
                    else
                    {
                        _Level--;
                        PointsUpToDate = false;
                    }
                }
                return _HasLevel;
            }
            else
            {
                return false;
            }
        }

        public override int GetPoints()
        {
            if (PointsUpToDate == false || _FirstChild == null)
            {
                bool isItem = false;
                bool isCompanion = false;
                bool isAlternateAttack = false;
                bool isAlternateForm = false;

                isItem = (Name == "Item");
                isCompanion = (Name == "Companion");
                isAlternateForm = (Name == "Alternate Form");
                if (Variant > 0)
                {
                    VariantListing SelectedVariant = _asscTemplate.VariantList.Where(n => n.ID == Variant).First();

                    if (SelectedVariant.Name == "Alternate Attack")
                    {
                        isAlternateAttack = true;
                    }
                }

                int VariablesOrRestrictions = 0;
                int ChildPoints = 0;

                NodeData temp = _FirstChild;
                while (temp != null)
                {
                    if (temp.GetType() == typeof(AttributeData))
                    {
                        AttributeListing SelectedAttribute = _asscTemplate.AttributeList.Where(n => n.ID == ((AttributeData)temp).ID).First();
                        if (SelectedAttribute.Type == "Restriction" || SelectedAttribute.Type == "Variable")
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

                //Update special points used counter while recalculating points:
                _SpecialPointsUsed = ChildPoints;

                if (isCompanion)
                {
                    if (ChildPoints > 120)
                    {
                        _points += 2 + ((ChildPoints - 120) / 10);
                    }
                    else
                    {
                        _points += 2;
                    }
                }

                if (isItem)
                {
                    //item point cost calc:
                    if (ChildPoints < 1)
                    {
                        _points += 1;
                    }
                    else
                    {
                        _points += (ChildPoints / 2);
                    }
                }

                //***
                //if alternate weapon attack half points
                if (isAlternateAttack)
                {
                    _points /= 2;
                }
                //***                

                PointsUpToDate = true;
            }

            return _points;
        }

        public override void SaveAdditionalXML(XmlTextWriter textWriter)
        {
            textWriter.WriteStartElement("AttributeStats");
            textWriter.WriteAttributeString("Level", _Level.ToString());
            textWriter.WriteAttributeString("Variant", _Variant.ToString());
            textWriter.WriteAttributeString("HasLevel", _HasLevel.ToString());
            textWriter.WriteAttributeString("Points", PointsPerLevel.ToString());
            textWriter.WriteAttributeString("PointAdj", _PointAdj.ToString());
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
                                    case "HasLevel":
                                        _HasLevel = bool.Parse(reader.Value);
                                        break;
                                    case "Level":
                                        _Level = int.Parse(reader.Value);
                                        break;
                                    case "Variant":
                                        _Variant = int.Parse(reader.Value);
                                        break;
                                    case "Points":
                                        PointsPerLevel = int.Parse(reader.Value);
                                        break;
                                    case "PointAdj":
                                        _PointAdj = int.Parse(reader.Value);
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
            if (_asscTemplate != null)
            {
                _attributeListing = _asscTemplate.AttributeList.Find(n => n.ID == AttributeID);
            }
        }
    }
}

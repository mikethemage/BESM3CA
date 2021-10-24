using BESM3CAData.Listings;
using BESM3CAData.Control;
using org.mariuszgromada.math.mxparser;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;

namespace BESM3CAData.Model
{
    public class AttributeNode : BaseNode
    {
        //Fields:             
        private int _specialPointsUsed = 0;
        private AttributeListing _attributeListing;
        private VariantListing _variantListing;

        //Properties:
        public override string DisplayText
        {
            get
            {
                if (_attributeListing != null)
                {
                    if (_attributeListing.SpecialContainer || Name == "Alternate Form")
                    {
                        return $"{Name} ({GetSpecialPoints()} Left) ({GetPoints()} Points)";
                    }
                    else
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
                //Need to process attribute description to calculate numeric components
                string baseDescription = _attributeListing.Description;

                if (baseDescription == "Custom")
                {
                    if (Level >= 1 && Level <= _attributeListing.CustomProgression.Count)
                    {
                        baseDescription = _attributeListing.CustomProgression[(Level - 1)];
                    }
                }
                else if (baseDescription == "Variant" && _variantListing != null && _variantListing.Desc != "")
                {
                    baseDescription = _variantListing.Desc;
                }

                string completedDescription = "";

                while (baseDescription != null)
                {
                    string[] pieces = baseDescription.Split('[', 2);
                    completedDescription += pieces[0];
                    if (pieces.Length > 1)
                    {
                        baseDescription = pieces[1];
                        pieces = baseDescription.Split(']', 2);

                        completedDescription += ProcessDescriptionValue(pieces[0]);

                        if (pieces.Length > 1)
                        {
                            baseDescription = pieces[1];
                        }
                        else
                        {
                            baseDescription = null;
                        }
                    }
                    else
                    {
                        baseDescription = null;
                    }
                }

                return completedDescription;
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
                return _attributeListing.Children;
            }
        }

        public bool HasVariants
        {
            get
            {
                if (_variantListing != null)
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
            get; private set;
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
                if (_attributeListing.Variants != null && value > 0)
                {
                    Variant = _attributeListing.Variants.First(n => n.ID == value);
                }

                PointsUpToDate = false;
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
                    Name = _attributeListing.Name;
                    PointsPerLevel = _attributeListing.CostperLevel;
                }
                PointsUpToDate = false;

            }
        }

        public int Level { get; private set; }

        public int PointsPerLevel { get; set; }

        public int PointAdj { get; private set; }

        public int BaseCost
        {
            get
            {
                return (PointsPerLevel * Level) + PointAdj;
            }
        }


        //Constructors:   
        public AttributeNode(DataController controller, string Notes = "") : base("", 0, Notes, controller)
        {
            //Default constructor for data loading only
        }

        public AttributeNode(AttributeListing attribute, string notes, DataController controller, int level = 1, int pointAdj = 0) : base(attribute.Name, attribute.ID, notes, controller)
        {
            Debug.Assert(controller.SelectedListingData != null);  //Check if we have listing data...

            if (attribute.Name == "Item")
            {
                HasLevel = false;
            }
            else
            {
                HasLevel = true;
                if (attribute.Name == "Weapon")
                {
                    Level = 0;
                }
            }

            PointAdj = pointAdj;
            Level = level;

            _attributeListing = attribute;

            UpdatePointsPerLevel();

            _variantListing = null;

            if (attribute.Name == "Companion")
            {
                AddChild(new CharacterNode(AssociatedController));
            }
            if (attribute.Name == "Mind Control")
            {
                AddChild(new AttributeNode(AssociatedController.SelectedListingData.AttributeList.Find(n => n.Name == "Range"), "", AssociatedController, 3, -3));
            }
        }


        //Methods:
        private string ProcessDescriptionValue(string valueToParse)
        {
            //Substitute "n" for Level:
            if (valueToParse.Contains("fn"))
            {
                if (int.TryParse(valueToParse.Replace("fn", ""), out int i))
                {
                    return AssociatedController.SelectedListingData.GetProgression("Fast", i - 1 + Level).ToString();
                }
            }
            if (valueToParse.Contains("mn"))
            {
                if (int.TryParse(valueToParse.Replace("mn", ""), out int i))
                {
                    return AssociatedController.SelectedListingData.GetProgression("Medium", i - 1 + Level).ToString();
                }
            }
            if (valueToParse.Contains("sn"))
            {
                if (int.TryParse(valueToParse.Replace("sn", ""), out int i))
                {
                    return AssociatedController.SelectedListingData.GetProgression("Slow", i - 1 + Level).ToString();
                }
            }
            if (valueToParse.Contains("tn")) //Time 
            {
                if (int.TryParse(valueToParse.Replace("tn", ""), out int i))
                {
                    return AssociatedController.SelectedListingData.GetProgression("Time", i - 1 + Level).ToString();
                }
            }
            if (valueToParse.Contains("trn")) //Time Reversed
            {
                if (int.TryParse(valueToParse.Replace("trn", ""), out int i))
                {
                    return AssociatedController.SelectedListingData.GetProgression("Time", i - (Level - 1));
                }
            }
            if (valueToParse.Contains("an"))
            {
                if (int.TryParse(valueToParse.Replace("an", ""), out int i))
                {
                    return AssociatedController.SelectedListingData.GetProgression("Area", i - 1 + Level).ToString();
                }
            }
            if (valueToParse.Contains("rn"))
            {
                if (int.TryParse(valueToParse.Replace("rn", ""), out int i))
                {
                    return AssociatedController.SelectedListingData.GetProgression("Range", i - 1 + Level).ToString();
                }
            }
            if (valueToParse.Contains("tgn"))
            {
                if (int.TryParse(valueToParse.Replace("tgn", ""), out int i))
                {
                    return AssociatedController.SelectedListingData.GetProgression("Targets", i - 1 + Level).ToString();
                }
            }
            if (valueToParse.Contains("grn"))
            {
                if (int.TryParse(valueToParse.Replace("grn", ""), out int i))
                {
                    return AssociatedController.SelectedListingData.GetProgression("Growth", i - 1 + Level).ToString();
                }
            }

            valueToParse = valueToParse.Replace("n", Level.ToString());

            Expression e = new Expression(valueToParse);

            return e.calculate().ToString();
        }

        private void UpdatePointsPerLevel()
        {
            if (AssociatedController.SelectedGenreIndex > -1 && _attributeListing.GenrePoints != null && _attributeListing.GenrePoints.Count > AssociatedController.SelectedGenreIndex)
            {
                PointsPerLevel = _attributeListing.GenrePoints[AssociatedController.SelectedGenreIndex];
            }
            else
            {
                PointsPerLevel = _attributeListing.CostperLevel;
            }
        }

        public override void InvalidateGenrePoints()
        {
            if (_attributeListing.GenrePoints != null)
            {
                PointsUpToDate = false;
                UpdatePointsPerLevel();
            }

            base.InvalidateGenrePoints();
        }

        public int GetSpecialPoints()
        {
            bool altform = false;
            if (Name == "Alternate Form")
            {
                altform = true;
            }

            int specialpoints = 0;

            if (_attributeListing.SpecialContainer || altform)
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

                specialpoints -= _specialPointsUsed;
            }

            return specialpoints;
        }

        public bool RaiseLevel()
        {
            if (_attributeListing.EnforceMaxLevel == false || (_attributeListing.MaxLevel != int.MaxValue && _attributeListing.MaxLevel > Level)) //need to check maxlevel
            {
                if (HasLevel == true)
                {
                    Level++;
                    PointsUpToDate = false;
                }
                return HasLevel;
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
                if (HasLevel == true)
                {
                    if (PointAdj < 0)
                    {
                        if ((Level * PointsPerLevel) + PointAdj > 0)
                        {
                            Level--;
                            PointsUpToDate = false;
                        }
                    }
                    else
                    {
                        Level--;
                        PointsUpToDate = false;
                    }
                }
                return HasLevel;
            }
            else
            {
                return false;
            }
        }

        public List<VariantListing> GetVariants()
        {
            if (HasVariants)
            {
                //LINQ Version:
                return _attributeListing.Variants.OrderByDescending(v => v.DefaultVariant).ThenBy(v => v.Name).ToList();
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

                if (VariantID > 0)
                {
                    if (_variantListing.Name == "Alternate Attack")
                    {
                        isAlternateAttack = true;
                    }
                }

                int VariablesOrRestrictions = 0;
                int ChildPoints = 0;

                BaseNode temp = FirstChild;
                while (temp != null)
                {
                    if (temp is AttributeNode tempAttribute)
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

                //Update special points used counter while recalculating points:
                _specialPointsUsed = ChildPoints;

                if (isCompanion)
                {
                    if (ChildPoints > 120)
                    {
                        _points += (2 + ((ChildPoints - 120) / 10)) * Level;
                    }
                    else
                    {
                        _points += 2 * Level;
                    }
                }

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

            switch (Name)
            {
                case "Tough":
                    stats = new CalcStats(Level * 5, 0, 0, 0);
                    break;
                case "Energy Bonus":
                    stats = new CalcStats(0, Level * 5, 0, 0);
                    break;
                case "Attack Combat Mastery":
                    stats = new CalcStats(0, 0, Level, 0);
                    break;
                case "Defence Combat Mastery":
                    stats = new CalcStats(0, 0, 0, Level);
                    break;
                default:
                    stats = new CalcStats(0, 0, 0, 0);
                    break;
            }

            if (stats.ACV > 0 || stats.DCV > 0 || stats.Energy > 0 || stats.Health > 0)
            {
                BaseNode child = FirstChild;
                while (child != null)
                {
                    if (child is AttributeNode childAttribute && childAttribute.AttributeType == "Restriction")
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
            textWriter.WriteAttributeString("Level", Level.ToString());
            textWriter.WriteAttributeString("Variant", VariantID.ToString());
            textWriter.WriteAttributeString("HasLevel", HasLevel.ToString());
            textWriter.WriteAttributeString("Points", PointsPerLevel.ToString());
            textWriter.WriteAttributeString("PointAdj", PointAdj.ToString());
            textWriter.WriteEndElement();
        }

        public override void LoadAdditionalXML(XmlTextReader reader)
        {
            if (AssociatedController.SelectedListingData != null)
            {
                _attributeListing = AssociatedController.SelectedListingData.AttributeList.Find(n => n.ID == ID);
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
                                    case "HasLevel":
                                        HasLevel = bool.Parse(reader.Value);
                                        break;
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

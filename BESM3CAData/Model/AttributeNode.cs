﻿using BESM3CAData.Control;
using BESM3CAData.Listings;
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
        private DataListing _dataListing;
        private VariantListing _variantListing;

        //Properties:
        public override string DisplayText
        {
            get
            {
                if (_dataListing != null)
                {
                    if (_dataListing is SpecialContainerWithVariantDataListing attributeListing && (attributeListing.SpecialContainer))
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
                return _dataListing.HasLevel;
            }
        }

        public override bool HasPointsStats
        {
            get
            {
                if (_dataListing.HasLevel)
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
                string baseDescription = _dataListing.Description;

                if (baseDescription == "Custom")
                {
                    if (Level >= 1 && _dataListing is LevelableDataListing levelableDataListing && Level <= levelableDataListing.CustomProgression.Count)
                    {
                        baseDescription = levelableDataListing.CustomProgression[(Level - 1)];
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

        public bool HasVariants
        {
            get
            {
                if (_variantListing != null)
                {
                    return true;
                }

                if (_dataListing is LevelableWithVariantDataListing variantDataListing && variantDataListing.RequiresVariant)
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
                if (_dataListing is LevelableWithVariantDataListing variantDataListing && variantDataListing.Variants != null && value > 0)
                {
                    Variant = variantDataListing.Variants.First(n => n.ID == value);
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
                    Name = _dataListing.Name;

                    if (_dataListing is LevelableDataListing levelableDataListing)
                    {
                        PointsPerLevel = levelableDataListing.CostperLevel;
                    }
                    else
                    {
                        PointsPerLevel = 0;
                    }

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

        public AttributeNode(DataListing attribute, string notes, DataController controller, int level = 1, int pointAdj = 0) : base(attribute.Name, attribute.ID, notes, controller)
        {
            Debug.Assert(controller.SelectedListingData != null);  //Check if we have listing data...

            _dataListing = attribute;

            if (_dataListing.HasLevel)
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
            if (AssociatedController.SelectedGenreIndex > -1 && _dataListing is MultiGenreDataListing skillDataListing && skillDataListing.GenrePoints != null && skillDataListing.GenrePoints.Count > AssociatedController.SelectedGenreIndex)
            {
                PointsPerLevel = skillDataListing.GenrePoints[AssociatedController.SelectedGenreIndex];
            }
            else if (_dataListing is LevelableDataListing levelableDataListing)
            {
                PointsPerLevel = levelableDataListing.CostperLevel;
            }
            else
            {
                PointsPerLevel = 0;
            }
        }

        public override void InvalidateGenrePoints()
        {
            if (_dataListing is MultiGenreDataListing skillDataListing && skillDataListing.GenrePoints != null)
            {
                PointsUpToDate = false;
                UpdatePointsPerLevel();
            }

            base.InvalidateGenrePoints();
        }

        public int GetSpecialPoints()
        {
            int specialpoints = 0;

            if (_dataListing is SpecialContainerWithVariantDataListing attributeListing)
            {
                if (attributeListing.SpecialContainer)//|| altform)
                {
                    if (PointsUpToDate == false)
                    {
                        GetPoints();
                    }

                    specialpoints = Level * attributeListing.SpecialPointsPerLevel;

                    specialpoints -= _specialPointsUsed;
                }
            }

            return specialpoints;
        }

        public bool RaiseLevel()
        {
            if (_dataListing is LevelableDataListing levelableDataListing)
            {
                if (levelableDataListing.EnforceMaxLevel == false || (levelableDataListing.MaxLevel != int.MaxValue && levelableDataListing.MaxLevel > Level))
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
            else
            {
                return false;
            }
        }

        public bool LowerLevel()
        {
            if (Level > 1 || (Level > 0 && _dataListing.Name == "Weapon"))
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

using BESM3CAData.Control;
using BESM3CAData.Listings;
using org.mariuszgromada.math.mxparser;
using System.Diagnostics;
using System.Xml;

namespace BESM3CAData.Model
{
    public class LevelableDataNode : DataNode, IPointsDataNode
    {
        //Properties:
        public int Level { get; protected set; }
        public int PointsPerLevel { get; set; }
        public int PointAdj { get; protected set; }
        public virtual int BaseCost
        {
            get
            {
                return (PointsPerLevel * Level) + PointAdj;
            }
        }
        protected override string BaseDescription
        {
            get
            {
                string result = _dataListing.Description;

                if (result == "Custom")
                {
                    if (Level >= 1 && _dataListing is LevelableDataListing levelableDataListing && Level <= levelableDataListing.CustomProgression.Count)
                    {
                        result = levelableDataListing.CustomProgression[(Level - 1)];
                    }
                }

                return result;
            }
        }


        //Constructors:
        public LevelableDataNode(DataController controller, string Notes = "") : base(controller, Notes)
        {
            //Default constructor for data loading only
        }

        public LevelableDataNode(LevelableDataListing attribute, string notes, DataController controller, int level = 1, int pointAdj = 0) : base(attribute, notes, controller, level, pointAdj)
        {
            Debug.Assert(controller.SelectedListingData != null);  //Check if we have listing data...

            if (attribute.Name == "Weapon")
            {
                Level = 0;
            }
            else
            {
                Level = level;
            }

            PointAdj = pointAdj;

            UpdatePointsPerLevel();
        }


        //Methods:
        public override int GetPoints()
        {
            if (PointsUpToDate == false || FirstChild == null)
            {
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

        protected override string ProcessDescriptionValue(string valueToParse)
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

        protected virtual void UpdatePointsPerLevel()
        {
            if (_dataListing is LevelableDataListing levelableDataListing)
            {
                PointsPerLevel = levelableDataListing.CostperLevel;
            }
            else
            {
                PointsPerLevel = 0;
            }
        }

        public bool RaiseLevel()
        {
            if (_dataListing is LevelableDataListing levelableDataListing)
            {
                if (levelableDataListing.EnforceMaxLevel == false || (levelableDataListing.MaxLevel != int.MaxValue && levelableDataListing.MaxLevel > Level))
                {
                    Level++;
                    PointsUpToDate = false;
                    return true;
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

                return true;
            }
            else
            {
                return false;
            }
        }


        //XML:
        public override void SaveAdditionalXML(XmlTextWriter textWriter)
        {
            textWriter.WriteStartElement("AttributeStats");
            textWriter.WriteAttributeString("Level", Level.ToString());
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
                                    case "Level":
                                        Level = int.Parse(reader.Value);
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

using System.Xml;
using System.Linq;
using BESM3CAData.Templates;
using System.Diagnostics;
using System.Collections.Generic;
using org.mariuszgromada.math.mxparser;

namespace BESM3CAData.Model
{
    public class AttributeData : NodeData
    {
        //Fields:
        int _Level;
        bool _HasLevel;
        int _Variant = 0;
        int _PointAdj = 0;
        int _SpecialPointsUsed = 0;
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

        private string ProcessDescriptionValue(string valueToParse)
        {
            //Substitute "n" for Level:
            if (valueToParse.Contains("fn"))
            {
                if (int.TryParse(valueToParse.Replace("fn", ""), out int i))
                {
                    return AssociatedController.SelectedTemplate.GetProgression("Fast", i - 1 + Level).ToString();
                }
            }
            if (valueToParse.Contains("mn"))
            {
                if (int.TryParse(valueToParse.Replace("mn", ""), out int i))
                {
                    return AssociatedController.SelectedTemplate.GetProgression("Medium", i - 1 + Level).ToString();
                }
            }
            if (valueToParse.Contains("sn"))
            {
                if (int.TryParse(valueToParse.Replace("sn", ""), out int i))
                {
                    return AssociatedController.SelectedTemplate.GetProgression("Slow", i - 1 + Level).ToString();
                }
            }
            if (valueToParse.Contains("tn"))// Time 
            {
                if (int.TryParse(valueToParse.Replace("tn", ""), out int i))
                {
                    return AssociatedController.SelectedTemplate.GetProgression("Time", i - 1 + Level).ToString();
                }
            }
            if (valueToParse.Contains("trn"))// Time Reversed
            {
                if (int.TryParse(valueToParse.Replace("trn", ""), out int i))
                {
                    return AssociatedController.SelectedTemplate.GetProgression("Time", i - (Level - 1));
                }
            }
            if (valueToParse.Contains("an"))
            {
                if (int.TryParse(valueToParse.Replace("an", ""), out int i))
                {
                    return AssociatedController.SelectedTemplate.GetProgression("Area", i - 1 + Level).ToString();
                }
            }
            if (valueToParse.Contains("rn"))
            {
                if (int.TryParse(valueToParse.Replace("rn", ""), out int i))
                {
                    return AssociatedController.SelectedTemplate.GetProgression("Range", i - 1 + Level).ToString();
                }
            }
            if (valueToParse.Contains("tgn"))
            {
                if (int.TryParse(valueToParse.Replace("tgn", ""), out int i))
                {
                    return AssociatedController.SelectedTemplate.GetProgression("Targets", i - 1 + Level).ToString();
                }
            }
            if (valueToParse.Contains("grn"))
            {
                if (int.TryParse(valueToParse.Replace("grn", ""), out int i))
                {
                    return AssociatedController.SelectedTemplate.GetProgression("Growth", i - 1 + Level).ToString();
                }
            }


            valueToParse = valueToParse.Replace("n", Level.ToString());

            Expression e = new Expression(valueToParse);

            return e.calculate().ToString();
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
                return AssociatedController.SelectedTemplate.AttributeList.Where(n => n.ID == ID).First().Children;
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
                if (AssociatedController.SelectedTemplate != null)
                {
                    _variantListing = AssociatedController.SelectedTemplate.VariantList.Where(n => n.ID == value).First();
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
        public AttributeData(string attributeName, int attributeID, string notes, Controller controller, int level = 1, int pointAdj = 0) : base(attributeName, attributeID, notes, controller)
        {
            Debug.Assert(controller.SelectedTemplate != null);  //Check if we have a template...

            if (attributeName == "Item")
            {
                _HasLevel = false;
            }
            else
            {
                _HasLevel = true;
                if (attributeName == "Weapon")
                {
                    _Level = 0;
                }
            }

            _PointAdj = pointAdj;
            _Level = level;

            if (attributeID != 0)
            {
                _attributeListing = AssociatedController.SelectedTemplate.AttributeList.Find(n => n.ID == attributeID);

                UpdatePointsPerLevel();
            }
            _variantListing = null;

            if (attributeName == "Companion")
            {
                AddChild(new CharacterData(AssociatedController/*, ""*/));
            }
            if (attributeName == "Mind Control")
            {
                AddChild(new AttributeData("Range", 167, "", AssociatedController, 3, -3));
            }
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


        //Member Functions:

        public override void InvalidateGenrePoints()
        {
            if(_attributeListing.GenrePoints!=null)
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

            AttributeListing SelectedAttribute = AssociatedController.SelectedTemplate.AttributeList.Where(n => n.ID == ID).First();

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

        public List<ListItems> GetVariants()
        {
            if (HasVariants)
            {
                //LINQ Version:
                List<ListItems> FilteredVarList = (from Att in AssociatedController.SelectedTemplate.AttributeList
                                                   where Att.ID == AttributeID
                                                   from Vari in AssociatedController.SelectedTemplate.VariantList
                                                   where Att.ID == Vari.AttributeID
                                                   orderby Vari.DefaultVariant descending, Vari.Name
                                                   select new ListItems
                                                   (
                                                       Att.Name + " [" + Vari.Name + "]",
                                                       Vari.ID
                                                   )).ToList();

                return FilteredVarList;
            }
            else
            {
                return null;
            }
        }

        public override int GetPoints()
        {
            if (PointsUpToDate == false || _firstChild == null)
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
                    VariantListing SelectedVariant = AssociatedController.SelectedTemplate.VariantList.Where(n => n.ID == Variant).First();

                    if (SelectedVariant.Name == "Alternate Attack")
                    {
                        isAlternateAttack = true;
                    }
                }

                int VariablesOrRestrictions = 0;
                int ChildPoints = 0;

                NodeData temp = _firstChild;
                while (temp != null)
                {
                    if (temp.GetType() == typeof(AttributeData))
                    {
                        AttributeListing SelectedAttribute = AssociatedController.SelectedTemplate.AttributeList.Where(n => n.ID == ((AttributeData)temp).ID).First();
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


        //XML:
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
            if (AssociatedController.SelectedTemplate != null)
            {
                _attributeListing = AssociatedController.SelectedTemplate.AttributeList.Find(n => n.ID == AttributeID);
            }
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
                NodeData child = Children;
                while (child != null)
                {
                    if (child.GetType() == typeof(AttributeData))
                    {
                        if (((AttributeData)child).AttributeType == "Restriction")
                        {
                            stats = new CalcStats(0, 0, 0, 0);
                            break;
                        }
                    }
                    child = child.Next;
                }
            }

            return stats;
        }

    }
}

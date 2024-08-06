﻿using BESM3CAData.Control;
using BESM3CAData.Listings;
using org.mariuszgromada.math.mxparser;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using System.Xml;

namespace BESM3CAData.Model
{
    public class LevelableDataNode : DataNode, IPointsDataNode, IVariantDataNode
    {
        public override void ChildPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            base.ChildPropertyChanged(sender, e);
            if (sender is BaseNode)
            {
                if (e.PropertyName == nameof(BaseNode.Points))
                {
                    RefreshVariablesOrRestrictions();
                }
            }
        }

        public override void RefreshAll()
        {
            foreach (BaseNode item in Children)
            {
                item.RefreshAll();
            }
            RefreshBaseCost();
            RefreshPoints();
            RefreshDisplayText();
        }

        protected override void RefreshDisplayText()
        {

            if (Variant != null)
            {                
                DisplayText = $"{Variant.FullName} ({Points} Points)";
            }
            else
            {
                DisplayText = $"{Name} ({Points} Points)";
            }            
        }

        //Properties:
        protected int _level;
        public virtual int Level
        {
            get
            {
                return _level;
            }
            protected set
            {
                int originalLevel = _level;
                _level = value;
                if (originalLevel != _level)
                {
                    OnPropertyChanged(nameof(Level));
                    RefreshBaseCost();
                    RefreshDescription();
                }
            }
        }

        public int RequiredLevels { get; private set; } = 0;
        public int FreeLevels { get; private set; } = 0;

        protected virtual void RefreshBaseCost()
        {
            BaseCost = PointsPerLevel * (Level - FreeLevels);
        }
        protected override void Children_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs args)
        {
            RefreshVariablesOrRestrictions();
        }

        private int _variablesOrRestrictions;
        public virtual int VariablesOrRestrictions
        {
            get
            {
                return _variablesOrRestrictions;
            }
            set
            {
                int originalVariablesOrRestrictions = _variablesOrRestrictions;
                _variablesOrRestrictions = value;
                if (originalVariablesOrRestrictions != _variablesOrRestrictions)
                {
                    OnPropertyChanged(nameof(VariablesOrRestrictions));
                    RefreshPoints();
                }
            }
        }
        
        protected void RefreshVariablesOrRestrictions()
        {
            int tempVariablesOrRestrictions = 0;
            foreach (var temp in Children)
            {
                if (temp is DataNode tempAttribute)
                {
                    if (tempAttribute.AttributeType == "Restriction" || tempAttribute.AttributeType == "Variable")
                    {
                        tempVariablesOrRestrictions += temp.Points;
                    }
                }             
            }
            VariablesOrRestrictions = tempVariablesOrRestrictions;
        }


        protected override void RefreshPoints()
        {
            int tempPoints = BaseCost;
            tempPoints += VariablesOrRestrictions;
            Points = tempPoints;
        }

        private int _pointsPerLevel;

        public int PointsPerLevel
        {
            get
            {
                return _pointsPerLevel;
            }
            set
            {
                if (value != _pointsPerLevel)
                {
                    _pointsPerLevel = value;
                    OnPropertyChanged(nameof(PointsPerLevel));
                    RefreshBaseCost();
                }

            }
        }

       

        protected override string BaseDescription
        {
            get
            {
                string result = AssociatedListing?.Description ?? "";

                if (AssociatedListing is LevelableDataListing levelableDataListing)
                {
                    if(levelableDataListing.Progression != null && levelableDataListing.Progression.CustomProgression && levelableDataListing.Progression.ProgressionsList != null)
                    {
                        if (Level >= 1 && Level <= levelableDataListing.Progression.ProgressionsList.Count)
                        {
                            result = levelableDataListing.Progression.ProgressionsList[(Level - 1)];
                        }
                    }
                    else if (result == "Variant" && _variantListing != null && _variantListing.Desc != "")
                    {
                        result = _variantListing.Desc ?? "";
                    }
                }     

                return result;
            }
        }

        //Constructors:
        public LevelableDataNode(RPGEntity controller, string Notes = "") : base(controller, Notes)
        {
            //Default constructor for data loading only
            CreateRaiseLevelCommand();
            CreateLowerLevelCommand();
        }

        public LevelableDataNode(LevelableDataListing attribute, bool isloading, string notes, RPGEntity controller, int level = 1, int freeLevels = 0, int requiredLevels = 0, bool isFreebie = false) : base(attribute, isloading, notes, controller, isFreebie)
        {
            Debug.Assert(controller.SelectedListingData != null);  //Check if we have listing data...
            FreeLevels = freeLevels;
            RequiredLevels = requiredLevels;

            if (!isloading)
            {
                if (attribute.Variants != null && attribute.Variants.Count > 0)
                {
                    VariantListing? defaultVariant = attribute.Variants.Where(x=>x.DefaultVariant).FirstOrDefault();
                    if (defaultVariant != null)
                    {
                        Variant=defaultVariant;
                    }
                }
            }

            UpdatePointsPerLevel();
            if (attribute.Name == "Weapon")
            {
                Level = 0;
            }
            else
            {
                Level = level;
            }
            CreateRaiseLevelCommand();
            CreateLowerLevelCommand();
        }

        //Methods:
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
                foreach (var child in Children)
                {
                    if (child is DataNode childAttribute && childAttribute.AttributeType == "Restriction")
                    {
                        //If any child restrictions are present then don't automatically add the current attribute to the Character's stats as they do not always apply
                        stats = new CalcStats(0, 0, 0, 0);
                        break;
                    }                 
                }
            }

            return stats;
        }

        protected override string ProcessDescriptionValue(string valueToParse)
        {
            if(AssociatedController.SelectedListingData == null)
            {
                return valueToParse;
            }

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
            if (AssociatedListing is LevelableDataListing levelableDataListing)
            {
                PointsPerLevel = levelableDataListing.CostperLevel;
            }
            else
            {
                PointsPerLevel = 0;
            }
        }

        public bool CanRaiseLevel()
        {
            if (AssociatedListing is LevelableDataListing levelableDataListing &&
                (levelableDataListing.EnforceMaxLevel == false || (levelableDataListing.MaxLevel != int.MaxValue && levelableDataListing.MaxLevel > Level)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void RaiseLevel()
        {
            bool canRaiseLevel = CanRaiseLevel();
            if (canRaiseLevel)
            {
                Level++;
                RaiseLevelCommand?.RaiseCanExecuteChanged();
                LowerLevelCommand?.RaiseCanExecuteChanged();
            }
            //return canRaiseLevel;
        }

        public RelayCommand? RaiseLevelCommand
        {
            get; private set;
        }

        private void CreateRaiseLevelCommand()
        {
            RaiseLevelCommand = new RelayCommand(RaiseLevel, CanRaiseLevel);
        }


        public bool CanLowerLevel()
        {
            if (Level > 1 || (Level > 0 && AssociatedListing?.Name == "Weapon"))
            {
                if (FreeLevels + RequiredLevels > 0)
                {
                    if (Level > FreeLevels + RequiredLevels)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        public void LowerLevel()
        {
            bool canRaiseLevel = CanLowerLevel();
            if (canRaiseLevel)
            {
                Level--;
                LowerLevelCommand?.RaiseCanExecuteChanged();
                RaiseLevelCommand?.RaiseCanExecuteChanged();
            }            
        }

        private void CreateLowerLevelCommand()
        {
            LowerLevelCommand = new RelayCommand(LowerLevel, CanLowerLevel);
        }

        public RelayCommand? LowerLevelCommand
        {
            get; private set;
        }

        //XML:
        //public override void SaveAdditionalXML(XmlTextWriter textWriter)
        //{
        //    textWriter.WriteStartElement("AttributeStats");
        //    textWriter.WriteAttributeString("Level", Level.ToString());
        //    textWriter.WriteAttributeString("Points", PointsPerLevel.ToString());
        //    //textWriter.WriteAttributeString("PointAdj", PointAdj.ToString());
        //    textWriter.WriteEndElement();
        //}

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
                                        //VariantID = int.Parse(reader.Value);
                                        break;

                                    case "Points":
                                        PointsPerLevel = int.Parse(reader.Value);
                                        break;

                                    //case "PointAdj":
                                    //    PointAdj = int.Parse(reader.Value);
                                    //    break;

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

        public List<VariantListing>? GetVariants()
        {
            if (AssociatedListing is LevelableDataListing variantDataListing && variantDataListing.Variants != null && variantDataListing.Variants.Count > 0)
            {
                //LINQ Version:
                return variantDataListing.Variants.OrderByDescending(v => v.DefaultVariant).ThenBy(v => v.Name).ToList();
            }
            else
            {
                return null;
            }
        }

        protected VariantListing? _variantListing;
        public VariantListing? Variant
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
                    PointsPerLevel = _variantListing.CostperLevel;
                }
                else
                {
                    _variantListing = null;                    

                    if (AssociatedListing is LevelableDataListing levelableDataListing)
                    {
                        PointsPerLevel = levelableDataListing.CostperLevel;
                    }
                    else
                    {
                        PointsPerLevel = 0;
                    }
                }
                RefreshDisplayText();
            }
        }

        public ObservableCollection<VariantListing> VariantList { get; set; } = new ObservableCollection<VariantListing>();

        private void VariantPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (IsSelected)
            {
                if (sender is VariantListing variantListing)
                {
                    if (e.PropertyName == nameof(VariantListing.IsSelected) && variantListing.IsSelected == true)
                    {
                        Variant = variantListing;
                    }
                }
            }
        }

        private DataListing? _associatedListing;

        public override DataListing? AssociatedListing
        {
            get
            {
                return _associatedListing;
            }
            protected set
            {
                if (value != _associatedListing)
                {
                    if (_associatedListing is LevelableDataListing oldVariantDataListing && oldVariantDataListing.Variants != null && oldVariantDataListing.Variants.Count > 0)
                    {
                        foreach (VariantListing oldVL in VariantList)
                        {
                            oldVL.PropertyChanged -= VariantPropertyChanged;
                        }

                        VariantList.Clear();
                    }

                    _associatedListing = value;

                    if (_associatedListing is LevelableDataListing newVariantDataListing && newVariantDataListing.Variants != null && newVariantDataListing.Variants.Count > 0)
                    {
                        foreach (VariantListing newVL in newVariantDataListing.Variants)
                        {
                            VariantList.Add(newVL);
                            newVL.PropertyChanged += VariantPropertyChanged;
                            if (newVL.DefaultVariant)
                            {
                                Variant = newVL;
                            }
                        }
                    }
                }
            }
        }
    }
}

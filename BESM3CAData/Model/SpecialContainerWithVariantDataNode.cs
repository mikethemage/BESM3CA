using BESM3CAData.Control;
using BESM3CAData.Listings;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace BESM3CAData.Model
{
    public class SpecialContainerWithVariantDataNode : LevelableWithVariantDataNode, IVariantDataNode, ISpecialContainerDataNode
    {

        protected override void Children_CollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            RefreshVariablesOrRestrictions();
            RefreshChildPoints();
        }
        public override int Level
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
                    RefreshSpecialPoints();
                }
            }
        }

        //Fields:        
        private int _specialPointsUsed = 0;

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

        protected override void RefreshDisplayText()
        {
            DisplayText = $"{Name} ({SpecialPoints} Left) ({Points} Points)";
        }


        private int _specialPoints;
        public int SpecialPoints
        {
            get
            {
                return _specialPoints;
            }
            set
            {
                if(value!=_specialPoints)
                {
                    _specialPoints = value;
                    OnPropertyChanged(nameof(SpecialPoints));
                    RefreshDisplayText();
                }
                
            }
        }
        public void RefreshSpecialPoints()
        {

            int tempSpecialPoints = 0;

            if (AssociatedListing is ISpecialContainerDataListing attributeListing)
            {
                _specialPointsUsed = ChildPoints;

                tempSpecialPoints = Level * attributeListing.SpecialPointsPerLevel;

                tempSpecialPoints -= _specialPointsUsed;

            }

            SpecialPoints = tempSpecialPoints;
        }

        public int GetSpecialPoints()
        {
            int specialpoints = 0;

            if (AssociatedListing is ISpecialContainerDataListing attributeListing)
            {
                _specialPointsUsed = ChildPoints;

                specialpoints = Level * attributeListing.SpecialPointsPerLevel;

                specialpoints -= _specialPointsUsed;

            }

            return specialpoints;
        }

        protected override void RefreshPoints()
        {
            //Points should equal BaseCost +- any restrictions or variables
            int tempPoints = BaseCost;
            tempPoints += VariablesOrRestrictions;

            //Update special points used counter while recalculating points:
            _specialPointsUsed = ChildPoints;

            Points = tempPoints;
        }

        private int _childPoints;
        public virtual int ChildPoints
        {
            get
            {
                return _childPoints;
            }
            set
            {
                int originalChildPoints = _childPoints;
                _childPoints = value;
                if (originalChildPoints != _childPoints)
                {
                    OnPropertyChanged(nameof(ChildPoints));
                    RefreshPoints();
                    RefreshSpecialPoints();
                }
            }
        }

        public override void ChildPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is BaseNode baseNode)
            {
                if (e.PropertyName == nameof(BaseNode.Points))
                {
                    RefreshVariablesOrRestrictions();
                    RefreshChildPoints();
                }
            }
        }

        private void RefreshChildPoints()
        {
            int tempChildPoints = 0;

            BaseNode temp = FirstChild;
            while (temp != null)
            {
                if (temp is DataNode tempAttribute)
                {
                    if (tempAttribute.AttributeType == "Restriction" || tempAttribute.AttributeType == "Variable")
                    {

                    }
                    else
                    {
                        tempChildPoints += temp.Points;
                    }
                }
                else
                {
                    tempChildPoints += temp.Points;
                }

                temp = temp.Next;
            }
            ChildPoints = tempChildPoints;
        }


        //Constructors:
        public SpecialContainerWithVariantDataNode(RPGEntity controller, string Notes = "") : base(controller, Notes)
        {

        }

        public SpecialContainerWithVariantDataNode(SpecialContainerWithVariantDataListing attribute, string notes, RPGEntity controller, int level = 1, int pointAdj = 0) : base(attribute, notes, controller, level, pointAdj)
        {

        }




    }
}

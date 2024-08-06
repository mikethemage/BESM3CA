﻿using BESM3CAData.Control;
using BESM3CAData.Listings;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace BESM3CAData.Model
{
    public class CompanionDataNode : LevelableDataNode
    {
        //Constructor:
        public CompanionDataNode(CompanionDataListing attribute, bool isLoading, string notes, RPGEntity controller, int level = 1, int freeLevels = 0, int requiredLevels = 0, bool isFreebie=false) : base(attribute, isLoading, notes, controller, level, freeLevels, requiredLevels, isFreebie)
        {                             

            RefreshChildPoints();
        }

        public CompanionDataNode(RPGEntity controller, string Notes = "") : base(controller, Notes)
        {
            //Default constructor for data loading only
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
                    RefreshBaseCost();
                }
            }
        }
        private void RefreshChildPoints()
        {
            int tempChildPoints = 0;

            foreach (var temp in Children)
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
            }
            ChildPoints = tempChildPoints;
        }

        public override void ChildPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            base.ChildPropertyChanged(sender, e);
            if (sender is BaseNode )
            {
                if (e.PropertyName == nameof(BaseNode.Points))
                {
                    RefreshVariablesOrRestrictions();
                    RefreshChildPoints();
                }
            }
        }


        //Methods:
        protected override void RefreshPoints()
        {
            //Points should equal BaseCost +- any restrictions or variables
            int tempPoints = BaseCost;
            tempPoints += VariablesOrRestrictions;
         
            Points = tempPoints;
        }

        protected override void RefreshBaseCost()
        {
            //Points calc for companions:
            if (ChildPoints > 120)
            {
                BaseCost = (2 + ((ChildPoints - 120) / 10)) * Level;
            }
            else
            {
                BaseCost = 2 * Level;
            }
        }

        private int _baseCost;
        public override int BaseCost
        {
            get
            {
                return _baseCost;
            }
            protected set
            {
                int originalCost = _baseCost;
                _baseCost = value;
                if (originalCost != _baseCost)
                {
                    //Cost has changed
                    OnPropertyChanged(nameof(BaseCost));
                    RefreshPoints();
                }
            }
        }

        
    }
}
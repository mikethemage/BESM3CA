﻿using BESM3CAData.Control;
using BESM3CAData.Listings;
using System.Collections.Generic;
using System.Linq;

namespace BESM3CAData.Model
{
    public class SpecialContainerWithVariantDataNode : LevelableWithVariantDataNode, IVariantDataNode, ISpecialContainerDataNode
    {
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

        public int GetSpecialPoints()
        {
            int specialpoints = 0;

            if (AssociatedListing is ISpecialContainerDataListing attributeListing)
            {

                if (PointsUpToDate == false)
                {
                    GetPoints();
                }

                specialpoints = Level * attributeListing.SpecialPointsPerLevel;

                specialpoints -= _specialPointsUsed;

            }

            return specialpoints;
        }

        public override int GetPoints()
        {
            if (PointsUpToDate == false || FirstChild == null)
            {
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

                //Update special points used counter while recalculating points:
                _specialPointsUsed = ChildPoints;

                PointsUpToDate = true;
            }

            return _points;
        }

        /*public VariantListing Variant
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
        }*/


        //Constructors:
        public SpecialContainerWithVariantDataNode(DataController controller, string Notes = "") : base(controller, Notes)
        {

        }

        public SpecialContainerWithVariantDataNode(SpecialContainerWithVariantDataListing attribute, string notes, DataController controller, int level = 1, int pointAdj = 0) : base(attribute, notes, controller, level, pointAdj)
        {

        }


        //Methods:
        /*public List<VariantListing> GetVariants()
        {
            if (_dataListing is IVariantDataListing variantDataListing)
            {
                //LINQ Version:
                return variantDataListing.Variants.OrderByDescending(v => v.DefaultVariant).ThenBy(v => v.Name).ToList();
            }
            else
            {
                return null;
            }
        }*/
    }
}

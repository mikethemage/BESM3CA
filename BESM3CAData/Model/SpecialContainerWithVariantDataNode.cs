using BESM3CAData.Control;
using BESM3CAData.Listings;
using System.Collections.Generic;
using System.Linq;

namespace BESM3CAData.Model
{
    public class SpecialContainerWithVariantDataNode : LevelableDataNode, IVariantDataNode
    {
        //Fields:
        private VariantListing _variantListing;

        //Properties:
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


        //Constructors:
        public SpecialContainerWithVariantDataNode(DataController controller, string Notes = "") : base(controller, Notes)
        {

        }

        public SpecialContainerWithVariantDataNode(SpecialContainerWithVariantDataListing attribute, string notes, DataController controller, int level = 1, int pointAdj = 0) : base(attribute, notes, controller, level, pointAdj)
        {

        }


        //Methods:
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
    }
}

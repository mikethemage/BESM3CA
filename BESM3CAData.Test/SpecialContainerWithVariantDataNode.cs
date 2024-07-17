using BESM3CAData.Control;
using BESM3CAData.Listings;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ComponentModel;

namespace BESM3CAData.Model
{
    public class SpecialContainerWithVariantDataNode : LevelableDataNode, IVariantDataNode
    {

        private DataListing _associatedListing;
        public override DataListing AssociatedListing
        {
            get
            {
                return _associatedListing;
            }
            protected set
            {
                if (value != _associatedListing)
                {
                    _associatedListing = value;

                    foreach (VariantListing oldVL in VariantList)
                    {
                        oldVL.PropertyChanged -= VariantPropertyChanged;
                    }

                    VariantList.Clear();
                    if (_associatedListing is LevelableWithVariantDataListing variantDataListing && variantDataListing.Variants != null)
                    {
                        foreach (VariantListing newVL in variantDataListing.Variants)
                        {
                            VariantList.Add(newVL);
                            newVL.PropertyChanged += VariantPropertyChanged;
                        }
                    }
                }



            }
        }

        private void VariantPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is VariantListing variantListing)
            {
                if (e.PropertyName == nameof(VariantListing.IsSelected) && variantListing.IsSelected == true)
                {
                    Variant = variantListing;
                }
            }
        }

        //Fields:
        private VariantListing _variantListing;

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
                    Name = AssociatedListing.Name;

                    if (AssociatedListing is LevelableDataListing levelableDataListing)
                    {
                        PointsPerLevel = levelableDataListing.CostperLevel;
                    }
                    else
                    {
                        PointsPerLevel = 0;
                    }

                }
                

            }
        }

        
        public ObservableCollection<VariantListing> VariantList { get ; set ; } = new ObservableCollection<VariantListing>();


        //Constructors:
        public SpecialContainerWithVariantDataNode(RPGEntity controller, string Notes = "") : base(controller, Notes)
        {

        }

        public SpecialContainerWithVariantDataNode(SpecialContainerWithVariantDataListing attribute, string notes, RPGEntity controller, int level = 1, int freeLevels = 0, int requiredLevels = 0) : base(attribute, notes, controller, level, freeLevels, requiredLevels)
        {

        }


        //Methods:
        public List<VariantListing> GetVariants()
        {
            if (AssociatedListing is LevelableWithVariantDataListing variantDataListing)
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

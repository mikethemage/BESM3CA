using BESM3CAData.Listings;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BESM3CAData.Model
{
    public interface IVariantDataNode
    {
        //Properties:
        VariantListing Variant { get; set; }

        //Methods:
        List<VariantListing> GetVariants();

        ObservableCollection<VariantListing> VariantList { get; set; }
    }
}
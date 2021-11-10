using BESM3CAData.Listings;
using System.Collections.Generic;

namespace BESM3CAData.Model
{
    public interface IVariantDataNode
    {
        
        VariantListing Variant { get; set; }

        List<VariantListing> GetVariants();
    }
}
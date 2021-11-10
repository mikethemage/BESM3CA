using System.Collections.Generic;

namespace BESM3CAData.Listings
{
    public interface IVariantDataListing
    {
        bool RequiresVariant { get; }
        List<VariantListing> Variants { get; set; }
    }
}
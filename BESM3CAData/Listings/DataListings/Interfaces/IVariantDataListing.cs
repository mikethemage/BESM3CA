using System.Collections.Generic;

namespace BESM3CAData.Listings
{
    public interface IVariantDataListing
    {      
        List<VariantListing> Variants { get; set; }        
    }
}
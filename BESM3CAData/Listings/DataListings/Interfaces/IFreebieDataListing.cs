using System.Collections.Generic;

namespace BESM3CAData.Listings
{
    public interface IFreebieDataListing
    {
        List<FreebieListing> Freebies { get; set; }
    }
}
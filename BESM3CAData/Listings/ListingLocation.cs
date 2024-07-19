using System.IO;
using System.Text.Json.Serialization;

namespace BESM3CAData.Listings
{
    public class ListingLocation
    {
        public string? ListingName { get; set; }
        public bool BuiltIn { get; set; }
        public string[]? ListingPathArray { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public string ListingPath
        {
            get
            {
                if (ListingPathArray != null)
                {
                    return Path.Combine(ListingPathArray);
                }
                else
                {
                    return string.Empty;
                }
            }
        }


        public MasterListing? LoadListing()
        {
            return MasterListing.JSONLoader(this);
        }
    }
}

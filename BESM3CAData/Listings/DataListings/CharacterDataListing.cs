using BESM3CAData.Listings.Serialization;
using BESM3CAData.Model;
using System.Collections.Generic;
using System.Linq;
using BESM3CAData.Control;

namespace BESM3CAData.Listings
{
    public class CharacterDataListing : DataListing
    {
        public override BaseNode CreateNode(string notes, DataController controller, int level=0, int pointsAdj=0)
        {
            return new CharacterNode(this, notes, controller);
        }

        public CharacterDataListing(DataListingSerialized data) : base(data)
        {
            
        }
    }
}

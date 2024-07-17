using BESM3CAData.Model;
using System.Collections.Generic;
using System.Linq;
using BESM3CAData.Control;
using Triarch.Dtos.Definitions;

namespace BESM3CAData.Listings
{
    public class CharacterDataListing : DataListing
    {
        public override BaseNode CreateNode(string notes, RPGEntity controller, int level=0, int pointsAdj=0, bool isFreebie=false)
        {
            return new CharacterNode(this, notes, controller, isFreebie);
        }

        public CharacterDataListing(RPGElementDefinitionDto data) : base(data)
        {
            
        }
    }
}

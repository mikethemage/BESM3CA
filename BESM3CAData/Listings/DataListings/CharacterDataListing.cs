using BESM3CAData.Model;
using System.Collections.Generic;
using System.Linq;
using BESM3CAData.Control;
using Triarch.Dtos.Definitions;

namespace BESM3CAData.Listings
{
    public class CharacterDataListing : DataListing
    {
        public override BaseNode CreateNode(string notes, RPGEntity controller, bool isLoading, int level=0, int freeLevels=0,int requiredLevels=0, bool isFreebie=false)
        {
            return new CharacterNode(this, isLoading, notes, controller, isFreebie);
        }

        public CharacterDataListing(RPGElementDefinitionDto data) : base(data)
        {
            
        }
    }
}

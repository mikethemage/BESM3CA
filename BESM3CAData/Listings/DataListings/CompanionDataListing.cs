using BESM3CAData.Control;
using BESM3CAData.Model;
using System.Collections.Generic;
using System.Linq;
using Triarch.Dtos.Definitions;

namespace BESM3CAData.Listings
{
    public class CompanionDataListing : LevelableDataListing
    {
        public CompanionDataListing(RPGElementDefinitionDto data) : base(data)
        {                  
        }

        public override RPGElementDefinitionDto Serialize()
        {
            RPGElementDefinitionDto result = base.Serialize();              
            
            return result;
        }

        public override DataNode CreateNode(string notes, RPGEntity controller, bool isLoading, int level = 1, int freeLevels = 0, int requiredLevels = 0, bool isFreebie = false)
        {
            return new CompanionDataNode(this, isLoading, notes, controller, level, freeLevels, requiredLevels, isFreebie);
        }
    }
}
using BESM3CAData.Listings.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BESM3CAData.Model;
using BESM3CAData.Control;

namespace BESM3CAData.Listings
{
    public class SpecialContainerWithVariantDataListing : LevelableWithVariantDataListing, IVariantDataListing
    {        
        public int SpecialPointsPerLevel { get; set; }
        public bool SpecialContainer { get; private set; }

        public override DataNode CreateNode(string notes, DataController controller, int level = 1, int pointAdj = 0)
        {
            return new SpecialContainerWithVariantDataNode(this, notes, controller, level, pointAdj);
        }

        public SpecialContainerWithVariantDataListing(DataListingSerialized data) : base(data)
        {
            SpecialPointsPerLevel = data.SpecialPointsPerLevel;
            SpecialContainer = data.SpecialContainer;            
        }

        public override DataListingSerialized Serialize()
        {
            DataListingSerialized result=base.Serialize();
            
            result.SpecialPointsPerLevel = this.SpecialPointsPerLevel;
            result.SpecialContainer = this.SpecialContainer;
            return result;
        }
    }
}

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
    public class SpecialContainerWithVariantDataListing : LevelableWithVariantDataListing, IVariantDataListing, ISpecialContainerDataListing
    {        
        //Properties:
        public int SpecialPointsPerLevel { get; set; }


        //Constructors:
        public SpecialContainerWithVariantDataListing(DataListingSerialized data) : base(data)
        {
            SpecialPointsPerLevel = data.SpecialPointsPerLevel;
        }


        //Methods:
        public override DataNode CreateNode(string notes, DataController controller, int level = 1, int pointAdj = 0)
        {
            return new SpecialContainerWithVariantDataNode(this, notes, controller, level, pointAdj);
        }

        public override DataListingSerialized Serialize()
        {
            DataListingSerialized result=base.Serialize();
            
            result.SpecialPointsPerLevel = this.SpecialPointsPerLevel;
            result.SpecialContainer = true;
            return result;
        }
    }
}

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
    public class SpecialContainerDataListing : LevelableDataListing, ISpecialContainerDataListing
    {        
        //Properties:
        public int SpecialPointsPerLevel { get; set; }


        //Constructors:
        public SpecialContainerDataListing(DataListingSerialized data) : base(data)
        {
            SpecialPointsPerLevel = data.SpecialPointsPerLevel ?? 1;
        }


        //Methods:
        public override DataNode CreateNode(string notes, RPGEntity controller, int level = 1, int pointAdj = 0)
        {
            return new SpecialContainerDataNode(this, notes, controller, level, pointAdj);
        }

        public override DataListingSerialized Serialize()
        {
            DataListingSerialized result=base.Serialize();
            
            result.SpecialPointsPerLevel = SpecialPointsPerLevel;
            result.SpecialContainer = true;
            return result;
        }
    }
}

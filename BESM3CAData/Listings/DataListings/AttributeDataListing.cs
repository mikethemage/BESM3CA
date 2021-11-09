using BESM3CAData.Listings.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BESM3CAData.Listings
{
    public class AttributeDataListing : DataListing
    {
        private bool Container { get; set; }
        private int SpecialPointsPerLevel { get; set; }
        public bool SpecialContainer { get; private set; }

        public AttributeDataListing(DataListingSerialized data) : base(data)
        {
            SpecialPointsPerLevel = data.SpecialPointsPerLevel;
            SpecialContainer = data.SpecialContainer;
            Container = data.Container;
        }

        public override DataListingSerialized Serialize()
        {
            DataListingSerialized result=base.Serialize();
            result.Container = this.Container;
            result.SpecialPointsPerLevel = this.SpecialPointsPerLevel;
            result.SpecialContainer = this.SpecialContainer;
            return result;
        }
    }
}

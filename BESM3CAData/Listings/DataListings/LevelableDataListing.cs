using BESM3CAData.Listings.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BESM3CAData.Listings
{
    public class LevelableDataListing : DataListing
    {
        public override bool HasLevel
        {
            get
            { 
                return true; 
            }
        }

        //Only things with Levels:
        public int MaxLevel { get; private set; }
        public int CostperLevel { get; private set; }
        public bool EnforceMaxLevel { get; private set; }
        public List<string> CustomProgression { get; private set; }

        //  To check if still needed: 
        private string CostperLevelDesc { get; set; }
        private string Progression { get; set; }


        public override DataListingSerialized Serialize()
        {
            DataListingSerialized result = base.Serialize();

            result.CostperLevelDesc = CostperLevelDesc;
            result.CostperLevel = CostperLevel;
            result.Progression = Progression;
            result.MaxLevel = MaxLevel;
            result.EnforceMaxLevel = this.EnforceMaxLevel;
            result.CustomProgression = this.CustomProgression;

            return result;
        }

        public LevelableDataListing()
        {
            CostperLevelDesc = "";
            Progression = "";
        }

        public LevelableDataListing(DataListingSerialized data) : base(data)
        {
            CostperLevelDesc = data.CostperLevelDesc;
            CostperLevel = data.CostperLevel;
            Progression = data.Progression;
            MaxLevel = data.MaxLevel;
            EnforceMaxLevel = data.EnforceMaxLevel;
            CustomProgression = data.CustomProgression;
        }
    }
}

using BESM3CAData.Listings.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using BESM3CAData.Model;
using BESM3CAData.Control;


namespace BESM3CAData.Listings
{
    public class LevelableDataListing : DataListing
    {       
        //Properties:
        //Only things with Levels:
        public int MaxLevel { get; private set; }
        public int CostperLevel { get; private set; }
        public bool EnforceMaxLevel { get; private set; }
        public List<string> CustomProgression { get; private set; }

        //  To check if still needed: 
        private string CostperLevelDesc { get; set; }
        private string Progression { get; set; }


        //Constructors:
        public LevelableDataListing()
        {
            CostperLevelDesc = "";
            Progression = "";
        }


        //Methods:
        public override DataNode CreateNode(string notes, DataController controller, int level = 1, int pointAdj = 0)
        {
            return new LevelableDataNode(this, notes, controller, level, pointAdj);
        }

        public override DataListingSerialized Serialize()
        {
            DataListingSerialized result = base.Serialize();

            result.CostperLevelDesc = CostperLevelDesc;
            result.CostperLevel = CostperLevel;
            result.Progression = Progression;
            result.MaxLevel = MaxLevel;
            result.EnforceMaxLevel = this.EnforceMaxLevel;
            result.CustomProgression = this.CustomProgression;
            result.HasLevel = true;
            return result;
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

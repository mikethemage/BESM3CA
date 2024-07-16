using System;
using System.Collections.Generic;
using System.Linq;
using BESM3CAData.Model;
using BESM3CAData.Control;
using Triarch.Dtos.Definitions;


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
        public string ProgressionName { get; set; }

        public ProgressionListing Progression { get; set; }


        //Constructors:
        public LevelableDataListing()
        {
            CostperLevelDesc = "";
            ProgressionName = "";
        }


        //Methods:
        public override DataNode CreateNode(string notes, RPGEntity controller, int level = 1, int pointAdj = 0)
        {
            return new LevelableDataNode(this, notes, controller, level, pointAdj);
        }

        public override RPGElementDefinitionDto Serialize()
        {
            RPGElementDefinitionDto result = base.Serialize();
            result.LevelableData = new LevelableDefinitionDto
            {
                CostPerLevel = CostperLevel,
                CostPerLevelDescription = CostperLevelDesc,
                ProgressionName = ProgressionName,
                MaxLevel = MaxLevel,
                EnforceMaxLevel = EnforceMaxLevel
            };           
            
            return result;
        }

        public LevelableDataListing(RPGElementDefinitionDto data) : base(data)
        {
            CostperLevelDesc = data.LevelableData.CostPerLevelDescription;
            CostperLevel = (int)data.LevelableData.CostPerLevel;
            ProgressionName = data.LevelableData.ProgressionName;
            MaxLevel = (int)data.LevelableData.MaxLevel;
            EnforceMaxLevel = (bool)data.LevelableData.EnforceMaxLevel;
            ProgressionName = data.LevelableData.ProgressionName;            
        }
    }
}

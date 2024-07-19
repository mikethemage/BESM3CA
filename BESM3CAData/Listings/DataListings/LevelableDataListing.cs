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
        public List<string>? CustomProgression { get; private set; }

        //  To check if still needed: 
        private string CostperLevelDesc { get; set; }
        public string ProgressionName { get; set; }

        public ProgressionListing? Progression { get; set; }

        public List<VariantListing>? Variants { get; set; }


        //Constructors:
        public LevelableDataListing()
        {
            CostperLevelDesc = "";
            ProgressionName = "";
        }


        //Methods:
        public override DataNode CreateNode(string notes, RPGEntity controller, bool isLoading, int level = 1, int freeLevels = 0, int requiredLevels = 0, bool isFreebie = false)
        {
            return new LevelableDataNode(this, isLoading, notes, controller, level, freeLevels, requiredLevels, isFreebie);                     
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

            if (Variants != null && Variants.Count > 0)
            {
                result.LevelableData.Variants = new List<VariantDefinitionDto>();
                foreach (VariantListing variant in Variants)
                {
                    result.LevelableData.Variants.Add(variant.Serialize());
                }
            }

            return result;
        }

        public LevelableDataListing(RPGElementDefinitionDto data) : base(data)
        {
            CostperLevelDesc = data.LevelableData?.CostPerLevelDescription ?? "";
            CostperLevel = data.LevelableData?.CostPerLevel ?? 0;            
            MaxLevel = data.LevelableData?.MaxLevel ?? 0;
            EnforceMaxLevel = data.LevelableData?.EnforceMaxLevel ?? false;
            ProgressionName = data.LevelableData?.ProgressionName ?? "";

            //Add variants and link back:
            if (data.LevelableData?.Variants != null)
            {
                Variants = new List<VariantListing>();
                foreach (VariantDefinitionDto variant in data.LevelableData.Variants)
                {
                    Variants.Add(new VariantListing { ID = variant.Id, Name = variant.VariantName, CostperLevel = variant.CostPerLevel, DefaultVariant = variant.IsDefault, Desc = variant.Description ?? "", Attribute = this });
                }
            }
        }
    }
}

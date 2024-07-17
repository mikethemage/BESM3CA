using System;
using System.Collections.Generic;
using System.Linq;
using BESM3CAData.Model;
using BESM3CAData.Control;
using Triarch.Dtos.Definitions;


namespace BESM3CAData.Listings
{
    public class LevelableWithFreebieDataListing : LevelableDataListing, IFreebieDataListing
    {               
        public List<FreebieListing> Freebies { get; set; } = new List<FreebieListing>();
               
        //Methods:
        public override DataNode CreateNode(string notes, RPGEntity controller, int level = 1, int freeLevels = 0, int requiredLevels = 0, bool isFreebie = false)
        {
            return new LevelableWithFreebieDataNode(this, notes, controller, level, freeLevels, requiredLevels, isFreebie);
        }

        public override RPGElementDefinitionDto Serialize()
        {
            RPGElementDefinitionDto result = base.Serialize();

            if (Freebies != null)
            {
                result.Freebies = Freebies.Select(x =>

                new FreebieDto
                {
                    FreebieElementDefinitionName = x.SubAttributeName,
                    FreeLevels = x.FreeLevels,
                    RequiredLevels = x.RequiredLevels
                }).ToList();
            }

            return result;
        }

        public LevelableWithFreebieDataListing(RPGElementDefinitionDto data) : base(data)
        {
            if (data.Freebies != null)
            {
                foreach (FreebieDto freebie in data.Freebies)
                {
                    Freebies.Add(new FreebieListing
                    {
                        SubAttributeName = freebie.FreebieElementDefinitionName,
                        RequiredLevels = freebie.RequiredLevels,
                        FreeLevels = freebie.FreeLevels
                    });
                }
            }
        }
    }
}

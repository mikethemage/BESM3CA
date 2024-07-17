using BESM3CAData.Control;
using BESM3CAData.Model;
using System.Collections.Generic;
using System.Linq;
using Triarch.Dtos.Definitions;

namespace BESM3CAData.Listings
{
    public class LevelableWithFreebieWithVariantDataListing : LevelableWithVariantDataListing, IFreebieDataListing
    {
        //Properties:


        public List<FreebieListing> Freebies { get; set; } = new List<FreebieListing>();

        //Constructors:
        public LevelableWithFreebieWithVariantDataListing(RPGElementDefinitionDto data) : base(data)
        {
            if(data.Freebies!=null)
            {
                foreach (FreebieDto freebie in data.Freebies)
                {
                    Freebies.Add(new FreebieListing
                    {
                        SubAttributeName = freebie.FreebieElementDefinitionName,
                        SubAttributeLevel = freebie.FreeLevels + freebie.RequiredLevels,
                        SubAttributePointsAdj = freebie.FreeLevels
                    });
                }
            }
            
        }

        //Methods:
        public override RPGElementDefinitionDto Serialize()
        {
            RPGElementDefinitionDto result = base.Serialize();

            if (Freebies != null)
            {
                result.Freebies = Freebies.Select(x =>

                new FreebieDto
                {
                    FreebieElementDefinitionName = x.SubAttributeName,
                    FreeLevels = x.SubAttributeLevel - x.SubAttributePointsAdj,
                    RequiredLevels = x.SubAttributePointsAdj
                }).ToList();
            }           


            return result;
        }

        public override DataNode CreateNode(string notes, RPGEntity controller, int level = 1, int pointAdj = 0, bool isFreebie = false)
        {
            return new LevelableWithFreebieWithVariantDataNode(this, notes, controller, level, pointAdj, isFreebie);
        }
    }
}
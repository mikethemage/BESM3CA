using BESM3CAData.Control;
using BESM3CAData.Model;
using System.Collections.Generic;
using Triarch.Dtos.Definitions;

namespace BESM3CAData.Listings
{
    public class CompanionDataListing : LevelableDataListing, IFreebieDataListing
    {
        public DataListing SubAttribute { get; set; }
        public int SubAttributeLevel { get; set; }
        public int SubAttributePointsAdj { get; set; }

        public CompanionDataListing(RPGElementDefinitionDto data) : base(data)
        {

            SubAttributeLevel = data.Freebies[0].FreeLevels + data.Freebies[0].RequiredLevels;
            SubAttributePointsAdj = data.Freebies[0].FreeLevels;
        }

        public override RPGElementDefinitionDto Serialize()
        {
            RPGElementDefinitionDto result = base.Serialize();

            result.Freebies = new List<FreebieDto> {
                new FreebieDto
                {
                    FreebieElementDefinitionName = SubAttribute.Name,
                    FreeLevels = SubAttributePointsAdj,
                    RequiredLevels = SubAttributeLevel - SubAttributePointsAdj
                }
            };

            return result;
        }

        public override DataNode CreateNode(string notes, RPGEntity controller, int level = 1, int pointAdj = 0)
        {
            return new CompanionDataNode(this, notes, controller, level, pointAdj);
        }
    }
}
using BESM3CAData.Control;
using BESM3CAData.Model;
using System.Collections.Generic;
using Triarch.Dtos.Definitions;

namespace BESM3CAData.Listings
{
    public class LevelableWithFreebieWithVariantDataListing : LevelableWithVariantDataListing, IFreebieDataListing
    {
        //Properties:
        public DataListing SubAttribute { get; set; }
        public int SubAttributeLevel { get; set; }
        public int SubAttributePointsAdj { get; set; }


        //Constructors:
        public LevelableWithFreebieWithVariantDataListing(RPGElementDefinitionDto data) : base(data)
        {

            SubAttributeLevel = data.Freebies[0].FreeLevels + data.Freebies[0].RequiredLevels;
            SubAttributePointsAdj = data.Freebies[0].FreeLevels;
        }


        //Methods:
        public override RPGElementDefinitionDto Serialize()
        {
            RPGElementDefinitionDto result = base.Serialize();

            result.Freebies = new List<FreebieDto> {
                new FreebieDto {
                FreebieElementDefinitionName = SubAttribute.Name,
                FreeLevels =  SubAttributeLevel - SubAttributePointsAdj,
                RequiredLevels = SubAttributePointsAdj
            } };

            return result;
        }

        public override DataNode CreateNode(string notes, RPGEntity controller, int level = 1, int pointAdj = 0)
        {
            return new LevelableWithFreebieWithVariantDataNode(this, notes, controller, level, pointAdj);
        }
    }
}
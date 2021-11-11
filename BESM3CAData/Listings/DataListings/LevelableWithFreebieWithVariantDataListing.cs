using BESM3CAData.Control;
using BESM3CAData.Listings.Serialization;
using BESM3CAData.Model;

namespace BESM3CAData.Listings
{
    public class LevelableWithFreebieWithVariantDataListing : LevelableWithVariantDataListing, IFreebieDataListing
    {
        //Properties:
        public DataListing SubAttribute { get; set; }
        public int SubAttributeLevel { get; set; }
        public int SubAttributePointsAdj { get; set; }


        //Constructors:
        public LevelableWithFreebieWithVariantDataListing(DataListingSerialized data) : base(data)
        {
            
            SubAttributeLevel = data.SubAttributeLevel;
            SubAttributePointsAdj = data.SubAttributePointsAdj;
        }


        //Methods:
        public override DataListingSerialized Serialize()
        {
            DataListingSerialized result = base.Serialize();

            result.HasFreebie = true;
            result.SubAttributeID = SubAttribute.ID;
            result.SubAttributeLevel = SubAttributeLevel;
            result.SubAttributePointsAdj = SubAttributePointsAdj;

            return result;
        }

        public override DataNode CreateNode(string notes, DataController controller, int level = 1, int pointAdj = 0)
        {
            return new LevelableWithFreebieWithVariantDataNode(this, notes, controller, level, pointAdj);
        }
    }
}
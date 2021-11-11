using BESM3CAData.Control;
using BESM3CAData.Listings.Serialization;
using BESM3CAData.Model;

namespace BESM3CAData.Listings
{
    public class CompanionDataListing : LevelableDataListing, IFreebieDataListing
    {
        public DataListing SubAttribute { get; set; }
        public int SubAttributeLevel { get; set; }
        public int SubAttributePointsAdj { get; set; }

        public CompanionDataListing(DataListingSerialized data) : base(data)
        {
            
            SubAttributeLevel = data.SubAttributeLevel;
            SubAttributePointsAdj = data.SubAttributePointsAdj;
        }

        public override DataListingSerialized Serialize()
        {
            DataListingSerialized result = base.Serialize();
            result.Companion = true;
            result.HasFreebie = true;
            result.SubAttributeID = SubAttribute.ID;
            result.SubAttributeLevel = SubAttributeLevel;
            result.SubAttributePointsAdj = SubAttributePointsAdj;

            return result;
        }

        public override DataNode CreateNode(string notes, DataController controller, int level = 1, int pointAdj = 0)
        {
            return new CompanionDataNode(this, notes, controller, level, pointAdj);
        }
    }
}
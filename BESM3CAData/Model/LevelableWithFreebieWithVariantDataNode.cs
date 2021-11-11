using BESM3CAData.Control;
using BESM3CAData.Listings;

namespace BESM3CAData.Model
{
    public class LevelableWithFreebieWithVariantDataNode : LevelableWithVariantDataNode
    {
        public LevelableWithFreebieWithVariantDataNode(LevelableWithFreebieWithVariantDataListing attribute, string notes, DataController controller, int level = 1, int pointAdj = 0) : base(attribute, notes, controller, level, pointAdj)
        {        
            //Auto create freebie when creating new instance of this attribute:
            AddChild(attribute.SubAttribute.CreateNode("", AssociatedController, attribute.SubAttributeLevel, attribute.SubAttributePointsAdj));
        }

        public LevelableWithFreebieWithVariantDataNode(DataController controller, string Notes = "") : base(controller, Notes)
        {
            //Default constructor for data loading only
            //Don't need to create Freebie as it should already be present in data load
        }

    }
}
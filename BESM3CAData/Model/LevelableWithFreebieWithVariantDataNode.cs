using BESM3CAData.Control;
using BESM3CAData.Listings;
using System.Linq;

namespace BESM3CAData.Model
{
    public class LevelableWithFreebieWithVariantDataNode : LevelableWithVariantDataNode
    {
        public LevelableWithFreebieWithVariantDataNode(LevelableWithFreebieWithVariantDataListing attribute, string notes, RPGEntity controller, int level = 1, int pointAdj = 0, bool isFreebie = false) : base(attribute, notes, controller, level, pointAdj, isFreebie)
        {
            foreach (FreebieListing freebie in attribute.Freebies)
            {
                DataListing subAttribute = controller.SelectedListingData.AttributeList.Where(x => x.Name == freebie.SubAttributeName).FirstOrDefault();
                if(subAttribute!= null)
                {
                    //Auto create freebie when creating new instance of this attribute:
                    AddChild(subAttribute.CreateNode("", AssociatedController, freebie.SubAttributeLevel, freebie.SubAttributePointsAdj, true));
                }                
            }            
        }

        public LevelableWithFreebieWithVariantDataNode(RPGEntity controller, string Notes = "") : base(controller, Notes)
        {
            //Default constructor for data loading only
            //Don't need to create Freebie as it should already be present in data load
        }

    }
}
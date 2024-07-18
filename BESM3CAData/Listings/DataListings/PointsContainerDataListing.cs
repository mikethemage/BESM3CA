using BESM3CAData.Model;
using System.Collections.Generic;
using System.Linq;
using BESM3CAData.Control;
using Triarch.Dtos.Definitions;

namespace BESM3CAData.Listings
{
    public class PointsContainerDataListing : DataListing
    {
        //Properties:
        public int PointScale { get; set; }


        //Constructors:
        public PointsContainerDataListing(RPGElementDefinitionDto data) : base(data)
        {
            PointScale = data.PointsContainerScale ?? 1;
        }


        //Methods:
        public override RPGElementDefinitionDto Serialize()
        {
            RPGElementDefinitionDto result = base.Serialize();
            
            result.PointsContainerScale = PointScale;

            return result;
        }

        public override PointsContainerDataNode CreateNode(string notes, RPGEntity controller, bool isLoading, int level = 1, int freeLevels = 0, int requiredLevels=0, bool isFreebie = false)
        {
            return new PointsContainerDataNode(this, isLoading, notes, controller, isFreebie);
        }
    }
}

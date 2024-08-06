using Triarch.Dtos.Definitions;

namespace BESM3CAData.Listings
{
    public class TypeListing
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public int TypeOrder { get; set; }


        public RPGElementTypeDto Serialize()
        {
            return new RPGElementTypeDto { TypeName = Name!, TypeOrder = TypeOrder };
        }        
    }
}

using BESM3CAData.Listings.Serialization;

namespace BESM3CAData.Listings
{
    public class TypeListing
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int TypeOrder { get; set; }


        public TypeListingSerialized Serialize()
        {
            return new TypeListingSerialized { ID = ID, Name = Name, TypeOrder = TypeOrder };
        }

        public static TypeListing Deserialize(TypeListingSerialized typeListing)
        {
            TypeListing result = new TypeListing { ID = typeListing.ID, Name = typeListing.Name, TypeOrder = typeListing.TypeOrder };
            return result;
        }
    }
}

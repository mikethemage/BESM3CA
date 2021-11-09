using BESM3CAData.Listings.Serialization;

namespace BESM3CAData.Listings
{
    public class VariantListing
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int CostperLevel { get; set; }
        public string Desc { get; set; }
        public bool DefaultVariant { get; set; }        
        public AttributeListing Attribute { get; set; }
               
        public string FullName
        {
            get
            {
                return $"{Attribute.Name} [{Name}]";
            }
        }


        public VariantListingSerialized Serialize()
        {
            return new VariantListingSerialized { ID=this.ID, Name=this.Name, CostperLevel=this.CostperLevel, Desc=this.Desc,DefaultVariant=this.DefaultVariant };
        }

    }
}

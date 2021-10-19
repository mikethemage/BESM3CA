using System.Text.Json.Serialization;

namespace BESM3CAData.Templates
{
    public class VariantListing
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public int CostperLevel { get; set; }               

        public string Desc { get; set; }

        public bool DefaultVariant { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public AttributeListing Attribute { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public string FullName 
        { 
            get 
            {
                return Attribute.Name + " [" + Name + "]";
            }
        }

    }
}

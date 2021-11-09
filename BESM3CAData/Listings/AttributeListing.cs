using BESM3CAData.Listings.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace BESM3CAData.Listings
{
    public class AttributeListing
    {
        //Properties:
        public int ID { get; private set; }
        public string Name { get; private set; }
        public int MaxLevel { get; private set; }
        public string Type { get; private set; }
        public int CostperLevel { get; private set; }
        public bool RequiresVariant { get; private set; }
        public bool SpecialContainer { get; private set; }
        public bool EnforceMaxLevel { get; private set; }
        public string Description { get; private set; }
        public List<string> CustomProgression { get; private set; }
        public List<int> GenrePoints { get; private set; }
        public List<AttributeListing> Children { get; private set; }
        public List<VariantListing> Variants { get; set; }

        //To check if still needed:
        private string CostperLevelDesc { get; set; }
        private string Progression { get; set; }
        private string Stat { get; set; }
        private string Page { get; set; }
        private bool Human { get; set; }
        private bool Container { get; set; }
        private int SpecialPointsPerLevel { get; set; }


        //Constructor:
        public AttributeListing()
        {
            CostperLevelDesc = "";
            Progression = "";
            Children = new List<AttributeListing>();
        }


        //Methods:
        public void AddChild(AttributeListing Child)
        {
            if (Child != null)
            {
                Children.Add(Child);
            }
        }


        //Serialization:
        public AttributeListingSerialized Serialize()
        {
            AttributeListingSerialized result = new AttributeListingSerialized
            {
                ID = this.ID,
                Name = this.Name,
                CostperLevelDesc = this.CostperLevelDesc,
                CostperLevel = this.CostperLevel,
                Progression = this.Progression,
                MaxLevel = this.MaxLevel,
                Stat = this.Stat,
                Page = this.Page,
                Human = this.Human,
                Type = this.Type,
                Container = this.Container,
                RequiresVariant = this.RequiresVariant,
                SpecialPointsPerLevel = this.SpecialPointsPerLevel,
                SpecialContainer = this.SpecialContainer,
                EnforceMaxLevel = this.EnforceMaxLevel,
                Description = this.Description,
                CustomProgression = this.CustomProgression,
                GenrePoints = this.GenrePoints
            };

            //Convert childrenlist to string:
            IEnumerable<int> ChildIDs = from child in Children
                                        select child.ID;

            result.ChildrenList = string.Join(",", ChildIDs);

            if (Variants != null)
            {
                result.Variants = new List<VariantListingSerialized>();
                foreach (VariantListing variant in Variants)
                {
                    result.Variants.Add(variant.Serialize());
                }
            }
            return result;
        }

        public static AttributeListing Deserialize(AttributeListingSerialized attribute)
        {
            AttributeListing result = new AttributeListing
            {
                ID = attribute.ID,
                Name = attribute.Name,
                CostperLevelDesc = attribute.CostperLevelDesc,
                CostperLevel = attribute.CostperLevel,
                Progression = attribute.Progression,
                MaxLevel = attribute.MaxLevel,
                Stat = attribute.Stat,
                Page = attribute.Page,
                Human = attribute.Human,
                Type = attribute.Type,
                Container = attribute.Container,
                RequiresVariant = attribute.RequiresVariant,
                SpecialPointsPerLevel = attribute.SpecialPointsPerLevel,
                SpecialContainer = attribute.SpecialContainer,
                EnforceMaxLevel = attribute.EnforceMaxLevel,
                Description = attribute.Description,
                CustomProgression = attribute.CustomProgression,
                GenrePoints = attribute.GenrePoints
            };

            //Add variants  and link back:
            if (attribute.Variants != null)
            {
                result.Variants = new List<VariantListing>();
                foreach (VariantListingSerialized variant in attribute.Variants)
                {
                    result.Variants.Add(new VariantListing { ID = variant.ID, Name = variant.Name, CostperLevel = variant.CostperLevel, DefaultVariant = variant.DefaultVariant, Desc = variant.Desc, Attribute = result });
                }
            }

            return result;
        }
    }
}

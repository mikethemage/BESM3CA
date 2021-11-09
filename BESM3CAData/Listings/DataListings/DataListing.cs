using BESM3CAData.Listings.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace BESM3CAData.Listings
{
    public class DataListing
    {
        //Properties:
        public int ID { get; private set; }
        public string Name { get; private set; }
        public int MaxLevel { get; private set; }
        public string Type { get; private set; }
        public int CostperLevel { get; private set; }
        public bool RequiresVariant { get; private set; }
        
        public bool EnforceMaxLevel { get; private set; }
        public string Description { get; private set; }
        public List<string> CustomProgression { get; private set; }
        
        public List<DataListing> Children { get; private set; }
        public List<VariantListing> Variants { get; set; }

        //To check if still needed:
        private string CostperLevelDesc { get; set; }
        private string Progression { get; set; }
        private string Stat { get; set; }
        private string Page { get; set; }
        private bool Human { get; set; }
        


        //Constructor:
        public DataListing()
        {
            CostperLevelDesc = "";
            Progression = "";
            Children = new List<DataListing>();
        }


        //Methods:
        public void AddChild(DataListing Child)
        {
            if (Child != null)
            {
                Children.Add(Child);
            }
        }


        //Serialization:
        public virtual DataListingSerialized Serialize()
        {
            DataListingSerialized result = new DataListingSerialized
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
                RequiresVariant = this.RequiresVariant,                
                EnforceMaxLevel = this.EnforceMaxLevel,
                Description = this.Description,
                CustomProgression = this.CustomProgression
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


        public DataListing(DataListingSerialized data)
        {
            ID = data.ID;
            Name = data.Name;
            CostperLevelDesc = data.CostperLevelDesc;
            CostperLevel = data.CostperLevel;
            Progression = data.Progression;
            MaxLevel = data.MaxLevel;
            Stat = data.Stat;
            Page = data.Page;
            Human = data.Human;
            Type = data.Type;
            
            RequiresVariant = data.RequiresVariant;

            


            EnforceMaxLevel = data.EnforceMaxLevel;
            Description = data.Description;
            CustomProgression = data.CustomProgression;
            
            Children = new List<DataListing>();

            //Add variants and link back:
            if (data.Variants != null)
            {
                Variants = new List<VariantListing>();
                foreach (VariantListingSerialized variant in data.Variants)
                {
                    Variants.Add(new VariantListing { ID = variant.ID, Name = variant.Name, CostperLevel = variant.CostperLevel, DefaultVariant = variant.DefaultVariant, Desc = variant.Desc, Attribute = this });
                }
            }            
        }
    }
}

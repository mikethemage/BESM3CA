//using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace BESM3CAData.Listings
{
    public class AttributeListing
    {
        //Properties:
        public int ID { get; set; }
        public string Name { get; set; }
        public string CostperLevelDesc { get; set; }
        public string Progression { get; set; }
        public int MaxLevel { get; set; }
        public string Stat { get; set; }
        public string Page { get; set; }
        public bool Human { get; set; }
        public string Type { get; set; }
        public bool Container { get; set; }
        public int CostperLevel { get; set; }
        public bool RequiresVariant { get; set; }
        public int SpecialPointsPerLevel { get; set; }
        public bool SpecialContainer { get; set; }
        public bool EnforceMaxLevel { get; set; }
        public string Description { get; set; }

        public List<string> CustomProgression { get; set; }

        public List<int> GenrePoints { get; set; }

        public string ChildrenList
        {
            //Used for serialisation
            get
            {
                IEnumerable<int> ChildIDs = from child in Children
                                            select child.ID;

                string temp = string.Join(",", ChildIDs);
                return temp;
            }
        }

        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public List<AttributeListing> Children { get; private set; } //Ignore for serialisation

        public List<VariantListing> Variants { get; set; }

        //Constructor:
        public AttributeListing()
        {
            //Used for deserialisation
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
    }
}

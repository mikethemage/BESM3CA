using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace BESM3CAData.Templates
{
    public class AttributeListing
    {
        //Fields:
        private readonly List<AttributeListing> _children;

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
                string temp = "";

                var ChildIDs = from child in _children
                               select child.ID;

                temp = string.Join(",", ChildIDs);

                return temp;
            }
        }

        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public List<AttributeListing> Children
        {
            //Ignore for serialisation
            get
            {
                return _children;
            }
        }
        
        public List<VariantListing> Variants
        {
            get;
            set;
        }


        //Constructor:
        public AttributeListing()
        {
            //Used for deserialisation
            CostperLevelDesc = "";
            Progression = "";
            _children = new List<AttributeListing>();

        }


        //Member functions:
        public void AddChild(AttributeListing Child)
        {
            if (Child != null)
            {
                _children.Add(Child);
            }
        }
    }
}

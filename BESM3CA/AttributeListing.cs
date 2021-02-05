using System.Collections.Generic;
using Newtonsoft.Json;
//using System.Text.Json;
//using System.Text.Json.Serialization;
using System.Linq;

namespace BESM3CA
{
    class AttributeListing
    {
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

        public string ChildrenList
        {
            get
            {
                string temp = "";

                var ChildIDs = from child in _children.Values
                               select child.ID;

                temp = string.Join(",", ChildIDs);

                return temp;
            }
        }
        private SortedList<string, AttributeListing> _children;

        [JsonIgnore]//(Condition = JsonIgnoreCondition.Always)]
        public SortedList<string, AttributeListing> Children
        {
            get
            {
                return _children;
            }
        }

        public AttributeListing()
        {
            CostperLevelDesc = "";
            Progression = "";

            _children = new SortedList<string, AttributeListing>();

        }

        public void AddChild(AttributeListing Child)
        {
            if (Child != null)
            {
                _children.Add(Child.Name, Child);
            }
        }
    }
}

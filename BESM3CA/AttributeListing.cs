using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

namespace BESM3CA
{
    class AttributeListing
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string CostperLevelDesc;
        public string Progression;
        public int MaxLevel;
        public string Stat;
        public string Page;
        public bool Human;
        public string Type;
        public bool Container;
        public int CostperLevel { get; set; }

        public bool RequiresVariant;
        public int SpecialPointsPerLevel;
        public bool SpecialContainer;
        public bool EnforceMaxLevel;

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

        [JsonIgnore]
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

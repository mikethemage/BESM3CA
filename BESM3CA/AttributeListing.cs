using System.Collections.Generic;

namespace BESM3CA
{
    class AttributeListing
    {
        public int ID
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        private int _id;

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        private string _name;

        public string CostperLevelDesc;
        public string Progression;
        public int MaxLevel;
        public string Stat;
        public string Page;
        public bool Human;
        public string Type;
        public bool Container;
        public int CostperLevel
        {
            get
            {
                return _costperlevel;
            }
            set
            {
                _costperlevel = value;
            }
        }

        private int _costperlevel;

        public bool RequiresVariant;
        public int SpecialPointsPerLevel;
        public bool SpecialContainer;
        public bool EnforceMaxLevel;

        SortedList<string, AttributeListing> Children;


        List<VariantListing> Variants;

        public AttributeListing()
        {
            CostperLevelDesc = "";
            Progression = "";

            Children = new SortedList<string, AttributeListing>();
            Variants = new List<VariantListing>();
        }

        public void AddChild(AttributeListing Child)
        {
            if (Child != null)
            {
                Children.Add(Child.Name, Child);
            }
        }
    }
}

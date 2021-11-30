using BESM3CAData.Listings.Serialization;
using BESM3CAData.Model;
using System.Collections.Generic;
using System.Linq;
using BESM3CAData.Control;
using System.ComponentModel;

namespace BESM3CAData.Listings
{
    public abstract class DataListing : INotifyPropertyChanged
    {
        //Properties:
        //Everything should have:
        public int ID { get; private set; }
        public string Name { get; private set; }
        public string Type { get; private set; }
        public string Description { get; private set; }  //Character doesn't need description really
        public List<DataListing> Children { get; private set; }

        //  To check if still needed:        
        private string Stat { get; set; }
        private string Page { get; set; }
        private bool Human { get; set; }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this,
                    new PropertyChangedEventArgs(propertyName));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public void RefreshFilteredPotentialChildren(string filter)
        {
            List<DataListing> selectedAttributeChildren = Children;
            if (selectedAttributeChildren != null)
            {
                //LINQ Version:
                List<DataListing> filteredAttList = selectedAttributeChildren
                    .Where(a => a.ID > 0 && (filter == "All" || filter == "" || a.Type == filter))
                    .OrderBy(a => a.Type)
                    .ThenBy(a => a.Name)
                    .ToList();

                FilteredPotentialChildren = filteredAttList;
            }
            else
            {
                FilteredPotentialChildren = null;
            }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value != _isSelected)
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }

        private List<DataListing> _filteredPotentialChildren;
        public List<DataListing> FilteredPotentialChildren
        {
            get { return _filteredPotentialChildren; }
            set
            {
                _filteredPotentialChildren = value;
                OnPropertyChanged(nameof(FilteredPotentialChildren));
            }
        }



        //Constructor:
        public DataListing()
        {
            Children = new List<DataListing>();
        }


        //Methods:
        public abstract BaseNode CreateNode(string notes, RPGEntity controller, int level = 1, int pointAdj = 0);
        //{
        //    return new DataNode(this, notes, controller);
        //}

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
                Stat = this.Stat,
                Page = this.Page,
                Human = this.Human,
                Type = this.Type,
                Description = this.Description,
                PointsContainer = false,
                SpecialContainer = false,
                RequiresVariant = false,
                MultiGenre = false,
                HasLevel = false
            };

            //Convert childrenlist to string:
            IEnumerable<int> ChildIDs = from child in Children
                                        select child.ID;

            result.ChildrenList = string.Join(",", ChildIDs);

            return result;
        }

        public DataListing(DataListingSerialized data)
        {
            ID = data.ID;
            Name = data.Name;
            Stat = data.Stat;
            Page = data.Page;
            Human = data.Human;
            Type = data.Type;
            Description = data.Description;

            Children = new List<DataListing>();
        }
    }
}

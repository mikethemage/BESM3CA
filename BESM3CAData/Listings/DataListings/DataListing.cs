using BESM3CAData.Listings.Serialization;
using BESM3CAData.Model;
using System.Collections.Generic;
using System.Linq;
using BESM3CAData.Control;

namespace BESM3CAData.Listings
{
    public class DataListing
    {
        //Properties:



        //Everything should have:
        public int ID { get; private set; }
        public string Name { get; private set; }
        public string Type { get; private set; }
        public string Description { get; private set; }
        public List<DataListing> Children { get; private set; }


        //  To check if still needed:        
        private string Stat { get; set; }
        private string Page { get; set; }
        private bool Human { get; set; }

        public virtual bool MultiGenre
        {
            get { return false; }
        }

        public virtual bool HasLevel
        {
            get
            {
                return false;
            }
        }

        public virtual DataNode CreateNode(string notes, DataController controller, int level = 1, int pointAdj = 0)
        {
            return new DataNode(this, notes, controller);
        }


        //Constructor:
        public DataListing()
        {

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
                Stat = this.Stat,
                Page = this.Page,
                Human = this.Human,
                Type = this.Type,
                Description = this.Description,
                HasLevel = this.HasLevel,
                MultiGenre=this.MultiGenre
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

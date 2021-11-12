using BESM3CAData.Control;
using BESM3CAData.Listings;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Xml;

namespace BESM3CAData.Model
{
    public abstract class BaseNode
    {
        //Fields:
        private int _lastChildOrder;
        protected int _points;
        private bool _pointsUpToDate;
        

        public DataListing AssociatedListing { get; private set; }


        //Properties:
        public DataController AssociatedController { get; private set; }
        public int ID { get; private set; }
        public string Name { get; set; }
        public string Notes { get; set; }


        //Tree structure properties:
        public BaseNode FirstChild { get; private set; }
        public BaseNode Parent { get; private set; }
        public int NodeOrder { get; private set; }
        public BaseNode Next { get; private set; }
        public BaseNode Prev { get; private set; }

        public ObservableCollection<BaseNode> Children { get; private set; } = new ObservableCollection<BaseNode>();


        public virtual string DisplayText
        {
            get
            {
                return $"{Name} ({GetPoints()} Points)";
            }
        }

        public List<DataListing> PotentialChildren
        {
            get
            {
                if (AssociatedListing != null)
                {
                    return AssociatedListing.Children;
                }
                else
                {
                    return null;
                }
            }
        }

        public bool PointsUpToDate
        {
            get
            {
                return _pointsUpToDate;
            }
            protected set
            {
                _pointsUpToDate = value;
                if (value == false && Parent != null)
                {
                    Parent.PointsUpToDate = false;
                }
            }
        }

        public bool Useable { get; private set; }

        //Constructors:
        public BaseNode(DataController controller,string notes = "")
        {
            //Default constructor for data loading only
            AssociatedController = controller;
            //Need to set attribute seperately in order to use this!
            Useable = false;
            Notes = notes;
            NodeOrder = 1;
            FirstChild = null;
            Parent = null;
            _lastChildOrder = 0;
            Next = null;
            Prev = null;
            _pointsUpToDate = false;
        }


        public BaseNode(DataListing attribute, DataController controller, string notes="")
        {
            Debug.Assert(controller.SelectedListingData != null);  //Check if we have listing data...

            AssociatedController = controller;
            AssociatedListing = attribute;
            Name = attribute.Name;
            ID = attribute.ID;
            Useable = true;
            Notes = notes;
            NodeOrder = 1;
            FirstChild = null;
            Parent = null;
            _lastChildOrder = 0;
            Next = null;
            Prev = null;
            _pointsUpToDate = false;
        }


        //Methods:
        public abstract int GetPoints();

        public List<string> GetTypesForFilter()
        {
            //LINQ Version:
            List<string> tempList = (

                                     from AttChild in PotentialChildren
                                     join TypeValue in AssociatedController.SelectedListingData.TypeList on AttChild.Type equals TypeValue.Name
                                     orderby TypeValue.TypeOrder
                                     select AttChild.Type

                                     ).Distinct().ToList();
            tempList.Insert(0, "All");
            return tempList;
        }

        public List<DataListing> GetFilteredPotentialChildren(string filter)
        {
            List<DataListing> selectedAttributeChildren = PotentialChildren;
            if (selectedAttributeChildren != null)
            {
                //LINQ Version:
                List<DataListing> filteredAttList = selectedAttributeChildren
                    .Where(a => a.ID > 0 && (filter == "All" || filter == "" || a.Type == filter))
                    .OrderBy(a => a.Type)
                    .ThenBy(a => a.Name)
                    .ToList();

                return filteredAttList;
            }
            else
            {
                return null;
            }
        }

        public void AddChild(BaseNode child)
        {
            if (FirstChild == null)
            {
                FirstChild = child;
            }
            else
            {
                BaseNode temp = FirstChild;
                while (temp.Next != null)
                {
                    temp = temp.Next;
                }
                temp.Next = child;
                child.Prev = temp;
            }

            child.Parent = this;
            _lastChildOrder++;
            child.NodeOrder = _lastChildOrder;
            Children.Add(child);
            _pointsUpToDate = false;
        }

        public void Delete()
        {
            if (Parent != null)
            {
                if (Parent.FirstChild == this)
                {
                    Parent.FirstChild = Next;
                }
                if (Next != null)
                {
                    Next.Prev = null;
                }
                if (Prev != null)
                {
                    Prev.Next = null;
                }

                Parent.Children.Remove(this);

                Parent.PointsUpToDate = false;

                Parent = null;
            }
        }

        public void MoveUp()
        {
            if (Prev != null)
            {
                BaseNode temp = Prev;

                if (temp.Parent.FirstChild == temp)
                {
                    temp.Parent.FirstChild = this;
                }

                if (Next != null)
                {
                    Next.Prev = temp;
                }
                if (temp.Prev != null)
                {
                    temp.Prev.Next = this;
                }

                Prev = temp.Prev;
                temp.Next = Next;
                Next = temp;
                temp.Prev = this;
                int tempNodeOrder = NodeOrder;
                NodeOrder = temp.NodeOrder;
                temp.NodeOrder = tempNodeOrder;
            }
        }

        public void MoveDown()
        {
            if (Next != null)
            {
                BaseNode temp = Next;

                if (Parent.FirstChild == this)
                {
                    Parent.FirstChild = temp;
                }

                if (Prev != null)
                {
                    Prev.Next = temp;
                }
                if (temp.Next != null)
                {
                    temp.Next.Prev = this;
                }

                Next = temp.Next;
                temp.Prev = Prev;
                Prev = temp;
                temp.Next = this;
                int tempNodeOrder = NodeOrder;
                NodeOrder = temp.NodeOrder;
                temp.NodeOrder = tempNodeOrder;
            }
        }

        public BaseNode AddChildAttribute(DataListing attribute)
        {
            BaseNode Temp = attribute.CreateNode("", AssociatedController);
            AddChild(Temp);
            return Temp;
        }

        public virtual void InvalidateGenrePoints()
        {
            BaseNode child = FirstChild;
            while (child != null)
            {
                child.InvalidateGenrePoints();
                child = child.Next;
            }
        }

        //Getting stats:
        public abstract CalcStats GetStats();

        //XML:
        public void SaveXML(XmlTextWriter textWriter)
        {
            textWriter.WriteStartElement(GetType().Name);
            textWriter.WriteAttributeString("Name", Name);
            textWriter.WriteAttributeString("ID", ID.ToString());
            textWriter.WriteStartElement("AdditionalData");
            SaveAdditionalXML(textWriter);
            textWriter.WriteEndElement();
            textWriter.WriteStartElement("Notes");
            textWriter.WriteString(Notes);
            textWriter.WriteEndElement();
            textWriter.WriteEndElement();
        }

        public abstract void SaveAdditionalXML(XmlTextWriter textWriter);

        public void LoadXML(XmlTextReader reader)
        {
            while (reader.NodeType != XmlNodeType.None)
            {
                // loading node attributes
                int attributeCount = reader.AttributeCount;
                if (attributeCount > 0)
                {
                    for (int i = 0; i < attributeCount; i++)
                    {
                        reader.MoveToAttribute(i);
                        switch (reader.Name)
                        {
                            case "Name":
                                Name = reader.Value;
                                break;
                            case "ID":
                                ID = int.Parse(reader.Value);
                                if (AssociatedController.SelectedListingData != null)
                                {
                                    AssociatedListing = AssociatedController.SelectedListingData.AttributeList.Find(n => n.ID == ID);
                                    Useable = true;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }

                reader.Read();

                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.Name == "Notes")
                    {
                        Notes = reader.ReadString();
                    }

                    if (reader.Name == "AdditionalData")
                    {
                        LoadAdditionalXML(reader);
                    }
                    
                }
                else if (reader.NodeType == XmlNodeType.EndElement)
                {
                    if (reader.Name.EndsWith(GetType().Name) || reader.Name.EndsWith("CharacterData") || reader.Name.EndsWith("AttributeData"))
                    {
                        break;
                    }
                }
            }
        }

        public abstract void LoadAdditionalXML(XmlTextReader reader);
    }
}

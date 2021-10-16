using BESM3CAData.Templates;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace BESM3CAData.Model
{
    public abstract class NodeData
    {
        private int _ID;
        protected NodeData _firstChild;
        protected NodeData _Parent;
        private int _lastChildOrder;
        protected int _points;
        private bool _pointsUpToDate;
        //protected TemplateData _associatedTemplate;

        public Controller AssociatedController;

        //Properties:
        public int NodeOrder { get; set; }
        public NodeData Next { get; set; }
        public NodeData Prev { get; set; }

        public virtual string DisplayText
        {
            get
            {
                return Name + " (" + GetPoints() + " Points)";
            }
        }

        public abstract bool HasCharacterStats
        {
            get;
        }
        public abstract bool HasLevelStats
        {
            get;
        }

        public abstract bool HasPointsStats
        {
            get;
        }

        public abstract List<AttributeListing> PotentialChildren
        {
            get;
        }

        
        public List<string> GetTypesForFilter()
        {
            //LINQ Version:
            List<string> tempList= (from AttChild in PotentialChildren
                                    orderby AttChild.Type
                                    select AttChild.Type).Distinct().ToList();
            tempList.Insert(0, "All");
            return tempList;
        }


        public List<ListItems> GetFilteredPotentialChildren(string filter)
        {
            List<AttributeListing> SelectedAttributeChildren = PotentialChildren;
            if (SelectedAttributeChildren != null)
            {
                //LINQ Version:
                List<ListItems> FilteredAttList = (from Att in SelectedAttributeChildren
                                                   where Att.ID > 0 &&
                                                   (filter == "All" || filter == "" || Att.Type == filter)

                                                   orderby Att.Type, Att.Name
                                                   select new ListItems(Att.Name, Att.ID, Att.Type)).ToList();

                //Add dividers:
                string Type = "";
                for (int i = 0; i < FilteredAttList.Count; i++)
                {
                    if (Type != FilteredAttList[i].Type)
                    {
                        if (Type != "")
                        {
                            FilteredAttList.Insert(i, new ListItems("-------------------------", 0));
                            i++;
                        }
                        Type = FilteredAttList[i].Type;
                        FilteredAttList.Insert(i, new ListItems(Type + ":", 0));
                        i++;
                        FilteredAttList.Insert(i, new ListItems("-------------------------", 0));
                        i++;
                    }
                }

                return FilteredAttList;
            }
            else
            {
                return null;
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
                if (value == false && _Parent != null)
                {
                    _Parent.PointsUpToDate = false;
                }
            }
        }

        public string Name { get; set; }

        public int ID
        {
            get
            {
                return _ID;
            }
        }

        public string Notes { get; set; }

        public NodeData Children
        {
            get
            {
                return _firstChild;
            }
        }

        public NodeData Parent
        {
            get
            {
                return _Parent;
            }
        }


        //Constructors:
        public NodeData(string attributeName, int attributeID, string notes, Controller controller)
        {
            AssociatedController = controller;
            Name = attributeName;
            _ID = attributeID;
            Notes = notes;

            NodeOrder = 1;
            _firstChild = null;
            _Parent = null;
            _lastChildOrder = 0;
            Next = null;
            Prev = null;
            _pointsUpToDate = false;
        }

        //Member Functions:
        public abstract int GetPoints();

        public void AddChild(NodeData child)
        {
            if (_firstChild == null)
            {
                _firstChild = child;
            }
            else
            {
                NodeData temp = _firstChild;
                while (temp.Next != null)
                {
                    temp = temp.Next;
                }
                temp.Next = child;
                child.Prev = temp;
            }

            child._Parent = this;
            _lastChildOrder++;
            child.NodeOrder = _lastChildOrder;
            _pointsUpToDate = false;
        }

        public void Delete()
        {
            if (Parent != null)
            {
                if (Parent._firstChild == this)
                {
                    Parent._firstChild = Next;
                }
                if (Next != null)
                {
                    Next.Prev = null;
                }
                if (Prev != null)
                {
                    Prev.Next = null;
                }
                Parent.PointsUpToDate = false;

                _Parent = null;
            }
        }

        public void MoveUp()
        {
            if (Prev != null)
            {
                NodeData temp = Prev;

                if (temp.Parent._firstChild == temp)
                {
                    temp.Parent._firstChild = this;
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
                NodeData temp = Next;

                if (Parent._firstChild == this)
                {
                    Parent._firstChild = temp;
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

        public AttributeData AddChildAttribute(string attributeName, int attributeID)
        {
            AttributeData Temp = new AttributeData(attributeName, attributeID, "", AssociatedController);
            AddChild(Temp);
            return Temp;
        }

        public virtual void InvalidateGenrePoints()
        {
            NodeData child = Children;
            while (child != null)
            {
                child.InvalidateGenrePoints();
                child = child.Next;
            }
        }

        //XML:
        public void SaveXML(XmlTextWriter textWriter)
        {
            textWriter.WriteStartElement(GetType().Name);
            textWriter.WriteAttributeString("Name", Name);
            textWriter.WriteAttributeString("ID", _ID.ToString());
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
                                _ID = int.Parse(reader.Value);
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
                    else
                    {
                        // loading node attributes
                        attributeCount = reader.AttributeCount;
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
                                        _ID = int.Parse(reader.Value);

                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                }
                else if (reader.NodeType == XmlNodeType.EndElement)
                {
                    if (reader.Name.EndsWith(GetType().Name))
                    {
                        break;
                    }
                }
            }
        }

        public abstract void LoadAdditionalXML(XmlTextReader reader);


        //Getting stats:
        public abstract CalcStats GetStats();
    }
}

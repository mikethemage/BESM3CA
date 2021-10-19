using BESM3CAData.Templates;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace BESM3CAData.Model
{
    public abstract class NodeData
    {
        //Fields:
        private int _lastChildOrder;
        protected int _points;
        private bool _pointsUpToDate;

        //Properties:
        public Controller AssociatedController { get; set; }
        public int ID { get; private set; }
        public string Name { get; set; }
        public string Notes { get; set; }

        public NodeData FirstChild { get; private set; }
        public NodeData Parent { get; private set; }
        public int NodeOrder { get; set; }
        public NodeData Next { get; private set; }
        public NodeData Prev { get; private set; }

        public virtual string DisplayText
        {
            get
            {
                return $"{Name} ({GetPoints()} Points)";
            }
        }

        public abstract bool HasCharacterStats { get; }
        public abstract bool HasLevelStats { get; }
        public abstract bool HasPointsStats { get; }
        public abstract List<AttributeListing> PotentialChildren { get; }

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

        //Constructors:
        public NodeData(string attributeName, int attributeID, string notes, Controller controller)
        {
            AssociatedController = controller;
            Name = attributeName;
            ID = attributeID;
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
        public List<string> GetTypesForFilter()
        {
            //LINQ Version:
            List<string> tempList = (from AttChild in PotentialChildren
                                     orderby AttChild.Type
                                     select AttChild.Type).Distinct().ToList();
            tempList.Insert(0, "All");
            return tempList;
        }

        public List<AttributeListing> GetFilteredPotentialChildren(string filter)
        {
            List<AttributeListing> selectedAttributeChildren = PotentialChildren;
            if (selectedAttributeChildren != null)
            {
                //LINQ Version:
                List<AttributeListing> filteredAttList = selectedAttributeChildren
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

        public abstract int GetPoints();

        public void AddChild(NodeData child)
        {
            if (FirstChild == null)
            {
                FirstChild = child;
            }
            else
            {
                NodeData temp = FirstChild;
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
                Parent.PointsUpToDate = false;

                Parent = null;
            }
        }

        public void MoveUp()
        {
            if (Prev != null)
            {
                NodeData temp = Prev;

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
                NodeData temp = Next;

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

        public AttributeData AddChildAttribute(AttributeListing attribute)
        {
            AttributeData Temp = new AttributeData(attribute, "", AssociatedController);
            AddChild(Temp);
            return Temp;
        }

        public virtual void InvalidateGenrePoints()
        {
            NodeData child = FirstChild;
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
                                        ID = int.Parse(reader.Value);

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
    }
}

using System.Collections.Generic;
using System.Xml;
using BESM3CA.Templates;
using System.Linq;

namespace BESM3CA.Model
{
    class NodeData 
    {
        //Fields:
        private string _name;
        private int _ID;
        private string _notes;
        protected NodeData _firstChild;
        protected NodeData _Parent;
        private int _lastChildOrder;
        protected int _points;
        private bool _pointsUpToDate;
        protected TemplateData _associatedTemplate;


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
                
        public virtual bool HasCharacterStats
        {
            get
            {
                //Virtual for overrides
                return false;
            }
        }
        public virtual bool HasLevelStats
        {
            get
            {
                //Virtual for overrides
                return false;
            }
        }
        public virtual bool HasPointsStats
        {
            get
            {
                //Virtual for overrides
                return false;
            }
        }
        
        public virtual List<AttributeListing> PotentialChildren
        {
            get
            {
                //Virtual for overrides
                return null;
            }
        }        

        public List<ListItems> GetFilteredPotentialChildren(string filter)
        {
            List<AttributeListing> SelectedAttributeChildren = PotentialChildren;
            if (SelectedAttributeChildren != null)
            {
                //LINQ Version:
                List<ListItems> FilteredAttList = (from Att in SelectedAttributeChildren
                                                   where
                                                   (
                                                   //cbFilter.SelectedIndex == -1 || 
                                                   filter == "All" || filter == "" || Att.Type == filter)

                                                   orderby Att.Type, Att.Name
                                                   select new ListItems(Att.Name, Att.ID, Att.Type)).ToList();

                //Add dividers:
                string Type = "";
                for (int i = 0; i<FilteredAttList.Count;i++)
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
        public int ID
        {
            get
            {
                return _ID;
            }
        }

        public string Notes
        {
            get
            {
                return _notes;
            }
            set
            {

                _notes = value;
            }
        }

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
        public NodeData(string attributeName, int attributeID, string notes, TemplateData templateData)
        {
            _associatedTemplate = templateData;
            _name = attributeName;
            _ID = attributeID;
            _notes = notes;

            NodeOrder = 1;
            _firstChild = null;
            _Parent = null;
            _lastChildOrder = 0;
            Next = null;
            Prev = null;
            _pointsUpToDate = false;
        }

        //Member Functions:
        public virtual int GetPoints()
        {
            //Virtual for derived classes
            return 0;
        }

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

        public AttributeData AddChildAttribute(string attributeName, int attributeID )
        {
            AttributeData Temp = new AttributeData(attributeName, attributeID, "", _associatedTemplate);
            AddChild(Temp);
            return Temp;
        }


        //XML:
        public void SaveXML(XmlTextWriter textWriter)
        {
            textWriter.WriteStartElement(GetType().Name);
            textWriter.WriteAttributeString("Name", _name);
            textWriter.WriteAttributeString("ID", _ID.ToString());
            textWriter.WriteStartElement("AdditionalData");
            //textWriter.WriteAttributeString("Type", GetType().Name);
            SaveAdditionalXML(textWriter);
            textWriter.WriteEndElement();
            textWriter.WriteStartElement("Notes");
            textWriter.WriteString(_notes);
            textWriter.WriteEndElement();
            textWriter.WriteEndElement();
        }

        public virtual void SaveAdditionalXML(XmlTextWriter textWriter)
        {
            //Virtual for derived classes
        }

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
                                _name = reader.Value;
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
                        _notes = reader.ReadString();
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
                                        _name = reader.Value;
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

        public virtual void LoadAdditionalXML(XmlTextReader reader)
        {
            //Virtual for derived classes
        }


        //Getting stats:
        public virtual CalcStats GetStats()
        {
            //Virtual for override            
            return null;
        }

    }
}

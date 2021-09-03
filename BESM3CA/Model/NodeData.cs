using System.Collections.Generic;
using System.Xml;
using BESM3CA.Templates;
using System.Linq;

namespace BESM3CA.Model
{
    class NodeData
    {
        //Fields:
        protected string _name;
        protected int _ID;
        protected string _Notes;
        protected NodeData _FirstChild;
        protected NodeData _Parent;
        private int _LastChildOrder;
        protected int _points;
        private bool _pointsUpToDate;

        protected TemplateData _asscTemplate;


        //Properties:
        public int NodeOrder { get; set; }
        public NodeData Next { get; set; }
        public NodeData Prev{ get; set; }

        public virtual string DisplayText
        {
            get
            {
                return Name + " (" + GetPoints(_asscTemplate) + " Points)";
            }
        }

        //***
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
        //***


        public virtual List<AttributeListing> PotentialChildren
        {
            get
            {
                //Virtual for overrides
                return null;
            }
        }

        public List<ListItems> GetFilteredPotentialChildren(string Filter)
        {
            List<AttributeListing> SelectedAttributeChildren = PotentialChildren;
            if (SelectedAttributeChildren != null)
            {
                //LINQ Version:
                List<ListItems> FilteredAttList = (from Att in SelectedAttributeChildren
                                                   where
                                                   (
                                                   //cbFilter.SelectedIndex == -1 || 
                                                   Filter == "All" || Filter == "" || Att.Type == Filter)

                                                   orderby Att.Type, Att.Name
                                                   select new ListItems(Att.Name, Att.ID, Att.Type)).ToList();

                return FilteredAttList;
            }
            else
            { 
                return null;
            }
        }

        protected bool PointsUpToDate
        {
            get
            {
                return _pointsUpToDate;
            }
            set
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
                return _Notes;
            }
            set
            {

                _Notes = value;
            }
        }

        public NodeData Children
        {
            get
            {
                return _FirstChild;
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
        public NodeData(string AttributeName, int AttributeID, string Notes, TemplateData useTemplate)
        {
            _asscTemplate = useTemplate;
            _name = AttributeName;
            _ID = AttributeID;
            _Notes = Notes;

            NodeOrder = 1;
            _FirstChild = null;
            _Parent = null;
            _LastChildOrder = 0;
            Next = null;
            Prev = null;
            _pointsUpToDate = false;
        }

        //public NodeData()
        //{
        //    //Default Constructor - currently needed for loading code
        //    NodeOrder = 1;
        //    _FirstChild = null;
        //    _Parent = null;
        //    _LastChildOrder = 0;
        //    Next = null;
        //    Prev = null;
        //    _pointsUpToDate = false;
        //}


        //Member Functions:
        public virtual int GetPoints(TemplateData templateData)
        {
            //Virtual for derived classes
            return 0;
        }

        public void AddChild(NodeData Child)
        {
            if (_FirstChild == null)
            {
                _FirstChild = Child;                
            }
            else
            {
                NodeData temp = _FirstChild;
                while (temp.Next!=null)
                {
                    temp = temp.Next;
                }
                temp.Next = Child;
                Child.Prev = temp;
            }
            
            Child._Parent = this;
            _LastChildOrder++;
            Child.NodeOrder= _LastChildOrder;
            _pointsUpToDate = false;
        }

        public void Delete()
        {
            if(Parent!=null)
            {
                if(Parent._FirstChild==this)
                {
                    Parent._FirstChild = Next;                    
                }
                if (Next != null)
                {
                    Next.Prev = null;
                }
                if (Prev != null)
                {
                    Prev.Next = null;
                }
                _Parent = null;
            }           
        }

        public void MoveUp()
        {
            if(Prev!=null)
            {
                NodeData temp = Prev;

                if (temp.Parent._FirstChild == temp)
                {
                    temp.Parent._FirstChild = this;
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

                if(Parent._FirstChild==this)
                {
                    Parent._FirstChild = temp;
                }

                if(Prev!=null)
                {
                    Prev.Next = temp;
                }
                if(temp.Next!=null)
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
        

        //XML:
        public void SaveXML(XmlTextWriter textWriter)
        {
            textWriter.WriteStartElement(GetType().ToString());
            textWriter.WriteAttributeString("Name", _name);
            textWriter.WriteAttributeString("ID", _ID.ToString());
            textWriter.WriteStartElement("AdditionalData");
            textWriter.WriteAttributeString("Type", GetType().ToString());
            SaveAdditionalXML(textWriter);
            textWriter.WriteEndElement();
            textWriter.WriteStartElement("Notes");
            textWriter.WriteString(_Notes);
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
                        _Notes = reader.ReadString();
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
                    if (reader.Name == GetType().ToString())
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
    }
}

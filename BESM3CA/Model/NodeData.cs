using System.Xml;

namespace BESM3CA
{
    class NodeData
    {
        //Members:
        protected string _name;
        protected int _ID;
        protected string _Notes;
        protected NodeData _FirstChild;
        protected NodeData _Parent;
        private int _LastChildOrder;

        //Properties:
        public int NodeOrder { get; set; }
        public NodeData Next { get; set; }
        public NodeData Prev{ get; set; }

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

        //Member Functions:
        public void addChild(NodeData Child)
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
            
        }

        public void Delete()
        {
            if(Parent!=null)
            {
                if(Parent._FirstChild==this)
                {
                    Parent._FirstChild = this.Next;                    
                }
                if (this.Next != null)
                {
                    this.Next.Prev = null;
                }
                if (this.Prev != null)
                {
                    this.Prev.Next = null;
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

                if (this.Next != null)
                {
                    this.Next.Prev = temp;
                }
                if (temp.Prev != null)
                {
                    temp.Prev.Next = this;
                }

                this.Prev = temp.Prev;
                temp.Next = this.Next;
                this.Next = temp;
                temp.Prev = this;
                int tempNodeOrder = this.NodeOrder;
                this.NodeOrder = temp.NodeOrder;
                temp.NodeOrder = tempNodeOrder;

            }
        }

        public void MoveDown()
        {
            if (Next != null)
            {
                NodeData temp = Next;

                if(this.Parent._FirstChild==this)
                {
                    this.Parent._FirstChild = temp;
                }

                if(this.Prev!=null)
                {
                    this.Prev.Next = temp;
                }
                if(temp.Next!=null)
                {
                    temp.Next.Prev = this;
                }
                
                this.Next = temp.Next;
                temp.Prev = this.Prev;
                this.Prev = temp;
                temp.Next = this;
                int tempNodeOrder = this.NodeOrder;
                this.NodeOrder = temp.NodeOrder;
                temp.NodeOrder = tempNodeOrder;
            }
        }

        //Constructors:
        public NodeData(string AttributeName, int AttributeID, string Notes)
        {
            _name = AttributeName;
            _ID = AttributeID;
            _Notes = Notes;
            NodeOrder = 1;
            _FirstChild = null;
            _Parent = null;
            _LastChildOrder = 0;
            Next = null;
            Prev = null;
        }  
        
        public NodeData()
        {
            NodeOrder = 1;
            _FirstChild = null;
            _Parent = null;
            _LastChildOrder = 0;
            Next = null;
            Prev = null;
        }

        //XML:
        public void SaveXML(XmlTextWriter textWriter)
        {
            textWriter.WriteStartElement(this.GetType().ToString());
            textWriter.WriteAttributeString("Name", _name);
            textWriter.WriteAttributeString("ID", _ID.ToString());
            textWriter.WriteStartElement("AdditionalData");
            textWriter.WriteAttributeString("Type", this.GetType().ToString());
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
                    if (reader.Name == this.GetType().ToString())
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

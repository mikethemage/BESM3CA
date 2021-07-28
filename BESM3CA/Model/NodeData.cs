using System.Xml;
using System.Collections.Generic;

namespace BESM3CA
{
    class NodeData
    {
        //Members:
        protected string _name;
        protected int _ID;
        protected string _Notes;
        protected List<NodeData> _Children;
        protected NodeData _Parent;
        private int _LastChildOrder;

        //Properties:
        public int NodeOrder { get; set; }

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

        public List<NodeData> Children
        {
            get
            {
                return _Children;
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
            _Children.Add(Child);
            Child._Parent = this;
            _LastChildOrder++;
            Child.NodeOrder= _LastChildOrder;
            return;
        }

        public void Delete()
        {
            _Parent._Children.Remove(this);
            _Parent = null;
            return;
        }

        //Constructors:
        public NodeData(string AttributeName, int AttributeID, string Notes)
        {
            _name = AttributeName;
            _ID = AttributeID;
            _Notes = Notes;
            NodeOrder = 1;
            _Children = new List<NodeData>();
            _Parent = null;
            _LastChildOrder = 0;
        }  
        
        public NodeData()
        {
            NodeOrder = 1;
            _Children = new List<NodeData>();
            _Parent = null;
            _LastChildOrder = 0;
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

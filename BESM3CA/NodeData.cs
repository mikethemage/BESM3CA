using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;


namespace BESM3CA
{
    
    class NodeData
    {
        public int NodeOrder;

        string _name;
        int _ID;
        string _Notes;

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

        
        public int ID
        {
            get
            {
                return _ID;
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

        public NodeData(string AttributeName, int AttributeID, string Notes)
        {
            _name = AttributeName;
            _ID = AttributeID;
            _Notes = Notes;
            NodeOrder = 1;
        }

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
        }

        public NodeData()
        {
            NodeOrder = 1;
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
                     

                       _Notes= reader.ReadString();
                   
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
        }
    }
}

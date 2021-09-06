using System.Diagnostics;
using System.Xml;
using BESM3CA.Templates;

namespace BESM3CA.Model
{
    class SaveLoad
    {
        // Xml tag for node, e.g. 'node' in case of <node></node> 
        private const string XmlNodeTag = "node";

        // Xml attributes for node e.g. <node text="Asia" tag="" 
        // imageindex="1"></node>
        private const string XmlNodeTextAtt = "text";

        private const string XmlTemplateTag = "template";

        private static void SetAttributeValue(NodeData node,
                    string propertyName, string value)
        {
            if (propertyName == XmlNodeTextAtt)
            {
                node.Name = value;
            }
        }

        public static NodeData DeserializeXML(string fileName, TemplateData templateData)
        {
            //Needs completely re-writing:
            NodeData rootNode = null;

            XmlTextReader reader = null;
            try
            {                
                reader = new XmlTextReader(fileName);
                NodeData parentNode = null;
                NodeData newNode = null;

                while (reader.Read())
                {
                    if (reader.Name == XmlTemplateTag)
                    {
                        reader.Read();
                        if (reader.NodeType == XmlNodeType.Text)
                        {
                            //Read template name
                            Debug.Assert(reader.Value == "BESM3E");
                            //todo: load correct template

                        }
                    }
                    else if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.Name == XmlNodeTag)
                        {
                                                        
                        }
                        else
                        {
                            if (reader.Name.EndsWith(".CharacterData") )
                            {
                                newNode = new CharacterData(templateData);  //todo: refactor to take reference to template
                                newNode.LoadXML(reader);
                                if (rootNode == null)// set root node:
                                {
                                    rootNode = newNode;                                    
                                }

                                if(parentNode!=null)
                                {
                                    parentNode.AddChild(newNode);
                                }
                                
                                parentNode = newNode;
                            }
                            else if (reader.Name.EndsWith(".AttributeData") )
                            {
                                newNode = new AttributeData("", 0, "", templateData);//todo: refactor to take reference to template
                                newNode.LoadXML(reader);
                                if (parentNode != null)
                                {
                                    parentNode.AddChild(newNode);
                                }

                                parentNode = newNode;
                            }
                            else
                            {
                                
                            }

                        }

                    }

                    // moving up to parent if end tag is encountered
                    else if (reader.NodeType == XmlNodeType.EndElement)
                    {
                        if (reader.Name == XmlNodeTag)
                        {
                            if (parentNode != null)
                            {
                                newNode = newNode.Parent;
                                parentNode = newNode;
                            }

                        }
                    }

                    else if (reader.NodeType == XmlNodeType.XmlDeclaration)
                    {
                        //Ignore Xml Declaration                    
                    }

                    else if (reader.NodeType == XmlNodeType.None)
                    {
                        break;
                    }

                    else if (reader.NodeType == XmlNodeType.Text)
                    {
                        
                    }

                }
            }
            finally
            {                
                reader.Close();
            }

            return rootNode;
        }

        public static void SerializeXML(NodeData rootNode, string fileName, TemplateData templateData)
        {
            XmlTextWriter textWriter = new XmlTextWriter(fileName, System.Text.Encoding.ASCII);

            // writing the xml declaration tag
            textWriter.WriteStartDocument();
            textWriter.WriteStartElement("root");

            textWriter.WriteElementString(XmlTemplateTag, templateData.TemplateName);


            // writing the main tag that encloses all node tags
            textWriter.WriteStartElement("Data");

            // save the nodes, recursive method
            SaveNodes(rootNode, textWriter);

            textWriter.WriteEndElement();
            textWriter.WriteEndElement();
            textWriter.Close();
        }

        private static void SaveNodes(NodeData nodesCollection, XmlTextWriter textWriter)
        {
            NodeData node = nodesCollection;
            while (node != null)
            {
                textWriter.WriteStartElement(XmlNodeTag);
                textWriter.WriteAttributeString(XmlNodeTextAtt, node.DisplayText);

                node.SaveXML(textWriter);

                if (node.Children != null)
                {
                    SaveNodes(node.Children, textWriter);
                }

                textWriter.WriteEndElement();

                node = node.Next;

            }
        }
    }
}

using System.Diagnostics;
using System.IO;
using System.Xml;
using BESM3CA.Templates;
using BESM3CA.Model;

namespace BESM3CA.Control
{
    class SaveLoad
    {
        // Xml tag for node, e.g. 'node' in case of <node></node> 
        private const string XmlNodeTag = "node";

        // Xml attributes for node e.g. <node text="Asia" tag="" 
        // imageindex="1"></node>
        private const string XmlNodeTextAtt = "text";

        private const string XmlTemplateTag = "template";

        //private static void SetAttributeValue(NodeData node,
        //            string propertyName, string value)
        //{
        //    if (propertyName == XmlNodeTextAtt)
        //    {
        //        node.Name = value;
        //    }
        //}

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
                            if (reader.Name.EndsWith("CharacterData"))
                            {
                                newNode = new CharacterData(templateData);  //todo: refactor to take reference to template
                                newNode.LoadXML(reader);
                                if (rootNode == null)
                                {
                                    // set root node
                                    rootNode = newNode;
                                }

                                if (parentNode != null)
                                {
                                    parentNode.AddChild(newNode);
                                }

                                parentNode = newNode;
                            }
                            else if (reader.Name.EndsWith("AttributeData"))
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
            XmlTextWriter textWriter = new XmlTextWriter(fileName, System.Text.Encoding.UTF8);

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
                //textWriter.WriteAttributeString(XmlNodeTextAtt, node.DisplayText);

                node.SaveXML(textWriter);

                if (node.Children != null)
                {
                    SaveNodes(node.Children, textWriter);
                }

                textWriter.WriteEndElement();

                node = node.Next;
            }
        }

        public static void ExportNode(NodeData nodes, int tabdepth, TextWriter tw)
        {
            //Code to export to Text format:
            string tabstring = "";
            for (int i = 0; i < tabdepth; i++)
            {
                tabstring += ("\t");
            }
            bool isAttrib = false;

            NodeData current = nodes;
            while (current != null)
            {
                string nexttabstring;

                if (current.GetType() == typeof(CharacterData))
                {
                    //write stuff
                    // write a line of text to the file
                    tw.WriteLine(tabstring + current.DisplayText);

                    nexttabstring = tabstring + "\t";
                    tw.WriteLine(nexttabstring + "Mind: " + ((CharacterData)current).Mind);
                    tw.WriteLine(nexttabstring + "Body: " + ((CharacterData)current).Body);
                    tw.WriteLine(nexttabstring + "Soul: " + ((CharacterData)current).Soul);
                    tw.WriteLine();

                    CalcStats stats = CalcStats.GetStats(current);

                    tw.WriteLine(nexttabstring + "ACV: " + stats.ACV);
                    tw.WriteLine(nexttabstring + "DCV: " + stats.DCV);
                    tw.WriteLine(nexttabstring + "Health: " + stats.Health);
                    tw.WriteLine(nexttabstring + "Energy: " + stats.Energy);
                    tw.WriteLine();
                }
                else
                {
                    if (((AttributeData)current).AttributeType == "Attribute")
                    {
                        //write stuff
                        // write a line of text to the file
                        tw.WriteLine(tabstring + current.DisplayText);

                        nexttabstring = tabstring + "\t";

                        if (((AttributeData)current).Name == "Item")
                        {

                            tw.WriteLine(tabstring + "(");
                        }
                        else
                        {
                            tw.WriteLine(nexttabstring + "Level " + ((AttributeData)current).Level + " x " + ((AttributeData)current).PointsPerLevel + " = " + (((AttributeData)current).Level * ((AttributeData)current).PointsPerLevel));
                        }

                        tw.WriteLine(nexttabstring + "Description: " + ((AttributeData)current).AttributeDescription);

                        isAttrib = true;
                    }
                    else
                    {
                        //write stuff
                        // write a line of text to the file
                        tw.WriteLine(tabstring + current.DisplayText + " Level " + ((AttributeData)current).Level);
                        nexttabstring = tabstring + "\t";
                    }
                }
                if (((NodeData)current).Notes != "")
                {
                    tw.WriteLine(nexttabstring + "[Notes: " + (((NodeData)current).Notes).Replace("\n", "\n" + nexttabstring) + "]");
                }

                ExportNode(current.Children, tabdepth + 1, tw);

                if (isAttrib)
                {
                    if (((AttributeData)current).Name == "Item")
                    {
                        tw.WriteLine(tabstring + ") / 2");
                    }
                    tw.WriteLine();
                }
                current = current.Next;
            }
        }
    }
}

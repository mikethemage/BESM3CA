using System.Diagnostics;
using System.IO;
using System.Xml;
using BESM3CAData.Templates;
using BESM3CAData.Model;


namespace BESM3CAData.Control
{
    public class SaveLoad
    {
        // Xml tag for node, e.g. 'node' in case of <node></node> 
        private const string XmlNodeTag = "node";

        private const string XmlTemplateTag = "template";

        private const string XmlGenreTag = "genre";

        public static NodeData DeserializeXML(string fileName, Controller controller)
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
                    if (reader.Name == XmlTemplateTag && reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Text)
                            {
                                //Read template name
                                Debug.Assert(reader.Value == "BESM3E");
                                //todo: load correct template
                            }
                        }
                    }
                    else if (reader.Name == XmlGenreTag && reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Text)
                            {
                                //Read genre name
                                controller.SelectedGenreIndex = controller.SelectedTemplate.Genres.IndexOf(reader.Value);
                            }
                        }
                    }
                    else if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.Name == XmlNodeTag)
                        {
                            //Do nothing
                        }
                        else
                        {
                            if (reader.Name.EndsWith("CharacterData"))
                            {
                                newNode = new CharacterData(controller);  //todo: refactor to take reference to template
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
                                newNode = new AttributeData("", 0, "", controller);//todo: refactor to take reference to template
                                newNode.LoadXML(reader);
                                if (parentNode != null)
                                {
                                    parentNode.AddChild(newNode);
                                }

                                parentNode = newNode;
                            }
                            else
                            {
                                //Do nothing
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
                        //Do Nothing
                    }

                }
            }
            finally
            {
                reader.Close();
            }

            return rootNode;
        }

        public static void SerializeXML(NodeData rootNode, string fileName, Controller controller)
        {
            XmlTextWriter textWriter = new XmlTextWriter(fileName, System.Text.Encoding.UTF8);

            // writing the xml declaration tag
            textWriter.WriteStartDocument();
            textWriter.WriteStartElement("root");

            textWriter.WriteElementString(XmlTemplateTag, controller.SelectedTemplate.TemplateName);
            if (controller.SelectedGenreIndex > -1)
            {
                textWriter.WriteElementString(XmlGenreTag, controller.SelectedTemplate.Genres[controller.SelectedGenreIndex]);
            }

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
                    //write a line of text to the file
                    tw.WriteLine(tabstring + current.DisplayText);

                    nexttabstring = tabstring + "\t";
                    tw.WriteLine(nexttabstring + "Mind: " + ((CharacterData)current).Mind);
                    tw.WriteLine(nexttabstring + "Body: " + ((CharacterData)current).Body);
                    tw.WriteLine(nexttabstring + "Soul: " + ((CharacterData)current).Soul);
                    tw.WriteLine();

                    CalcStats stats = current.GetStats();

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
                        //write a line of text to the file
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

                        if (((AttributeData)current).AttributeDescription != "")
                        {
                            tw.WriteLine(nexttabstring + "Description: " + ((AttributeData)current).AttributeDescription);
                        }
                        isAttrib = true;
                    }
                    else
                    {
                        //write stuff
                        //write a line of text to the file
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

        public static void ExportHTMLNode(NodeData nodes, int tabdepth, TextWriter tw)
        {
            //Code to export to HTML format:
            bool isAttrib = false;

            NodeData current = nodes;
            while (current != null)
            {
                if (current.GetType() == typeof(CharacterData))
                {
                    tw.WriteLine("<li class=\"CharacterNode\">");

                    tw.WriteLine("<div class=\"CharacterStats\">");
                    tw.WriteLine("<h2>" + current.DisplayText + "</h2>");

                    tw.WriteLine("<div class=\"CoreStats\">");
                    tw.WriteLine("<strong>Mind:</strong> " + ((CharacterData)current).Mind);
                    tw.WriteLine("<strong>Body:</strong> " + ((CharacterData)current).Body);
                    tw.WriteLine("<strong>Soul:</strong> " + ((CharacterData)current).Soul);
                    tw.WriteLine("</div>");

                    CalcStats stats = current.GetStats();

                    tw.WriteLine("<div class=\"DerivedStats\">");
                    tw.WriteLine("<strong>ACV:</strong> " + stats.ACV);
                    tw.WriteLine("<strong>DCV:</strong> " + stats.DCV);
                    tw.WriteLine("<strong>Health:</strong> " + stats.Health);
                    tw.WriteLine("<strong>Energy:</strong> " + stats.Energy);
                    tw.WriteLine("</div>");

                    tw.WriteLine("</div>");

                    if (current.Notes != "")
                    {
                        tw.WriteLine("<div class=\"Notes\">");
                        tw.WriteLine("[Notes: " + current.Notes.Replace("\n", "<br>") + "]");
                        tw.WriteLine("</div>");
                    }

                    if (current.Children != null)
                    {
                        tw.WriteLine("<ul class=\"AttributeList\">");

                        ExportHTMLNode(current.Children, tabdepth + 1, tw);

                        tw.WriteLine("</ul>");
                    }

                    tw.WriteLine("</li>");
                }
                else
                {
                    if (((AttributeData)current).AttributeType == "Attribute")
                    {
                        tw.WriteLine("<li class=\"AttributeNode\">");
                        tw.WriteLine("<h3>" + current.DisplayText + "</h3>");

                        if (((AttributeData)current).Name == "Item")
                        {
                            tw.WriteLine("(");
                        }
                        else
                        {
                            tw.WriteLine("<p>Level " + ((AttributeData)current).Level + " x " + ((AttributeData)current).PointsPerLevel + " = " + (((AttributeData)current).Level * ((AttributeData)current).PointsPerLevel) + "</p>");
                        }

                        if (((AttributeData)current).AttributeDescription != "")
                        {
                            tw.WriteLine("<p>Description: " + ((AttributeData)current).AttributeDescription + "</p>");
                        }
                        isAttrib = true;
                    }
                    else
                    {
                        tw.WriteLine("<li class=\"" + ((AttributeData)current).AttributeType + "Node\">");
                        tw.WriteLine(current.DisplayText + " Level " + ((AttributeData)current).Level);
                    }
                    if (current.Notes != "")
                    {
                        tw.WriteLine("<p>[Notes: " + (current.Notes).Replace("\n", "<br>") + "]</p>");
                    }

                    if (current.Children != null)
                    {
                        tw.WriteLine("<ul class=\"AttributeList\">");
                        ExportHTMLNode(current.Children, tabdepth + 1, tw);
                        tw.WriteLine("</ul>");
                    }
                    if (isAttrib)
                    {
                        if (((AttributeData)current).Name == "Item")
                        {
                            tw.WriteLine(") / 2");
                        }
                    }
                    tw.WriteLine("</li>");
                }

                current = current.Next;
            }
        }
    }
}

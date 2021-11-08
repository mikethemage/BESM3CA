using BESM3CAData.Listings;
using BESM3CAData.Model;
using System.IO;
using System.Xml;


namespace BESM3CAData.Control
{
    public class SaveLoad
    {
        // Xml tag for node, e.g. 'node' in case of <node></node> 
        private const string XmlNodeTag = "node";

        private const string XmlListingTag = "listing";
        private const string OldXmlListingTag = "template";

        private const string XmlGenreTag = "genre";

        public static BaseNode DeserializeXML(string fileName, DataController controller)
        {
            //Needs completely re-writing:
            BaseNode rootNode = null;

            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(fileName);
                BaseNode parentNode = null;
                BaseNode newNode = null;

                while (reader.Read())
                {
                    if ((reader.Name == XmlListingTag || reader.Name == OldXmlListingTag) && reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Text)
                            {
                                //Read Listing name      
                                ListingLocation selectedListing = controller.ListingDirectory.AvailableListings.Find(x => (x.ListingName == reader.Value));

                                //Only need to reload listings if different:
                                if (controller.SelectedListingData.ListingName != selectedListing.ListingName)
                                {
                                    //Load listing from file:
                                    controller.SelectedListingData = ListingData.JSONLoader(selectedListing);
                                }
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
                                controller.SelectedGenreIndex = controller.SelectedListingData.Genres.IndexOf(reader.Value);
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
                            if (reader.Name.EndsWith("CharacterData") || reader.Name.EndsWith("CharacterNode"))
                            {
                                newNode = new CharacterNode(controller);
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
                            else if (reader.Name.EndsWith("AttributeData") || reader.Name.EndsWith("AttributeNode"))
                            {
                                newNode = new AttributeNode(controller);
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

        public static void SerializeXML(BaseNode rootNode, string fileName, DataController controller)
        {
            XmlTextWriter textWriter = new XmlTextWriter(fileName, System.Text.Encoding.UTF8);

            // writing the xml declaration tag
            textWriter.WriteStartDocument();
            textWriter.WriteStartElement("root");

            textWriter.WriteElementString(XmlListingTag, controller.SelectedListingData.ListingName);
            if (controller.SelectedGenreIndex > -1)
            {
                textWriter.WriteElementString(XmlGenreTag, controller.SelectedListingData.Genres[controller.SelectedGenreIndex]);
            }

            // writing the main tag that encloses all node tags
            textWriter.WriteStartElement("Data");

            // save the nodes, recursive method
            SaveNodes(rootNode, textWriter);

            textWriter.WriteEndElement();
            textWriter.WriteEndElement();
            textWriter.Close();
        }

        private static void SaveNodes(BaseNode nodesCollection, XmlTextWriter textWriter)
        {
            BaseNode node = nodesCollection;
            while (node != null)
            {
                textWriter.WriteStartElement(XmlNodeTag);

                node.SaveXML(textWriter);

                if (node.FirstChild != null)
                {
                    SaveNodes(node.FirstChild, textWriter);
                }

                textWriter.WriteEndElement();

                node = node.Next;
            }
        }

        public static void ExportNode(BaseNode nodes, int tabdepth, TextWriter tw)
        {
            //Code to export to Text format:
            string tabstring = "";
            for (int i = 0; i < tabdepth; i++)
            {
                tabstring += ("\t");
            }
            bool isAttrib = false;

            BaseNode current = nodes;
            while (current != null)
            {
                string nexttabstring;

                if (current is CharacterNode currentCharacter)
                {
                    //write stuff
                    //write a line of text to the file
                    tw.WriteLine($"{tabstring}{current.DisplayText}");

                    nexttabstring = $"{tabstring}\t";
                    tw.WriteLine($"{nexttabstring}Mind: {currentCharacter.Mind}");
                    tw.WriteLine($"{nexttabstring}Body: {currentCharacter.Body}");
                    tw.WriteLine($"{nexttabstring}Soul: {currentCharacter.Soul}");
                    tw.WriteLine();

                    CalcStats stats = current.GetStats();

                    tw.WriteLine($"{nexttabstring}ACV: {stats.ACV}");
                    tw.WriteLine($"{nexttabstring}DCV: {stats.DCV}");
                    tw.WriteLine($"{nexttabstring}Health: {stats.Health}");
                    tw.WriteLine($"{nexttabstring}Energy: {stats.Energy}");
                    tw.WriteLine();
                }
                else if (current is AttributeNode currentAttribute)
                {
                    if (currentAttribute.AttributeType == "Attribute")
                    {
                        //write stuff
                        //write a line of text to the file
                        tw.WriteLine($"{tabstring}{current.DisplayText}");

                        nexttabstring = $"{tabstring}\t";

                        if (currentAttribute.Name == "Item")
                        {
                            tw.WriteLine($"{tabstring}(");
                        }
                        else
                        {
                            tw.WriteLine($"{nexttabstring}Level {currentAttribute.Level} x {currentAttribute.PointsPerLevel} = {currentAttribute.Level * currentAttribute.PointsPerLevel}");
                        }

                        if (currentAttribute.AttributeDescription != "")
                        {
                            tw.WriteLine($"{nexttabstring}Description: {currentAttribute.AttributeDescription}");
                        }
                        isAttrib = true;
                    }
                    else
                    {
                        //write stuff
                        //write a line of text to the file
                        tw.WriteLine($"{tabstring}{current.DisplayText} Level {currentAttribute.Level}");
                        nexttabstring = $"{tabstring}\t";
                    }
                }
                else
                {
                    //Should never get here
                    nexttabstring = tabstring;
                }

                if (current.Notes != "")
                {
                    tw.WriteLine($"{nexttabstring}[Notes: {current.Notes.Replace("\n", "\n" + nexttabstring)}]");
                }

                ExportNode(current.FirstChild, tabdepth + 1, tw);

                if (isAttrib)
                {
                    if (current.Name == "Item")
                    {
                        tw.WriteLine($"{tabstring}) / 2");
                    }
                    tw.WriteLine();
                }
                current = current.Next;
            }
        }

        public static void ExportHTMLNode(BaseNode nodes, int tabdepth, TextWriter tw)
        {
            //Code to export to HTML format:
            bool isAttrib = false;

            BaseNode current = nodes;
            while (current != null)
            {
                if (current is CharacterNode currentCharacter)
                {
                    tw.WriteLine("<li class=\"CharacterNode\">");

                    tw.WriteLine("<div class=\"CharacterStats\">");
                    tw.WriteLine($"<h2>{current.DisplayText}</h2>");

                    tw.WriteLine("<div class=\"CoreStats\">");
                    tw.WriteLine($"<strong>Mind:</strong> {currentCharacter.Mind}");
                    tw.WriteLine($"<strong>Body:</strong> {currentCharacter.Body}");
                    tw.WriteLine($"<strong>Soul:</strong> {currentCharacter.Soul}");
                    tw.WriteLine("</div>");

                    CalcStats stats = current.GetStats();

                    tw.WriteLine("<div class=\"DerivedStats\">");
                    tw.WriteLine($"<strong>ACV:</strong> {stats.ACV}");
                    tw.WriteLine($"<strong>DCV:</strong> {stats.DCV}");
                    tw.WriteLine($"<strong>Health:</strong> {stats.Health}");
                    tw.WriteLine($"<strong>Energy:</strong> {stats.Energy}");
                    tw.WriteLine("</div>");

                    tw.WriteLine("</div>");

                    if (current.Notes != "")
                    {
                        tw.WriteLine("<div class=\"Notes\">");
                        tw.WriteLine($"[Notes: {current.Notes.Replace("\n", "<br>")}]");
                        tw.WriteLine("</div>");
                    }

                    if (current.FirstChild != null)
                    {
                        tw.WriteLine("<ul class=\"AttributeList\">");

                        ExportHTMLNode(current.FirstChild, tabdepth + 1, tw);

                        tw.WriteLine("</ul>");
                    }

                    tw.WriteLine("</li>");
                }
                else
                {
                    if (((AttributeNode)current).AttributeType == "Attribute")
                    {
                        tw.WriteLine("<li class=\"AttributeNode\">");
                        tw.WriteLine($"<h3>{current.DisplayText}</h3>");

                        if (((AttributeNode)current).Name == "Item")
                        {
                            tw.WriteLine("(");
                        }
                        else
                        {
                            tw.WriteLine($"<p>Level {((AttributeNode)current).Level} x {((AttributeNode)current).PointsPerLevel} = {((AttributeNode)current).Level * ((AttributeNode)current).PointsPerLevel}</p>");
                        }

                        if (((AttributeNode)current).AttributeDescription != "")
                        {
                            tw.WriteLine($"<p>Description: {((AttributeNode)current).AttributeDescription}</p>");
                        }
                        isAttrib = true;
                    }
                    else
                    {
                        tw.WriteLine($"<li class=\"{((AttributeNode)current).AttributeType}Node\">");
                        tw.WriteLine($"{current.DisplayText} Level {((AttributeNode)current).Level}");
                    }
                    if (current.Notes != "")
                    {
                        tw.WriteLine($"<p>[Notes: {current.Notes.Replace("\n", "<br>")}]</p>");
                    }

                    if (current.FirstChild != null)
                    {
                        tw.WriteLine("<ul class=\"AttributeList\">");
                        ExportHTMLNode(current.FirstChild, tabdepth + 1, tw);
                        tw.WriteLine("</ul>");
                    }
                    if (isAttrib)
                    {
                        if (((AttributeNode)current).Name == "Item")
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

﻿using BESM3CAData.Listings;
using BESM3CAData.Model;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

            Version fileVersion=null;
            try
            {
                reader = new XmlTextReader(fileName);
                BaseNode parentNode = null;
                BaseNode newNode = null;

                while (reader.Read())
                {
                    if (reader.Name == "root" && reader.NodeType == XmlNodeType.Element)
                    {
                        int attributeCount = reader.AttributeCount;
                        if (attributeCount > 0)
                        {
                            for (int i = 0; i < attributeCount; i++)
                            {
                                reader.MoveToAttribute(i);
                                switch (reader.Name)
                                {
                                    case "version":
                                        fileVersion = new Version(reader.Value);
                                        break;                                    
                                    default:
                                        break;
                                }
                            }
                            if(fileVersion == null || fileVersion < new Version("0.2.2.0"))
                            {
                                ///version is too old
                                break;
                            }
                        }
                        else 
                        {
                            //No version number!
                            break;
                        }

                    }
                    else if ((reader.Name == XmlListingTag || reader.Name == OldXmlListingTag) && reader.NodeType == XmlNodeType.Element)
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
                                    controller.SelectedListingData = MasterListing.JSONLoader(selectedListing);
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
                                controller.CurrentEntity.GenreList.FirstOrDefault(x=>x.GenreName==reader.Value).IsSelected=true;
                                
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
                                newNode = new CharacterNode(controller.CurrentEntity);
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
                            else if (reader.Name.EndsWith("DataNode"))
                            {
                                switch (reader.Name)
                                {
                                    case "LevelableDataNode":
                                        newNode = new LevelableDataNode(controller.CurrentEntity);
                                        break;
                                    case "LevelableWithVariantDataNode":
                                        newNode = new LevelableWithVariantDataNode(controller.CurrentEntity);
                                        break;
                                    case "MultiGenreDataNode":
                                        newNode = new MultiGenreDataNode(controller.CurrentEntity);
                                        break;
                                    case "PointsContainerDataNode":
                                        newNode = new PointsContainerDataNode(controller.CurrentEntity);
                                        break;
                                    case "SpecialContainerDataNode":
                                        newNode = new SpecialContainerDataNode(controller.CurrentEntity);
                                        break;
                                    case "SpecialContainerWithVariantDataNode":
                                        newNode = new SpecialContainerWithVariantDataNode(controller.CurrentEntity);
                                        break;
                                    case "CompanionDataNode":
                                        newNode = new CompanionDataNode(controller.CurrentEntity);
                                        break;
                                    case "LevelableWithFreebieWithVariantDataNode":
                                        newNode = new LevelableWithFreebieWithVariantDataNode(controller.CurrentEntity);
                                        break;
                                    default:
                                        throw new InvalidDataException($"Unable to find correct node type for: {reader.Name}");
                                        //break;
                                }
                                if (newNode != null)
                                {
                                    newNode.LoadXML(reader);
                                    if (parentNode != null)
                                    {
                                        parentNode.AddChild(newNode);
                                    }

                                    parentNode = newNode;
                                }
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

        public static void SerializeXML(BaseNode rootNode, string fileName, RPGEntity controller)
        {
            XmlTextWriter textWriter = new XmlTextWriter(fileName, System.Text.Encoding.UTF8);

            // writing the xml declaration tag
            textWriter.WriteStartDocument();
            textWriter.WriteStartElement("root");
            textWriter.WriteAttributeString("version", System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString());

            textWriter.WriteElementString(XmlListingTag, controller.SelectedListingData.ListingName);
            if (controller.SelectedGenreEntry!=null)
            {
                textWriter.WriteElementString(XmlGenreTag, controller.SelectedGenreEntry.GenreName);
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
                else if (current is DataNode currentAttribute)
                {
                    if (currentAttribute.AttributeType == "Attribute")
                    {
                        //write stuff
                        //write a line of text to the file
                        tw.WriteLine($"{tabstring}{current.DisplayText}");

                        nexttabstring = $"{tabstring}\t";

                        if (currentAttribute is LevelableDataNode levelableDataNode)
                        {
                            tw.WriteLine($"{nexttabstring}Level {levelableDataNode.Level} x {levelableDataNode.PointsPerLevel} = {levelableDataNode.Level * levelableDataNode.PointsPerLevel}");
                        }
                        else
                        {
                            if (currentAttribute.Name == "Item")
                            {
                                tw.WriteLine($"{tabstring}(");
                            }
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
                        if (currentAttribute is LevelableDataNode levelableDataNode)
                        {
                            tw.WriteLine($"{tabstring}{current.DisplayText} Level {levelableDataNode.Level}");
                        }
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
                    if (((DataNode)current).AttributeType == "Attribute")
                    {
                        tw.WriteLine("<li class=\"AttributeNode\">");
                        tw.WriteLine($"<h3>{current.DisplayText}</h3>");






                        if (current is LevelableDataNode levelableDataNode)
                        {

                            tw.WriteLine($"<p>Level {(levelableDataNode).Level} x {(levelableDataNode).PointsPerLevel} = {(levelableDataNode).Level * (levelableDataNode).PointsPerLevel}</p>");
                        }
                        else
                        {
                            if (((DataNode)current).Name == "Item")
                            {
                                tw.WriteLine("(");
                            }
                        }

                        if (((DataNode)current).AttributeDescription != "")
                        {
                            tw.WriteLine($"<p>Description: {((DataNode)current).AttributeDescription}</p>");
                        }
                        isAttrib = true;
                    }
                    else
                    {
                        tw.WriteLine($"<li class=\"{((DataNode)current).AttributeType}Node\">");

                        if (current is LevelableDataNode levelableDataNode)
                        {
                            tw.WriteLine($"{current.DisplayText} Level {levelableDataNode.Level}");
                        }


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
                        if (((DataNode)current).Name == "Item")
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

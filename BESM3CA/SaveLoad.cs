using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace BESM3CA
{
    class SaveLoad
    {
        // Xml tag for node, e.g. 'node' in case of <node></node>

        private const string XmlNodeTag = "node";

        // Xml attributes for node e.g. <node text="Asia" tag="" 
        // imageindex="1"></node>

        private const string XmlNodeTextAtt = "text";
      //  private const string XmlNodeTagAtt = "tag";
      
        private void SetAttributeValue(TreeNode node,
                    string propertyName, string value)
        {
            if (propertyName == XmlNodeTextAtt)
            {
                node.Text = value;
            }
            
           /* else if (propertyName == XmlNodeTagAtt)
            {
                node.Tag = value;
            }*/
        }
        
        public void DeserializeTreeView(TreeView treeView, string fileName)
        {
            XmlTextReader reader = null;
            try
            {
                // disabling re-drawing of treeview till all nodes are added

                treeView.BeginUpdate();
                reader = new XmlTextReader(fileName);
                TreeNode parentNode = null;
                TreeNode newNode=null;
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.Name == XmlNodeTag)
                        {
                            newNode = new TreeNode();
                            bool isEmptyElement = reader.IsEmptyElement;

                            // loading node attributes

                            int attributeCount = reader.AttributeCount;
                            if (attributeCount > 0)
                            {
                                for (int i = 0; i < attributeCount; i++)
                                {
                                    reader.MoveToAttribute(i);
                                    SetAttributeValue(newNode,
                                                 reader.Name, reader.Value);
                                }
                            }
                            // add new node to Parent Node or TreeView

                            if (parentNode != null)
                                parentNode.Nodes.Add(newNode);
                            else
                                treeView.Nodes.Add(newNode);

                            
                            // making current node 'ParentNode' if its not empty

                            if (!isEmptyElement)
                            {
                                parentNode = newNode;
                            }
                        }
                        else
                        {
                            if (reader.Name == "BESM3CA.CharacterData" && newNode != null)
                            {
                                newNode.Tag = new CharacterData();
                                ((CharacterData)newNode.Tag).LoadXML(reader);
                            }
                            else if (reader.Name == "BESM3CA.AttributeData" && newNode != null)
                            {
                                newNode.Tag = new AttributeData();
                                ((AttributeData)newNode.Tag).LoadXML(reader);
                            }
                            else 
                            {
                            }

                        }
                        
                    }
                    // moving up to in TreeView if end tag is encountered

                    else if (reader.NodeType == XmlNodeType.EndElement)
                    {
                        if (reader.Name == XmlNodeTag)
                        {
                            parentNode = parentNode.Parent;
                        }
                    }
                    else if (reader.NodeType == XmlNodeType.XmlDeclaration)
                    {
                        //Ignore Xml Declaration                    

                    }
                    else if (reader.NodeType == XmlNodeType.None)
                    {
                        return;
                    }
                    else if (reader.NodeType == XmlNodeType.Text)
                    {
                        parentNode.Nodes.Add(reader.Value);
                    }

                }
            }
            finally
            {
                // enabling redrawing of treeview after all nodes are added

                treeView.EndUpdate();
                treeView.ExpandAll();
                reader.Close();
            }
        }

        public void SerializeTreeView(TreeView treeView, string fileName)
        {
            XmlTextWriter textWriter = new XmlTextWriter(fileName, System.Text.Encoding.ASCII);

            // writing the xml declaration tag
            textWriter.WriteStartDocument();
            //textWriter.WriteRaw("\r\n");
            
            // writing the main tag that encloses all node tags
            textWriter.WriteStartElement("TreeView");

            // save the nodes, recursive method
            SaveNodes(treeView.Nodes, textWriter);

            textWriter.WriteEndElement();
            textWriter.Close();
        }

        private void SaveNodes(TreeNodeCollection nodesCollection,  XmlTextWriter textWriter)
        {
            for (int i = 0; i < nodesCollection.Count; i++)
            {
                TreeNode node = nodesCollection[i];
                textWriter.WriteStartElement(XmlNodeTag);
                textWriter.WriteAttributeString(XmlNodeTextAtt,
                                                           node.Text);

                if (node.Tag != null)
                {
                    //textWriter.WriteAttributeString(XmlNodeTagAtt,
                    //                            node.Tag.ToString());

                    ((NodeData)node.Tag).SaveXML(textWriter);
                    // write data
                }


                if (node.Nodes.Count > 0)
                {
                    SaveNodes(node.Nodes, textWriter);
                }
                textWriter.WriteEndElement();
            }
        }
    }

    
}

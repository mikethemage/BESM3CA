using System.Xml;
using BESM3CA.Templates;
using System.Linq;

namespace BESM3CA.Model
{
    class AttributeData : NodeData
    {
        int _Level;
        int _Points;
        bool _HasLevel;
        int _Variant = 0;
        int _PointAdj = 0;

        public bool HasLevel
        {
            get
            {
                return _HasLevel;
            }
        }            

        public int Variant
        {
            get
            {
                return _Variant;
            }
            set
            {
                _Variant = value;
            }
        }

        public bool raiseLevel()
        {

            //need to check maxlevel
            if (_HasLevel == true)
            {
                _Level++;
                _pointsUpToDate = false;
            }
            return _HasLevel;
        }

        public bool lowerLevel()
        {
            if (_Level > 0)
            {
                if (_HasLevel == true)
                {
                    if (_PointAdj < 0)
                    {
                        if ((_Level * _Points) + _PointAdj > 0)
                        {
                            _Level--;
                            _pointsUpToDate = false;
                        }
                    }
                    else
                    {
                        _Level--;
                        _pointsUpToDate = false;
                    }
                }
                return _HasLevel;
            }
            else
            {
                return false;
            }
        }

        public int AttributeID
        {
            get
            {
                return ID;
            }
        }

        public int Level
        {
            get
            {
                return _Level;
            }
        }

        public int PointsPerLevel
        {
            get
            {
                return _Points;
            }
            set
            {
                _Points = value;
            }
        }

        public int PointAdj
        {
            get
            {
                return _PointAdj;
            }
            set
            {
                _PointAdj = value;
            }
        }

        public int BaseCost
        {
            get
            {
                return (PointsPerLevel * Level) + _PointAdj;
            }
        }

        public override int GetPoints(TemplateData templateData)
        {
            if (_pointsUpToDate == false || _FirstChild == null)
            {
                
                bool isItem = false;
                bool isCompanion = false;
                bool isAlternateAttack = false;
                bool isAlternateForm = false;

                isItem = (Name == "Item");
                isCompanion = (Name == "Companion");
                isAlternateForm = (Name == "Alternate Form");
                if (Variant > 0)
                {
                    VariantListing SelectedVariant = templateData.VariantList.Where(n => n.ID == Variant).First();

                    if (SelectedVariant.Name == "Alternate Attack")
                    {
                        isAlternateAttack = true;
                    }
                }
                _points = 0;

                //NodeData temp = _FirstChild;
                //while(temp != null)
                //{
                //    _points += temp.GetPoints();
                //    temp = temp.Next;
                //}

                _points += BaseCost;
                _pointsUpToDate = true;
            }

           
            return _points;
        }


        /*
         * int basepoints = 0;
            int level = 1;
            
            int PointAdj = 0;
                    
                basepoints = ((AttributeData)Node.Tag).PointsPerLevel;
                level = ((AttributeData)Node.Tag).Level;
                

                AttributeListing SelectedAttribute = templateData.AttributeList.Where(n => n.ID == ((AttributeData)Node.Tag).ID).First();

                if (SelectedAttribute.Type == "Special")
                {
                    basepoints = 0;
                    level = 0;
                }
                PointAdj = ((AttributeData)Node.Tag).PointAdj;
            }
            

            int Extra = 0;
            int itempoints = 0;
            foreach (TreeNode Child in Node.Nodes)
            {
                if (isItem == false && isCompanion == false && isAlternateForm == false)
                {
                    Extra += GetPoints(Child);
                }
                else if (isCompanion == true && Child.Tag.GetType() == typeof(AttributeData))
                {
                    AttributeListing SelectedAttribute = templateData.AttributeList.Where(n => n.ID == ((AttributeData)Child.Tag).ID).First();

                    if (SelectedAttribute.Type == "Restriction")
                    {
                        Extra += GetPoints(Child);
                    }
                }
                else if (isItem == true)
                {
                    itempoints += GetPoints(Child);
                }
                else if (isCompanion == true)
                {
                    if (Child.Tag.GetType() == typeof(CharacterData))
                    {
                        int temp = GetPoints(Child);
                        if (temp > 120)
                        {
                            basepoints += 2 + ((GetPoints(Child) - 120) / 10);
                        }
                        else
                        {
                            basepoints += 2;
                        }
                    }
                    else
                    {
                        Extra += GetPoints(Child);
                    }
                }
            }
            if (isItem)
            {
                if (itempoints < 1)
                {
                    basepoints = 1;
                }
                else
                {
                    basepoints += itempoints / 2;
                }
            }
            
            //***
            //if alternate weapon attack half points
            if (isAlternateAttack)
            {
                return ((basepoints * level) + Extra + 1) / 2;
            }
            //***

            return (basepoints * level) + Extra + PointAdj;



         * 
         * 
         * 
         * 
         * 
          foreach (TreeNode Node in Nodes)
            {
                refreshTree(Node.Nodes);

                if (Node.Tag.GetType() == typeof(AttributeData))
                {
                    bool altform = false;
                    if (((AttributeData)Node.Tag).Name == "Alternate Form")
                    {
                        altform = true;
                    }

                    AttributeListing SelectedAttribute = templateData.AttributeList.Where(n => n.ID == ((AttributeData)Node.Tag).ID).First();

                    if (SelectedAttribute.SpecialContainer || altform)
                    {
                        int LevelsUsed = 0;
                        foreach (TreeNode child in Node.Nodes)
                        {
                            SelectedAttribute = templateData.AttributeList.Where(n => n.ID == ((AttributeData)child.Tag).ID).First();

                            if (SelectedAttribute.Type == "Special" || altform)
                            {
                                LevelsUsed += ((AttributeData)child.Tag).Level * ((AttributeData)child.Tag).PointsPerLevel;
                            }
                        }
                        int specialpoints;
                        if (altform)
                        {
                            specialpoints = ((AttributeData)Node.Tag).Level * 10;
                        }
                        else
                        {
                            specialpoints = ((AttributeData)Node.Tag).Level;
                        }
                        Node.Text = ((AttributeData)Node.Tag).Name + " (" + (specialpoints - LevelsUsed).ToString() + " Left)" + " (" + GetPoints(Node) + " Points)";
                    }
                    else
                    {
                        SelectedAttribute = templateData.AttributeList.Where(n => n.ID == ((AttributeData)Node.Tag).ID).First();

                        if (SelectedAttribute.Type == "Special")
                        {
                            Node.Text = ((AttributeData)Node.Tag).Name;

                        }
                        else
                        {
                            Node.Text = ((AttributeData)Node.Tag).Name + " (" + GetPoints(Node) + " Points)";
                        }
                    }
                }
                else
                {
                    Node.Text = ((CharacterData)Node.Tag).Name + " (" + GetPoints(Node) + " Points)";
                }                
            }
        }
         */

        public AttributeData(string AttributeName, int AttributeID, string Notes, int Level, int Points) : base(AttributeName, AttributeID, Notes)
        {
            _Level = Level;
            _Points = Points;
            _HasLevel = true;
        }

        public AttributeData(string AttributeName, int AttributeID, string Notes, int Level, int Points, int PointAdj)
            : base(AttributeName, AttributeID, Notes)
        {
            _PointAdj = PointAdj;
            _Level = Level;
            _Points = Points;
            _HasLevel = true;
        }

        public AttributeData(string AttributeName, int AttributeID, string Notes, int Points) : base(AttributeName, AttributeID, Notes)
        {
            if (AttributeName == "Item")
            {
                _HasLevel = false;
                _Level = 1;
            }
            else
            {
                _HasLevel = true;
                if(AttributeName=="Weapon")
                {
                    _Level = 0;
                }
                else
                {
                    _Level = 1;
                }
            }

            _Points = Points;

            if (AttributeName == "Companion")
            {
                addChild(new CharacterData(""));
            }
            if (AttributeName == "Mind Control")
            {
                addChild(new AttributeData("Range", 167, "", 3, 1, -3));
            }
        }

        public override void SaveAdditionalXML(XmlTextWriter textWriter)
        {
            textWriter.WriteStartElement("AttributeStats");
            textWriter.WriteAttributeString("Level", _Level.ToString());
            textWriter.WriteAttributeString("Variant", _Variant.ToString());
            textWriter.WriteAttributeString("HasLevel", _HasLevel.ToString());
            textWriter.WriteAttributeString("Points", _Points.ToString());
            textWriter.WriteAttributeString("PointAdj", _PointAdj.ToString());
            textWriter.WriteEndElement();
        }

        public AttributeData() : base()
        {

        }

        public override void LoadAdditionalXML(XmlTextReader reader)
        {
            while (reader.NodeType != XmlNodeType.None)
            {
                reader.Read();

                if (reader.NodeType == XmlNodeType.Element)
                {

                    if (reader.Name == "AttributeStats")
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
                                    case "HasLevel":
                                        _HasLevel = bool.Parse(reader.Value);
                                        break;
                                    case "Level":
                                        _Level = int.Parse(reader.Value);
                                        break;
                                    case "Variant":
                                        _Variant = int.Parse(reader.Value);
                                        break;
                                    case "Points":
                                        _Points = int.Parse(reader.Value);
                                        break;
                                    case "PointAdj":
                                        _PointAdj = int.Parse(reader.Value);
                                        break;
                                    default:
                                        break;
                                }
                            }
                            break;
                        }
                    }
                }
                else if (reader.NodeType == XmlNodeType.EndElement)
                {
                    if (reader.Name == "AttributeStats")
                    {
                        break;
                    }
                }
            }

        }
    }

}

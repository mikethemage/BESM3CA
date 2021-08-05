using System.Xml;

namespace BESM3CA
{
    class AttributeData : NodeData
    {
        int _Level;
        int _Points;
        bool _HasLevel;
        int _Variant = 0;
        int _PointAdj = 0;

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
                        }
                    }
                    else
                    {
                        _Level--;
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
                _Level = 0;
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

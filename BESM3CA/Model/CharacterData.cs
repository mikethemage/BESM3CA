using System.Xml;
using BESM3CA.Templates;

namespace BESM3CA.Model
{
    class CharacterData : NodeData
    {
        //Internal Variables:
        int _body;
        int _mind;
        int _soul;

        //Properties:
        public string CharacterName {get; set;}
     
        public int BaseCost
        {
            get
            {
                return (_body + _mind + _soul) * 10;
            }
        }

        public int basehealth
        {
            get
            {
                return (_body + _soul) * 5;
            }
        }

        public int baseenergy
        {
            get
            {
                return (_mind + _soul) * 5;
            }
        }

        public int baseCV
        {
            get
            {
                return (_body + _mind + _soul) / 3;
            }
        }

        public int Body
        {
            get
            {
                return _body;
            }
            set
            {
                if (value >= 0)
                {
                    _body = value;
                    PointsUpToDate = false;
                }
            }
        }

        public int Mind
        {
            get
            {
                return _mind;
            }
            set
            {
                if (value >= 0)
                {
                    _mind = value;
                    PointsUpToDate = false;
                }
            }
        }

        public int Soul
        {
            get
            {
                return _soul;
            }
            set
            {
                if (value >= 0)
                {
                    _soul = value;
                    PointsUpToDate = false;
                }
            }
        }


        //Constructors:
        public CharacterData(string Notes) : base("Character", 0, Notes)
        {
            _body = 1;
            _mind = 1;
            _soul = 1;
        }

        public CharacterData() : base()
        {
            //Default Constructor
        }


        //Member functions:
        public override int GetPoints(TemplateData templateData)
        {            
                if (PointsUpToDate == false || _FirstChild == null)
                {
                    _points = BaseCost;
                    NodeData temp = _FirstChild;
                    while(temp != null)
                    {
                        _points += temp.GetPoints(templateData);
                        temp = temp.Next;
                    }
                    PointsUpToDate = true;
                }
                
                return _points;            
        }

        public override void SaveAdditionalXML(XmlTextWriter textWriter)
        {
            textWriter.WriteStartElement("CharacterStats");
            textWriter.WriteAttributeString("Mind", _mind.ToString());
            textWriter.WriteAttributeString("Body", _body.ToString());
            textWriter.WriteAttributeString("Soul", _soul.ToString());
            textWriter.WriteEndElement();
        }

        public override void LoadAdditionalXML(XmlTextReader reader)
        {
            while (reader.NodeType != XmlNodeType.None)
            {
                reader.Read();

                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.Name == "CharacterStats")
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
                                    case "Mind":
                                        _mind = int.Parse(reader.Value);
                                        break;
                                    case "Body":
                                        _body = int.Parse(reader.Value);
                                        break;
                                    case "Soul":
                                        _soul = int.Parse(reader.Value);
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
                    if (reader.Name == "CharacterStats")
                    {
                        break;
                    }
                }
            }
        }
    }
}
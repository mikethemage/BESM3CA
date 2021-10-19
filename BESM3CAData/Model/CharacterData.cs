using BESM3CAData.Templates;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace BESM3CAData.Model
{
    public class CharacterData : NodeData
    {
        //Fields:
        private int _body;
        private int _mind;
        private int _soul;

        //Properties:
        public string CharacterName { get; set; }

        public override bool HasCharacterStats
        {
            get
            {
                return true;
            }
        }

        public override bool HasLevelStats
        {
            get
            {
                return false;
            }
        }

        public override bool HasPointsStats
        {
            get
            {
                return false;
            }
        }

        public override List<AttributeListing> PotentialChildren
        {
            get
            {
                if (AssociatedController.SelectedTemplate != null)
                {
                    return AssociatedController.SelectedTemplate.AttributeList.Where(n => (n.Type == "Attribute" || n.Type == "Defect" || n.Type == "Skill") && n.Name != "Character").ToList<AttributeListing>();
                }
                else
                {
                    return null;
                }
            }
        }        

        public int BaseCost
        {
            get
            {
                return (_body + _mind + _soul) * 10;
            }
        }

        public int BaseHealth
        {
            get
            {
                return (_body + _soul) * 5;
            }
        }

        public int BaseEnergy
        {
            get
            {
                return (_mind + _soul) * 5;
            }
        }

        public int BaseCV
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


        //Constructor:
        public CharacterData(Controller controller, string Notes = "") : base("Character", 0, Notes, controller)
        {
            _body = 1;
            _mind = 1;
            _soul = 1;
        }


        //Methods:
        public override int GetPoints()
        {
            if (PointsUpToDate == false || FirstChild == null)
            {
                _points = BaseCost;
                NodeData temp = FirstChild;
                while (temp != null)
                {
                    _points += temp.GetPoints();
                    temp = temp.Next;
                }
                PointsUpToDate = true;
            }

            return _points;
        }

        //Stat calculation:
        public override CalcStats GetStats()
        {
            CalcStats stats;

            stats = new CalcStats(BaseHealth,
                    BaseEnergy,
                    BaseCV,
                    BaseCV);

            NodeData child = FirstChild;
            while (child != null)
            {
                CalcStats temp = child.GetStats();
                stats += temp;
                child = child.Next;
            }

            return stats;
        }

        //XML:
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
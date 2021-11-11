using BESM3CAData.Control;
using BESM3CAData.Listings;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace BESM3CAData.Model
{
    public class CharacterNode : BaseNode
    {
        //Fields:
        private int _body;
        private int _mind;
        private int _soul;


        //Properties:
        public string CharacterName { get; set; }               
               

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


        //Constructors:  
        public CharacterNode(DataController controller, string notes = "") : base( controller,notes)
        {
            //Default constructor for data loading only
        }

        public CharacterNode(CharacterDataListing attribute, string notes, DataController controller) : base(attribute,  controller,notes)
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
                BaseNode temp = FirstChild;
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

            BaseNode child = FirstChild;
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
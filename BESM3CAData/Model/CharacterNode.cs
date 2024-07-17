using BESM3CAData.Control;
using BESM3CAData.Listings;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml;

namespace BESM3CAData.Model
{
    public class CharacterNode : BaseNode
    {
        public override void RefreshAll()
        {
            foreach (BaseNode item in Children)
            {
                item.RefreshAll();
            }
            RefreshBaseCost();
            RefreshPoints();
            RefreshDisplayText();
        }

        private int _body;
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
                    if (value != _body)
                    {
                        _body = value;
                        OnPropertyChanged(nameof(Body));
                        RefreshBaseCost();
                        
                    }

                }
            }
        }

        private int _mind;
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
                    if (value != _mind)
                    {
                        _mind = value;
                        OnPropertyChanged(nameof(Mind));
                        RefreshBaseCost();
                        
                    }
                }
            }
        }

        private int _soul;
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
                    if (value != _soul)
                    {
                        _soul = value;
                        OnPropertyChanged(nameof(Soul));
                        RefreshBaseCost();
                        
                    }
                }
            }
        }

        private void RefreshBaseCost()
        {
            BaseCost = (Body + Mind + Soul) * 10;
        }

        public override bool CanDelete()
        {
            return false;
        }

        protected override void RefreshPoints()
        {
            int tempPoints = BaseCost;
            BaseNode temp = FirstChild;
            while (temp != null)
            {
                tempPoints += temp.Points;
                temp = temp.Next;
            }
            Points = tempPoints;
        }




        //Properties:
        public string CharacterName { get; set; }

        public override void ChildPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.ChildPropertyChanged(sender, e);
            if (sender is BaseNode)
            {
                if (e.PropertyName == nameof(BaseNode.Points))
                {
                    RefreshPoints();
                }
            }
        }



        public int BaseHealth
        {
            get
            {
                return (Body + Soul) * 5;
            }
        }

        public int BaseEnergy
        {
            get
            {
                return (Mind + Soul) * 5;
            }
        }

        public int BaseCV
        {
            get
            {
                return (Body + Mind + Soul) / 3;
            }
        }




        //Constructors:  
        public CharacterNode(RPGEntity controller, string notes = "") : base(controller, notes)
        {
            //Default constructor for data loading only
        }

        public CharacterNode(CharacterDataListing attribute, string notes, RPGEntity controller, bool isFreebie) : base(attribute, controller, notes, isFreebie)
        {
            Body = 1;
            Mind = 1;
            Soul = 1;
        }


        //Methods:



        public CalcStats Stats
        {
            get { return GetStats(); }
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
        //public override void SaveAdditionalXML(XmlTextWriter textWriter)
        //{
        //    textWriter.WriteStartElement("CharacterStats");
        //    textWriter.WriteAttributeString("Mind", Mind.ToString());
        //    textWriter.WriteAttributeString("Body", Body.ToString());
        //    textWriter.WriteAttributeString("Soul", Soul.ToString());
        //    textWriter.WriteEndElement();
        //}

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
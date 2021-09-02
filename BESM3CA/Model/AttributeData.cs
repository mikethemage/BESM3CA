using System.Xml;
using BESM3CA.Templates;
using System.Linq;

namespace BESM3CA.Model
{
    class AttributeData : NodeData
    {
        //Internal Variables:
        int _Level;
        int _PointsPerLevel;
        bool _HasLevel;
        int _Variant = 0;
        int _PointAdj = 0;


        //Properties:
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
                return _PointsPerLevel;
            }
            set
            {
                _PointsPerLevel = value;
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


        //Constructors:
        public AttributeData(string AttributeName, int AttributeID, string Notes, int Level, int Points) : base(AttributeName, AttributeID, Notes)
        {
            _Level = Level;
            _PointsPerLevel = Points;
            _HasLevel = true;
        }

        public AttributeData(string AttributeName, int AttributeID, string Notes, int Level, int Points, int PointAdj)
            : base(AttributeName, AttributeID, Notes)
        {
            _PointAdj = PointAdj;
            _Level = Level;
            _PointsPerLevel = Points;
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
                if (AttributeName == "Weapon")
                {
                    _Level = 0;
                }
                else
                {
                    _Level = 1;
                }
            }

            _PointsPerLevel = Points;

            if (AttributeName == "Companion")
            {
                addChild(new CharacterData(""));
            }
            if (AttributeName == "Mind Control")
            {
                addChild(new AttributeData("Range", 167, "", 3, 1, -3));
            }
        }

        public AttributeData() : base()
        {

        }


        //Member Functions:
        public bool raiseLevel()
        {
            //need to check maxlevel
            if (_HasLevel == true)
            {
                _Level++;
                PointsUpToDate = false;
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
                        if ((_Level * _PointsPerLevel) + _PointAdj > 0)
                        {
                            _Level--;
                            PointsUpToDate = false;
                        }
                    }
                    else
                    {
                        _Level--;
                        PointsUpToDate = false;
                    }
                }
                return _HasLevel;
            }
            else
            {
                return false;
            }
        }        

        public override int GetPoints(TemplateData templateData)
        {
            if (PointsUpToDate == false || _FirstChild == null)
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

                int VariablesOrRestrictions = 0;
                int ChildPoints = 0;

                NodeData temp = _FirstChild;
                while(temp != null)
                {
                    if (temp.GetType() == typeof(AttributeData))
                    {
                        AttributeListing SelectedAttribute = templateData.AttributeList.Where(n => n.ID == ((AttributeData)temp).ID).First();
                        if (SelectedAttribute.Type == "Restriction" || SelectedAttribute.Type == "Variable")
                        {
                            VariablesOrRestrictions += temp.GetPoints(templateData);
                        }
                        else
                        {
                            ChildPoints += temp.GetPoints(templateData);
                        }

                    }
                    else 
                    {
                        ChildPoints += temp.GetPoints(templateData);
                    }
                    
                    temp = temp.Next;
                }

                _points = BaseCost;
               
                _points += VariablesOrRestrictions;

                if (isCompanion)
                {
                    if (ChildPoints > 120)
                    {
                        _points += 2 + ((ChildPoints - 120) / 10);
                    }
                    else
                    {
                        _points += 2;
                    }
                }

                if (isItem)
                {
                    //item point cost calc:
                    if (ChildPoints < 1)
                    {
                        _points += 1;
                    }
                    else
                    {
                        _points += (ChildPoints / 2);
                    }
                } 
                
                //***
                //if alternate weapon attack half points
                if (isAlternateAttack)
                {
                    _points /= 2;
                }
                //***
                //Points should equal BaseCost +- any restrictions or variables

                PointsUpToDate = true;
            }

           
            return _points;
        }

        public override void SaveAdditionalXML(XmlTextWriter textWriter)
        {
            textWriter.WriteStartElement("AttributeStats");
            textWriter.WriteAttributeString("Level", _Level.ToString());
            textWriter.WriteAttributeString("Variant", _Variant.ToString());
            textWriter.WriteAttributeString("HasLevel", _HasLevel.ToString());
            textWriter.WriteAttributeString("Points", _PointsPerLevel.ToString());
            textWriter.WriteAttributeString("PointAdj", _PointAdj.ToString());
            textWriter.WriteEndElement();
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
                                        _PointsPerLevel = int.Parse(reader.Value);
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

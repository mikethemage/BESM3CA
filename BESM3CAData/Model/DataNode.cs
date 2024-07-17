using BESM3CAData.Control;
using BESM3CAData.Listings;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;

namespace BESM3CAData.Model
{
    public abstract class DataNode : BaseNode
    {
        //Properties:


        protected virtual string BaseDescription
        {
            get
            {
                string result = AssociatedListing.Description;
                return result;
            }
        }


        public void RefreshDescription()
        {
            //Need to process attribute description to calculate numeric components
            string description = BaseDescription;

            string completedDescription = "";

            while (description != null)
            {
                string[] pieces = description.Split('[', 2);
                completedDescription += pieces[0];
                if (pieces.Length > 1)
                {
                    description = pieces[1];
                    pieces = description.Split(']', 2);

                    completedDescription += ProcessDescriptionValue(pieces[0]);

                    if (pieces.Length > 1)
                    {
                        description = pieces[1];
                    }
                    else
                    {
                        description = null;
                    }
                }
                else
                {
                    description = null;
                }
            }

            AttributeDescription = completedDescription;
        }


        protected string _attributeDescription;

        public string AttributeDescription
        {
            get
            {
                return _attributeDescription;
            }
            set
            {
                if (value != _attributeDescription)
                {
                    _attributeDescription = value;
                    OnPropertyChanged(nameof(AttributeDescription));
                }
            }
        }

        public string AttributeType
        {
            get
            {
                return AssociatedListing.Type;
            }
        }


        //Constructors:   
        public DataNode(RPGEntity controller, string notes = "") : base(controller, notes)
        {
            //Default constructor for data loading only
        }

        public DataNode(DataListing attribute, string notes, RPGEntity controller, bool isFreebie) : base(attribute, controller, notes, isFreebie)
        {
            //Pass parameters to base constructor
            RefreshDescription();
        }


        //Methods:
        protected virtual string ProcessDescriptionValue(string valueToParse)
        {
            return valueToParse;
        }


        public override CalcStats GetStats()
        {
            CalcStats stats;

            stats = new CalcStats(0, 0, 0, 0);

            if (stats.ACV > 0 || stats.DCV > 0 || stats.Energy > 0 || stats.Health > 0)
            {
                BaseNode child = FirstChild;
                while (child != null)
                {
                    if (child is DataNode childAttribute && childAttribute.AttributeType == "Restriction")
                    {
                        stats = new CalcStats(0, 0, 0, 0);
                        break;
                    }
                    child = child.Next;
                }
            }

            return stats;
        }


        //XML:     
        public override void LoadAdditionalXML(XmlTextReader reader)
        {
            while (reader.NodeType != XmlNodeType.None)
            {
                reader.Read();

                
                if (reader.NodeType == XmlNodeType.EndElement)
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

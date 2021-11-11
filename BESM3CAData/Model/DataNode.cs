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
        public override string DisplayText
        {
            get
            {
                if (AssociatedListing != null)
                {
                    if (AttributeType == "Special")
                    {
                        return Name;
                    }
                    else
                    {
                        return $"{Name} ({GetPoints()} Points)";
                    }
                }
                else
                {
                    return "";
                }
            }
        }

        protected virtual string BaseDescription
        {
            get
            {
                string result = AssociatedListing.Description;
                return result;
            }
        }

        public string AttributeDescription
        {
            get
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

                return completedDescription;
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
        public DataNode(DataController controller, string notes = "") : base(controller,notes )
        {
            //Default constructor for data loading only
        }

        public DataNode(DataListing attribute, string notes, DataController controller) : base(attribute, controller,notes)
        {
           //Pass parameters to base constructor
           
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

                //if (reader.NodeType == XmlNodeType.Element)
                //{
                    //if (reader.Name == "AttributeStats")
                    //{
                        //// loading node attributes
                        //int attributeCount = reader.AttributeCount;
                        //if (attributeCount > 0)
                        //{
                        //    for (int i = 0; i < attributeCount; i++)
                        //    {
                        //        reader.MoveToAttribute(i);
                        //        switch (reader.Name)
                        //        {
                        //            default:
                        //                //Todo: remove method
                        //                break;
                        //        }
                        //    }
                        //    break;
                        //}
                    //}
                //}
                //else
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

﻿using BESM3CAData.Control;
using BESM3CAData.Listings;
using System.Xml;

namespace BESM3CAData.Model
{
    public class PointsContainerDataNode : DataNode, IPointsDataNode
    {
        //Properties:
        public int PointAdj
        {
            get
            {
                return 0;
            }
        }

        //Constructors:
        public PointsContainerDataNode(DataController controller, string Notes = "") : base(controller, Notes)
        {
            //Default constructor for data loading only
        }

        public PointsContainerDataNode(PointsContainerDataListing attribute, string notes, DataController controller) : base(attribute, notes, controller)
        {
            
        }


        //Methods:
        public override int GetPoints()
        {
            if (PointsUpToDate == false || FirstChild == null)
            {
                int VariablesOrRestrictions = 0;
                int ChildPoints = 0;

                BaseNode temp = FirstChild;
                while (temp != null)
                {
                    if (temp is DataNode tempAttribute)
                    {
                        if (tempAttribute.AttributeType == "Restriction" || tempAttribute.AttributeType == "Variable")
                        {
                            VariablesOrRestrictions += temp.GetPoints();
                        }
                        else
                        {
                            ChildPoints += temp.GetPoints();
                        }
                    }
                    else
                    {
                        ChildPoints += temp.GetPoints();
                    }

                    temp = temp.Next;
                }

                _points = VariablesOrRestrictions;

                //container point cost calc:
                if (((PointsContainerDataListing)AssociatedListing).PointScale == 0 || ChildPoints < ((PointsContainerDataListing)AssociatedListing).PointScale)
                {
                    //PointScale should be set for containers - if not set points to 0 to avoid divide by 0 errors:
                    _points += 0;
                }
                else
                {
                    //Scale points e.g. 3E items valued at 1/2 total child points:
                    _points += ChildPoints / ((PointsContainerDataListing)AssociatedListing).PointScale;
                }

                PointsUpToDate = true;
            }

            return _points;
        }

        public override void SaveAdditionalXML(XmlTextWriter textWriter)
        {
            //Todo: remove method
        }
    }
}

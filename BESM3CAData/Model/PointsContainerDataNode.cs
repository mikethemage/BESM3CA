using BESM3CAData.Control;
using BESM3CAData.Listings;

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

        public PointsContainerDataNode(PointsContainerDataListing attribute, string notes, DataController controller, int level = 1, int pointAdj = 0) : base(attribute, notes, controller, level, pointAdj)
        {
            _dataListing = attribute;
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
                if (((PointsContainerDataListing)_dataListing).PointScale == 0 || ChildPoints < ((PointsContainerDataListing)_dataListing).PointScale)
                {
                    _points += 0;
                }
                else
                {
                    _points += ChildPoints / ((PointsContainerDataListing)_dataListing).PointScale;
                }

                PointsUpToDate = true;
            }

            return _points;
        }
    }
}

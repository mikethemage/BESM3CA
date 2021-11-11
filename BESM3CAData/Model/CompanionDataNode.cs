using BESM3CAData.Control;
using BESM3CAData.Listings;

namespace BESM3CAData.Model
{
    public class CompanionDataNode : LevelableDataNode
    {
        //Constructor:
        public CompanionDataNode(CompanionDataListing attribute, string notes, DataController controller, int level = 1, int pointAdj = 0) : base(attribute, notes, controller, level, pointAdj)
        {
            AddChild(attribute.SubAttribute.CreateNode("", AssociatedController, attribute.SubAttributeLevel, attribute.SubAttributePointsAdj));
        }

        public CompanionDataNode(DataController controller, string Notes = "") : base(controller, Notes)
        {
            //Default constructor for data loading only
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

                //Points should equal BaseCost +- any restrictions or variables
                _points = BaseCost;
                _points += VariablesOrRestrictions;

                //Points calc for companions:
                if (ChildPoints > 120)
                {
                    _points += (2 + ((ChildPoints - 120) / 10)) * Level;
                }
                else
                {
                    _points += 2 * Level;
                }


                PointsUpToDate = true;
            }

            return _points;
        }
    }
}
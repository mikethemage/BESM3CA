using BESM3CAData.Control;
using BESM3CAData.Listings;

namespace BESM3CAData.Model
{
    public class SpecialContainerDataNode : LevelableDataNode
    {
        //Fields:
        private int _specialPointsUsed = 0;


        //Properties:
        public override string DisplayText
        {
            get
            {
                if (_dataListing != null)
                {
                    if (_dataListing is SpecialContainerWithVariantDataListing attributeListing && (attributeListing.SpecialContainer))
                    {
                        return $"{Name} ({GetSpecialPoints()} Left) ({GetPoints()} Points)";
                    }
                    else
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
                }
                else
                {
                    return "";
                }
            }
        }


        //Constructors:
        public SpecialContainerDataNode(DataController controller, string Notes = "") : base(controller, Notes)
        {

        }

        public SpecialContainerDataNode(SpecialContainerDataListing attribute, string notes, DataController controller, int level = 1, int pointAdj = 0) : base(attribute, notes, controller, level, pointAdj)
        {

        }


        //Methods:
        public int GetSpecialPoints()
        {
            int specialpoints = 0;

            if (_dataListing is SpecialContainerWithVariantDataListing attributeListing)
            {
                if (attributeListing.SpecialContainer)//|| altform)
                {
                    if (PointsUpToDate == false)
                    {
                        GetPoints();
                    }

                    specialpoints = Level * attributeListing.SpecialPointsPerLevel;

                    specialpoints -= _specialPointsUsed;
                }
            }

            return specialpoints;
        }

        public override int GetPoints()
        {
            if (PointsUpToDate == false || FirstChild == null)
            {
                bool isCompanion = Name == "Companion";
                bool isAlternateAttack = false;

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

                //Update special points used counter while recalculating points:
                _specialPointsUsed = ChildPoints;

                if (isCompanion)
                {
                    if (ChildPoints > 120)
                    {
                        _points += (2 + ((ChildPoints - 120) / 10)) * Level;
                    }
                    else
                    {
                        _points += 2 * Level;
                    }
                }

                //if alternate weapon attack half points:
                if (isAlternateAttack)
                {
                    _points /= 2;
                }

                PointsUpToDate = true;
            }

            return _points;
        }
    }
}

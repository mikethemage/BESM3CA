using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BESM3CAData.Listings;
using System.Xml;
using BESM3CAData.Control;

namespace BESM3CAData.Model
{
    public class SpecialContainerDataNode : LevelableDataNode
    {
        private int _specialPointsUsed = 0;

        public SpecialContainerDataNode(DataController controller, string Notes = "") : base(controller, Notes)
        {

        }

        public SpecialContainerDataNode(SpecialContainerDataListing attribute, string notes, DataController controller, int level = 1, int pointAdj = 0) : base(attribute, notes, controller, level, pointAdj)
        {

     
        }


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
                bool isItem = Name == "Item";
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

                if (isItem)
                {
                    //item point cost calc:
                    if (ChildPoints < 2)
                    {
                        _points += 0;
                    }
                    else
                    {
                        _points += ChildPoints / 2;
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

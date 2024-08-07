using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Triarch.BusinessLogic.Models.Definitions;

namespace Triarch.BusinessLogic.Models.Entities;
public class PointsContainer : RPGElement
{

    public override int Points
    {
        get 
        {
            int tempPoints = VariablesOrRestrictions;

            if(AssociatedDefinition is PointsContainerDefinition pointsContainerDefinition)
            {
                if (pointsContainerDefinition.PointsContainerScale != 0)
                {
                    if (ChildPoints > 0 && ChildPoints < pointsContainerDefinition.PointsContainerScale)
                    {
                        //If child points positive but less that pointscale we should still charge 1 point for the container:
                        tempPoints += 1;
                    }
                    else
                    {
                        //Scale points e.g. 3E items valued at 1/2 total child points:
                        tempPoints += ChildPoints / pointsContainerDefinition.PointsContainerScale;
                    }
                }
            }

            return tempPoints;
        }
    }
}

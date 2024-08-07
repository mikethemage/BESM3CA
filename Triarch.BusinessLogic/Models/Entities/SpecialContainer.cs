using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Triarch.BusinessLogic.Models.Definitions;

namespace Triarch.BusinessLogic.Models.Entities;
public class SpecialContainer : Levelable
{
    public int SpecialPointsTotal
    {
        get
        {
            if(AssociatedDefinition is SpecialContainerDefinition specialContainerDefinition)
            {
                return Level * specialContainerDefinition.SpecialPointsPerLevel;
            }
            return 0;
        }
    }

    public int SpecialPointsUsed
    {
        get
        {
            return ChildPoints;
        }
    }

    

    public int SpecialPointsRemaining
    {
        get
        {
            return SpecialPointsTotal - SpecialPointsUsed;
        }
    }
    
}

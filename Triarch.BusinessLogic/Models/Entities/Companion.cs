using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Triarch.BusinessLogic.Models.Entities;
public class Companion : Levelable
{
    
    public override int BaseCost
    {
        get
        {
            int childPoints = ChildPoints;
            //Points calc for companions:
            if (childPoints > 120)
            {
                return (2 + ((childPoints - 120) / 10)) * Level;
            }
            else
            {
                return 2 * Level;
            }
        }
    }

    public override int Points
    {
        get
        {
            return BaseCost + VariablesOrRestrictions;
        }
    }
}

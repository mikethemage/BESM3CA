using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Triarch.BusinessLogic.Models.Definitions;

namespace Triarch.BusinessLogic.Models.Entities;
public class MultiGenre : Levelable
{
    public override int PointsPerLevel
    {
        get
        {
            if (AssociatedDefinition is MultiGenreDefinition multiGenreDefinition)
            {
                if (multiGenreDefinition.Variants != null && Variant != null)
                {
                    return Variant.CostPerLevel;
                }
                else
                {
                    GenreCostPerLevel? genreCost = multiGenreDefinition.MultiGenreCostPerLevels.Where(x => x.Genre == Entity.Genre).FirstOrDefault();
                    if(genreCost != null)
                    {
                        return genreCost.CostPerLevel;
                    }
                    return multiGenreDefinition.CostPerLevel ?? 0;
                }
            }
            else
            {
                return 0;
            }
        }
    }
}

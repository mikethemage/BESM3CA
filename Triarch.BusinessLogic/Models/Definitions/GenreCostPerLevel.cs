using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Triarch.BusinessLogic.Models.Definitions;

public class GenreCostPerLevel
{
    public Genre Genre { get; set; } = null!;

    public int CostPerLevel { get; set; }
}

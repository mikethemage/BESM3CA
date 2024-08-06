using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Triarch.BusinessLogic.Models.Definitions;

public class Freebie
{
    public RPGElementDefinition SubAttribute { get; set; } = null!; 
    public int FreeLevels { get; set; } = 0;
    public int RequiredLevels { get; set; } = 0;
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Triarch.BusinessLogic.Models.Definitions;
public class VariantDefinition
{
    public string VariantName { get; set; } = null!;

    public int CostPerLevel { get; set; }
    
    public string Description { get; set; } = string.Empty;
    
    public bool IsDefault { get; set; } = false;
}

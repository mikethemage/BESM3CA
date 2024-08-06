using System.ComponentModel.DataAnnotations;

namespace Triarch.Dtos.Definitions;

public class CoreRulesetDto
{      
    [Required]
    [MaxLength(60)]
    public string CoreRulesetName { get; set; } = null!;    
}

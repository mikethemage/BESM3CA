using System.ComponentModel.DataAnnotations;

namespace Triarch.Dtos.Definitions;

public class CoreRulesetDto
{    
    public int Id { get; set; }

    [Required]
    [MaxLength(60)]
    public string CoreRulesetName { get; set; } = null!;    
}

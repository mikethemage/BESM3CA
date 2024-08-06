using System.ComponentModel.DataAnnotations;

namespace Triarch.Dtos.Definitions;

public class RPGSystemHeadingDto
{
    [Required]
    public string SystemName { get; set; } = null!;

    public string? CoreRulesetName { get; set; } = null;
}
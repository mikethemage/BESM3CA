using System.ComponentModel.DataAnnotations;

namespace Triarch.Dtos.Definitions;

public class GenreCostPerLevelDto
{
    [Required]
    public string GenreName { get; set; } = null!;

    public int CostPerLevel { get; set; }
}
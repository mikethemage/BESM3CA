using System.ComponentModel.DataAnnotations;

namespace Triarch.Dtos.Definitions;

public class GenreCostPerLevelDto
{
    public int Id { get; set; }

    [Required]
    public string GenreName { get; set; } = null!;

    public int CostPerLevel { get; set; }
}
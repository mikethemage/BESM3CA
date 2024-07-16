using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Triarch.Dtos.Definitions;

public class LevelableDefinitionDto
{
    public int Id { get; set; }

    public int? MaxLevel { get; set; }

    
    public bool? EnforceMaxLevel { get; set; }

    public int? CostPerLevel { get; set; }

    [MaxLength(100)]
    public string? CostPerLevelDescription { get; set; } = null;

    public List<GenreCostPerLevelDto>? MultiGenreCostPerLevels { get; set; } = null;  

    public string? ProgressionName { get; set; } = null;
    
    public bool? ProgressionReversed { get; set; } = null;

    public List<VariantDefinitionDto>? Variants { get; set; } = null;    

    public int? SpecialPointsPerLevel { get; set; } = null; 
}
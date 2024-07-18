using System.ComponentModel.DataAnnotations;

namespace Triarch.Dtos.Entities;

public class LevelableDataDto
{
    [Required]
    public int Level { get; set; }

    public string? VariantName { get; set; }
    
    public int? FreeLevels { get; set; }
    public int? RequiredLevels { get; set; }
}
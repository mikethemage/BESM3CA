using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Triarch.Dtos.Definitions;

public class VariantDefinitionDto
{
    [Key]
    public int Id { get; set; }

    [MaxLength(100)]
    public string VariantName { get; set; } = null!;

    public int CostPerLevel { get; set; }

    [MaxLength(250)]
    public string? Description { get; set; } = null;

    [DefaultValue(false)]
    public bool IsDefault { get; set; } = false;    
}
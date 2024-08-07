using System.ComponentModel.DataAnnotations;

namespace Triarch.Dtos.Entities;

public class RPGElementDto
{
    [MaxLength(100)]
    [Required]
    public string ElementName { get; set; } = null!;

    public CharacterDataDto? CharacterData { get; set; }    

    public LevelableDataDto? LevelableData { get; set; }

    public string Notes { get; set; } = string.Empty;

    public List<RPGElementDto>? Children { get; set; }

    public bool IsFreebie { get; set; } = false;
}
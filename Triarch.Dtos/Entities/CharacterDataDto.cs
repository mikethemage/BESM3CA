using System.ComponentModel.DataAnnotations;

namespace Triarch.Dtos.Entities;

public class CharacterDataDto
{
    [Required]
    public int Mind { get; set; }

    [Required]
    public int Body { get; set; }

    [Required]
    public int Soul { get; set; }
}
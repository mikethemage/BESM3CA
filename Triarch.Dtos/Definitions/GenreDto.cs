using System.ComponentModel.DataAnnotations;

namespace Triarch.Dtos.Definitions;

public class GenreDto
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string GenreName { get; set; } = string.Empty;

    public int GenreOrder { get; set; }
}
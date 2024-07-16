using System.ComponentModel.DataAnnotations;

namespace Triarch.Dtos.Definitions;

public class ProgressionEntryDto
{
    public int Id { get; set; }

    [MaxLength(400)]
    public string Text { get; set; } = string.Empty;

    public int ProgressionLevel { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace Triarch.Dtos.Definitions;

public class ProgressionEntryDto
{
    [MaxLength(400)]
    public string Text { get; set; } = string.Empty;

    public int ProgressionLevel { get; set; }
}
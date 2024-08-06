using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Triarch.Dtos.Definitions;

public class ProgressionDto
{
    [Required]
    [MaxLength(100)]
    public string ProgressionType { get; set; } = string.Empty;

    [DefaultValue(false)]
    public bool CustomProgression { get; set; } = false;

    [DefaultValue(false)]
    public bool Linear { get; set; } = false;    

    public List<ProgressionEntryDto> Progressions { get; set; } = [];
}
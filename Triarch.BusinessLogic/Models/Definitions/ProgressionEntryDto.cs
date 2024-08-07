using System.ComponentModel.DataAnnotations;

namespace Triarch.BusinessLogic.Models.Definitions;

public class ProgressionEntry
{    
    public string Text { get; set; } = string.Empty;

    public int ProgressionLevel { get; set; }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Triarch.BusinessLogic.Models.Definitions;

public class Progression
{    
    public string ProgressionType { get; set; } = string.Empty;

 
    public bool CustomProgression { get; set; } = false;

 
    public bool Linear { get; set; } = false;    

    public List<ProgressionEntry> Progressions { get; set; } = [];
}
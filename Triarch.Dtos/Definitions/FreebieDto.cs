using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Triarch.Dtos.Definitions;

public class FreebieDto
{       
    [Required]
    public string FreebieElementDefinitionName { get; set; }  = null!;

    [DefaultValue(0)]
    public int FreeLevels { get; set; }  

    [DefaultValue(0)]
    public int RequiredLevels { get; set; }     
}
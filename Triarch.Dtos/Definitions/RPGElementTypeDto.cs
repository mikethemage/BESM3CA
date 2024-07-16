using System.ComponentModel.DataAnnotations;

namespace Triarch.Dtos.Definitions;

public class RPGElementTypeDto
{
    public int Id { get; set; }

    [MaxLength(100)]
    public string TypeName { get; set; } = null!;
    public bool BuiltIn { get; set; } = false;

    public int TypeOrder { get; set; }    
}
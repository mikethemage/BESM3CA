using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Triarch.Dtos.Entities;
public class EntityDto
{
    [Required]
    public string RPGSystemName { get; set; } = null!;
    
    public string? GenreName { get; set; }

    [Required]
    public string EntityType { get; set; } = null!;

    [Required]
    public string EntityName { get; set; } = null!;

    [Required]
    public RPGElementDto RootElement { get; set; } = null!;
}

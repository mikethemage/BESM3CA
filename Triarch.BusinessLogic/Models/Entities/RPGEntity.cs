using Triarch.BusinessLogic.Models.Definitions;

namespace Triarch.BusinessLogic.Models.Entities;

public class RPGEntity
{    
    public RPGSystem RPGSystem { get; set; } = null!;

    public Genre Genre { get; set; } = null!;
    
    public string EntityType { get; set; } = null!;

    public string EntityName { get; set; } = null!;
    
    public RPGElement RootElement { get; set; } = null!;
}

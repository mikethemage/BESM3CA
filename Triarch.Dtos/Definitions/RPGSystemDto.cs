using System.ComponentModel.DataAnnotations;

namespace Triarch.Dtos.Definitions;

public class RPGSystemDto
{    
    public int Id { get; set; }

    public CoreRulesetDto Ruleset { get; set; } = null!; 
    
    [MaxLength(60)]
    public string SystemName { get; set; } = null!;  

    [MaxLength(250)]
    public string? DescriptiveName { get; set; } = null;

    public int OwnerUserId { get; set; } = 1;

    public List<RPGElementTypeDto> ElementTypes { get; set; } = [];

    public List<RPGElementDefinitionDto> ElementDefinitions { get; set; } = [];

    public List<GenreDto> Genres { get; set; } = [];

    public List<ProgressionDto> Progressions { get; set; } = [];

}

using Triarch.BusinessLogic.Models.Definitions;

namespace Triarch.Prototype.ViewModels;

public class ElementDefinitionListItemViewModel
{
    public string DisplayName { get; set; } = null!;
    public RPGElementDefinition Model { get; set; } = null!;
    public bool IsSelected { get; set; } = false;
}
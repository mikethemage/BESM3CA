using Triarch.BusinessLogic.Models.Definitions;

namespace Triarch.Prototype.ViewModels;

public class VariantListItemViewModel
{
    public VariantListItemViewModel(VariantDefinition variantDefinition)
    {
        VariantDefinitionData = variantDefinition;
        DisplayText = variantDefinition.VariantName;
    }
    public VariantDefinition VariantDefinitionData { get; private set; }
    public string DisplayText { get; private set; }
    public bool IsSelected { get; set; } = false;
}
namespace Triarch.Prototype.ViewModels;

public class AllowedChildrenViewModel
{
    public List<FilterTypeViewModel> FilterList { get; set; } = new List<FilterTypeViewModel>();
    public FilterTypeViewModel SelectedFilter { get; set; } = null!;
    public List<ElementDefinitionListItemViewModel> AllowedChildrenList { get; set; } = new List<ElementDefinitionListItemViewModel>();
    public ElementDefinitionListItemViewModel SelectedChild { get; set; } = null!;
}
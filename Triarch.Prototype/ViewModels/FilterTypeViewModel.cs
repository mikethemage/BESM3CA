using Triarch.BusinessLogic.Models.Definitions;

namespace Triarch.Prototype.ViewModels;

public class FilterTypeViewModel
{
    public string DisplayName { get; set; } = null!;
    public RPGElementType? Model { get; set; } = null;
    public bool IsSelected = false;
}
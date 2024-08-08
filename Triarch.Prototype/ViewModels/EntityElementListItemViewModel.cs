using Triarch.BusinessLogic.Models.Entities;

namespace Triarch.Prototype.ViewModels;

public class EntityElementListItemViewModel
{
    public string DisplayText { get; set; }
    public RPGElement ElementData { get; private set; }

    public EntityElementListItemViewModel(RPGElement element)
    {
        ElementData = element;
        DisplayText = element.Name;
        foreach (RPGElement child in element.Children)
        {
            Children.Add(new EntityElementListItemViewModel(child));
        }
    }

    public bool IsSelected { get; set; } = false;

    public List<EntityElementListItemViewModel> Children { get; set; } = new List<EntityElementListItemViewModel>();
}
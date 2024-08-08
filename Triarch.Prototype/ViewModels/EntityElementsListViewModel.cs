using Triarch.BusinessLogic.Models.Entities;

namespace Triarch.Prototype.ViewModels;

public class EntityElementsListViewModel
{
    public EntityElementsListViewModel(RPGEntity entity)
    {
        RootElement = new EntityElementListItemViewModel(entity.RootElement);
        RootElement.IsSelected = true;
        Selected = RootElement;
    }

    public EntityElementListItemViewModel RootElement { get; private set; }

    public EntityElementListItemViewModel Selected { get; set; }
}
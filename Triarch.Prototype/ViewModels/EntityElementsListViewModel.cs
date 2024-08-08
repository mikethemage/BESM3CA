using System.Collections.ObjectModel;
using System.ComponentModel;
using Triarch.BusinessLogic.Models.Entities;

namespace Triarch.Prototype.ViewModels;

public class EntityElementsListViewModel : INotifyPropertyChanged
{
    public EntityElementsListViewModel(RPGEntity entity, EntityViewModel owner)
    {
        _owner = owner;
        RootElements.Add(new EntityElementListItemViewModel(entity.RootElement, this));
        RootElements[0].IsSelected = true;
        _selected = RootElements[0];        
    }
    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public ObservableCollection<EntityElementListItemViewModel> RootElements { get; private set; } = new ObservableCollection<EntityElementListItemViewModel>();

    private EntityElementListItemViewModel _selected;
    private readonly EntityViewModel _owner;

    public EntityElementListItemViewModel Selected { get { return _selected; } set
        { 
            _selected = value;
            _owner.SelectedElement = new EntityElementViewModel(value.ElementData);
            OnPropertyChanged(nameof(Selected));
        } }

    public event PropertyChangedEventHandler? PropertyChanged;
}
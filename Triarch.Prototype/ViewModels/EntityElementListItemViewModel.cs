using System.Collections.ObjectModel;
using System.ComponentModel;
using Triarch.BusinessLogic.Models.Entities;

namespace Triarch.Prototype.ViewModels;

public class EntityElementListItemViewModel : INotifyPropertyChanged
{
    private string _displayText = "";
    public string DisplayText { get { return _displayText; }
        set {  
            _displayText = value; 
            OnPropertyChanged(nameof(DisplayText));
        }
    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public RPGElement ElementData { get; private set; }

    public EntityElementListItemViewModel(RPGElement element, EntityElementsListViewModel owner)
    {
        ElementData = element;
        _owner = owner;
        DisplayText = element.Name;
        foreach (RPGElement child in element.Children)
        {
            Children.Add(new EntityElementListItemViewModel(child, owner));
        }
    }

    private bool _isSelected = false;
    private readonly EntityElementsListViewModel _owner;

    public bool IsSelected 
    { 
        get
        { return _isSelected; }
        set
        {
            _isSelected = value;
            OnPropertyChanged(nameof(IsSelected));
            if(value == true)
            {
                _owner.Selected = this;
            }            
        }
    }

    public ObservableCollection<EntityElementListItemViewModel> Children { get; set; } = new ObservableCollection<EntityElementListItemViewModel>();

    public event PropertyChangedEventHandler? PropertyChanged;
}
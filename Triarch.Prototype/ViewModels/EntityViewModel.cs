using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Triarch.BusinessLogic.Models.Entities;

namespace Triarch.Prototype.ViewModels;

public class EntityViewModel : INotifyPropertyChanged
{
    public string FileName
    {
        get
        {
            if (_filePath == "")
            {
                return "";
            }
            else
            {
                return Path.GetFileName(_filePath);
            }
        }
    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private readonly RPGEntity _entity;
    private string _filePath;
    private EntityElementViewModel? _selectedElement = null;

    public event PropertyChangedEventHandler? PropertyChanged;

    public EntityViewModel(RPGEntity entity, string filePath = "")
    {
        _entity = entity;
        _filePath = filePath;
        EntityElements = new EntityElementsListViewModel(_entity, this);
        OnPropertyChanged(nameof(EntityElements));
    }

    public EntityElementsListViewModel EntityElements { get; private set; }

    public EntityElementViewModel? SelectedElement
    {
        get
        {
            return _selectedElement;
        }
        set
        {
            _selectedElement = value;
            OnPropertyChanged(nameof(SelectedElement));
        }
    }
}

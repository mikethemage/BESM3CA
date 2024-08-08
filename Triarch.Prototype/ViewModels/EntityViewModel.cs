using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Triarch.BusinessLogic.Models.Entities;

namespace Triarch.Prototype.ViewModels;

public class EntityViewModel
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

    private readonly RPGEntity _entity;
    private string _filePath;

    public EntityViewModel(RPGEntity entity, string filePath = "")
    {
        _entity = entity;
        _filePath = filePath;
        EntityElements = new EntityElementsListViewModel(_entity);
    }

    public EntityElementsListViewModel EntityElements { get; private set; }

    public EntityElementViewModel? SelectedElement { get; set; } = null;
}

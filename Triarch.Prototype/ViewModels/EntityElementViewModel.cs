using System.ComponentModel;
using Triarch.BusinessLogic.Models.Definitions;
using Triarch.BusinessLogic.Models.Entities;

namespace Triarch.Prototype.ViewModels;

public class EntityElementViewModel : INotifyPropertyChanged
{
    public EntityElementViewModel(RPGElement element)
    {
        _element = element;

        if(element is Levelable levelable)
        {
            LevelableData = new LevelableDataViewModel(levelable);
            if (element.AssociatedDefinition is LevelableDefinition levelableDefinition)
            {
                if (levelableDefinition.Variants != null && levelableDefinition.Variants.Count > 0)
                {
                    VariantList = new VariantListViewModel(levelable);
                }
            }
        }
        if (element is Character character)
        {
            CharacterData = new CharacterDataViewModel(character);
        }
    }

    private readonly RPGElement _element;

    public event PropertyChangedEventHandler? PropertyChanged;

    public VariantListViewModel? VariantList { get; private set; } = null;
    public AllowedChildrenViewModel AllowedChildrenList { get; set; } = null!;

    public CharacterDataViewModel? CharacterData { get; set; } = null;
    public LevelableDataViewModel? LevelableData { get; set; } = null;

    public int Points { get { return _element.Points; } }
}
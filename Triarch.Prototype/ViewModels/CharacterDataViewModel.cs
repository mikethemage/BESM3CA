using Triarch.BusinessLogic.Models.Entities;

namespace Triarch.Prototype.ViewModels;

public class CharacterDataViewModel
{
    public CharacterDataViewModel(Character model)
    {
        _model = model;
        Body = _model.Body;
        Mind = _model.Mind;
        Soul = _model.Soul;
    }

    public int Body { get; set; }
    public int Mind { get; set; }
    public int Soul { get; set; }

    private Character _model;

}
using Triarch.BusinessLogic.Models.Definitions;
using Triarch.BusinessLogic.Models.Entities;

namespace Triarch.Prototype.ViewModels;

public class VariantListViewModel
{
    public VariantListViewModel(Levelable model)
    {
        if(model.AssociatedDefinition is LevelableDefinition levelableDefinition &&
            levelableDefinition.Variants != null &&
            levelableDefinition.Variants.Count > 0)
        {
            VariantList = levelableDefinition.Variants.Select(x=>new VariantListItemViewModel(x)).ToList();
            if(model.Variant!=null)
            {
                var selected = VariantList.Where(x => x.VariantDefinitionData == model.Variant).FirstOrDefault();
                if (selected != null)
                {
                    selected.IsSelected = true;
                    Selected = selected;
                }                
            }
        }
        else
        {
            throw new Exception("Variant list error!");
        }
    }

    public List<VariantListItemViewModel> VariantList { get; private set; }

    public VariantListItemViewModel Selected {  get; set; } = null!;
}
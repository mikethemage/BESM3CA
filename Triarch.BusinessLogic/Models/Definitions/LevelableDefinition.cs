using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Triarch.BusinessLogic.Models.Entities;
using Triarch.Dtos.Definitions;

namespace Triarch.BusinessLogic.Models.Definitions;
public class LevelableDefinition : RPGElementDefinition
{
    public int? MaxLevel { get; set; }

    public bool? EnforceMaxLevel { get; set; }

    public int? CostPerLevel { get; set; }
    
    public string? CostPerLevelDescription { get; set; } = null;    

    public Progression? Progression { get; set; } = null;

    public bool? ProgressionReversed { get; set; } = null;

    public List<VariantDefinition>? Variants { get; set; } = null;    

    public override RPGElement CreateNode(RPGEntity ownerEntity, string notes, bool isFreebie = false)
    {
        int level;
        if (ElementName == "Weapon")
        {
            level = 0;
        }
        else
        {
            level = 1;
        }
        return CreateNode(ownerEntity, notes, level, isFreebie);
    }

    public virtual RPGElement CreateNode(RPGEntity ownerEntity, string notes, int level, bool isFreebie = false, int freeLevels = 0, int requiredLevels = 0)
    {
        VariantDefinition? defaultVariant = null;

        if(Variants!=null)
        {
            defaultVariant = Variants.Where(x=>x.IsDefault).FirstOrDefault();
        }        

        if(ElementName=="Companion")
        {
            return new Companion
            {
                AssociatedDefinition = this,
                Entity = ownerEntity,
                Notes = notes,
                IsFreebie = isFreebie,
                Level = level,
                FreeLevels = freeLevels,
                RequiredLevels = requiredLevels,
                Variant = defaultVariant
            };
        }        
        else
        {
            return new Levelable
            {
                AssociatedDefinition = this,
                Entity = ownerEntity,
                Notes = notes,
                IsFreebie = isFreebie,
                Level = level,
                FreeLevels = freeLevels,
                RequiredLevels = requiredLevels,
                Variant = defaultVariant
            };
        }
    }
}

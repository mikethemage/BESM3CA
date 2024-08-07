using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Triarch.BusinessLogic.Models.Entities;

namespace Triarch.BusinessLogic.Models.Definitions;
public class SpecialContainerDefinition : LevelableDefinition
{
    public int SpecialPointsPerLevel { get; set; } = 1;

    public override RPGElement CreateNode(RPGEntity ownerEntity, string notes, int level, bool isFreebie = false, int freeLevels = 0, int requiredLevels = 0)
    {
        VariantDefinition? defaultVariant = null;

        if (Variants != null)
        {
            defaultVariant = Variants.Where(x => x.IsDefault).FirstOrDefault();
        }

        return new SpecialContainer
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

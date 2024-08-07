using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Triarch.BusinessLogic.Models.Entities;

namespace Triarch.BusinessLogic.Models.Definitions;
public class PointsContainerDefinition : RPGElementDefinition
{
    public int PointsContainerScale { get; set; } = 1;

    public override RPGElement CreateNode(RPGEntity ownerEntity, string notes, bool isFreebie = false)
    {
        return new PointsContainer
        {
            AssociatedDefinition = this,
            Entity = ownerEntity,
            Notes = notes,
            IsFreebie = isFreebie
        };
    }
}

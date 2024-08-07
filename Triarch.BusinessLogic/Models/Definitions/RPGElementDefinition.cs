using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Triarch.BusinessLogic.Models.Entities;

namespace Triarch.BusinessLogic.Models.Definitions;

public class RPGElementDefinition
{
    
    public string ElementName { get; set; } = null!;

    
    public RPGElementType ElementType { get; set; } = null!;

    
    public string? Description { get; set; } = null;  //Do we need a better name for this?  Used to reference description of effects

    
    public string? Stat { get; set; } = null;

    
    public string? PageNumbers { get; set; } = null;

    
    public bool Human { get; set; } = false;

      
    public List<RPGElementDefinition> AllowedChildren { get; set; } = [];

    public List<Freebie> Freebies { get; set; } = new();

    public virtual RPGElement CreateNode(RPGEntity ownerEntity, string notes, bool isFreebie = false)
    {
        if(ElementType.TypeName=="Character")
        {
            return new Character { 
                AssociatedDefinition = this,
                Entity = ownerEntity,
                Notes = notes,
                IsFreebie = isFreebie
            };
        }
        else
        {
            throw new Exception("Cannot create abstract element!");
        }        
    }   


}
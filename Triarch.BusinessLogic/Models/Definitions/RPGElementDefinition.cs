using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using Triarch.Dtos.Definitions;

namespace Triarch.BusinessLogic.Models.Definitions;

public abstract class RPGElementDefinition
{
    //Properties:
    //Everything should have:    
    public string Name { get; private set; } = string.Empty;
    public RPGElementType Type { get; private set; } = null!;
    public string? Description { get; private set; }  //Character doesn't need description really
    public List<RPGElementDefinition> Children { get; private set; } = new();

    public List<Freebie> Freebies { get; set; } = new();

    //  To check if still needed:        
    private string? Stat { get; set; }
    private string? PageNumbers { get; set; }
    private bool Human { get; set; }

    public void RefreshFilteredPotentialChildren(string filter)
    {
        List<RPGElementDefinition> selectedAttributeChildren = Children;
        if (selectedAttributeChildren != null)
        {
            //LINQ Version:
            List<RPGElementDefinition> filteredAttList = selectedAttributeChildren
                .Where(a => a.Name != "Character" && (filter == "All" || filter == "" || a.Type.TypeName == filter))
                .OrderBy(a => a.Type.TypeOrder)
                .ThenBy(a => a.Name)
                .ToList();

            FilteredPotentialChildren = filteredAttList;
        }
        else
        {
            FilteredPotentialChildren = null;
        }
    }    

    private List<RPGElementDefinition>? _filteredPotentialChildren;

    public List<RPGElementDefinition>? FilteredPotentialChildren
    {
        get { return _filteredPotentialChildren; }
        set
        {
            _filteredPotentialChildren = value;
            
        }
    }

    //Constructor:
    public RPGElementDefinition()
    {            
    }

    //Methods:
    public abstract BaseNode CreateNode(string notes, RPGSystem associatedSystem, bool isLoading, int level = 1, int freeLevels = 0, int requiredLevels = 0, bool isFreebie = false);        

    public void AddChild(RPGElementDefinition Child)
    {            
        Children.Add(Child);            
    }

    //Serialization:
    public virtual RPGElementDefinitionDto Serialize()
    {
        RPGElementDefinitionDto result = new RPGElementDefinitionDto
        {                
            ElementName = this.Name,
            Stat = this.Stat,
            PageNumbers = this.PageNumbers,
            Human = this.Human,
            ElementTypeName = this.Type.TypeName,
            Description = this.Description                
        };

        //Convert childrenlist to string:
        IEnumerable<string> ChildIDs = from child in Children
                                    select child.Name;

        result.AllowedChildrenNames = ChildIDs.ToList();

        if (Freebies != null && Freebies.Count > 0)
        {
            result.Freebies = Freebies.Select(x =>

            new FreebieDto
            {
                FreebieElementDefinitionName = x.SubAttribute.Name,
                FreeLevels = x.FreeLevels,
                RequiredLevels = x.RequiredLevels
            }).ToList();
        }

        return result;
    }

    public RPGElementDefinition(RPGElementDefinitionDto data, RPGElementType elementType)
    {            
        Name = data.ElementName;
        Stat = data.Stat ?? "";
        PageNumbers = data.PageNumbers ?? "";
        Human = data.Human;
        Type = elementType;
        Description = data.Description ?? "";

        Children = new List<RPGElementDefinition>();

        if (data.Freebies != null)
        {
            foreach (FreebieDto freebie in data.Freebies)
            {
                Freebies.Add(new Freebie
                {
                    //SubAttributeName = freebie.FreebieElementDefinitionName,
                    RequiredLevels = freebie.RequiredLevels,
                    FreeLevels = freebie.FreeLevels
                });
            }
        }
    }
}

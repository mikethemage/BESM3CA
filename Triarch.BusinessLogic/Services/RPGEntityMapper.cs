using Triarch.BusinessLogic.Models.Definitions;
using Triarch.BusinessLogic.Models.Entities;
using Triarch.Dtos.Entities;

namespace Triarch.BusinessLogic.Services;
public class RPGEntityMapper
{
    IRPGSystemProvider _rPGSystemProvider;

    public RPGEntityMapper(IRPGSystemProvider rPGSystemProvider)
    {
        _rPGSystemProvider = rPGSystemProvider;
    }

    public RPGEntity Deserialize(EntityDto input)
    {
        RPGSystem rPGSystem = _rPGSystemProvider.LoadSystem(input.RPGSystemName);
        RPGEntity output = new RPGEntity { RPGSystem = rPGSystem, EntityName=input.EntityName, EntityType=input.EntityType};
        output.Genre = rPGSystem.Genres.Where(x=>x.GenreName==input.GenreName).FirstOrDefault() ?? rPGSystem.Genres.First();

        output.RootElement = DeserializeElement(input.RootElement, rPGSystem, output);

        return output;
    }

    private RPGElement DeserializeElement(RPGElementDto input, RPGSystem rPGSystem, RPGEntity owningEntity)
    {
        RPGElementDefinition associatedDefinition = rPGSystem.ElementDefinitions.Where(x => x.ElementName == input.ElementName).First();
        RPGElement output = associatedDefinition.CreateNode(owningEntity, input.Notes, input.IsFreebie);

        if(output is Character character && input.CharacterData!=null)
        {
            character.Body=input.CharacterData.Body;
            character.Mind = input.CharacterData.Mind;
            character.Soul = input.CharacterData.Soul;
        }

        if (output is Levelable levelable && input.LevelableData!=null) 
        {
            if(input.LevelableData.FreeLevels!=null)
            {
                levelable.FreeLevels = (int)input.LevelableData.FreeLevels;
            }

            if (input.LevelableData.RequiredLevels != null)
            {
                levelable.RequiredLevels = (int)input.LevelableData.RequiredLevels;
            }

            levelable.Level=input.LevelableData.Level;

            if(input.LevelableData.VariantName!=null && associatedDefinition is LevelableDefinition levelableDefinition && levelableDefinition.Variants != null)
            {
                levelable.Variant = levelableDefinition.Variants.Where(x=>x.VariantName==input.LevelableData.VariantName).FirstOrDefault();
            }
        }

        if(input.Children!=null)
        {
            foreach (RPGElementDto childDto in input.Children)
            {
                RPGElement child = DeserializeElement(childDto, rPGSystem, owningEntity);
                child.Parent = output;
                output.Children.Add(child);
            }
        }

        return output;
    }

    

    public EntityDto Serialize(RPGEntity input)
    {
        EntityDto output = new EntityDto
        {
            EntityName = input.EntityName,
            EntityType = input.EntityType,
            GenreName = input.Genre.GenreName,
            RPGSystemName = input.RPGSystem.SystemName
        };

        output.RootElement = SerializeElement(input.RootElement);

        return output;
    }

    private RPGElementDto SerializeElement(RPGElement input)
    {
        RPGElementDto output = new RPGElementDto
        {
            ElementName = input.Name,
            Notes = input.Notes,
            IsFreebie = input.IsFreebie
        };

        if (input is Character character)
        {
            output.CharacterData = new CharacterDataDto
            {
                Body = character.Body,
                Mind = character.Mind,
                Soul = character.Soul
            };
        }

        if (input is Levelable levelable)
        {
            output.LevelableData = new LevelableDataDto
            {
                Level = levelable.Level
            };

            if(levelable.IsFreebie)
            {
                output.LevelableData.RequiredLevels = levelable.RequiredLevels;
                output.LevelableData.FreeLevels = levelable.FreeLevels;
            }

            if (levelable.Variant!=null)
            {
                output.LevelableData.VariantName=levelable.Variant.VariantName;
            }                
        }

        if(input.Children.Count>0)
        {
            output.Children = new List<RPGElementDto>();
            foreach (RPGElement child in input.Children)
            {
                output.Children.Add(SerializeElement(child));
            }
        }       

        return output;
    }
}

using Triarch.BusinessLogic.Models.Definitions;
using Triarch.Dtos.Definitions;

namespace Triarch.BusinessLogic.Services;
public class RPGSystemMapper
{
    public RPGSystem Deserialize(RPGSystemDto input)
    {
        RPGSystem output = new RPGSystem
        {
            SystemName = input.SystemName
        };

        foreach (RPGElementTypeDto typeDto in input.ElementTypes)
        {
            output.ElementTypes.Add(new RPGElementType { TypeName = typeDto.TypeName, TypeOrder = typeDto.TypeOrder, BuiltIn = typeDto.BuiltIn });
        }

        foreach (GenreDto genreDto in input.Genres)
        {
            output.Genres.Add(new Genre { GenreName = genreDto.GenreName, GenreOrder = genreDto.GenreOrder });
        }

        foreach (ProgressionDto progressionDto in input.Progressions)
        {
            Progression progression = new Progression { ProgressionType = progressionDto.ProgressionType, CustomProgression = progressionDto.CustomProgression, Linear = progressionDto.Linear };
            foreach (ProgressionEntryDto progressionEntryDto in progressionDto.Progressions)
            {
                progression.Progressions.Add(new ProgressionEntry { ProgressionLevel = progressionEntryDto.ProgressionLevel, Text = progressionEntryDto.Text });
            }
            output.Progressions.Add(progression);
        }

        foreach (RPGElementDefinitionDto elementDefinitionDto in input.ElementDefinitions)
        {
            RPGElementDefinition elementDefinition;
            if (elementDefinitionDto.ElementTypeName == "Character")
            {
                elementDefinition = new RPGElementDefinition();
            }
            else if (elementDefinitionDto.PointsContainerScale != null)
            {
                elementDefinition = new PointsContainerDefinition { PointsContainerScale = (int)elementDefinitionDto.PointsContainerScale };
            }
            else if (elementDefinitionDto.LevelableData != null)
            {
                if (elementDefinitionDto.ElementTypeName == "Companion")
                {
                    elementDefinition = new LevelableDefinition();
                }
                else if (elementDefinitionDto.LevelableData.SpecialPointsPerLevel != null)
                {
                    elementDefinition = new SpecialContainerDefinition { SpecialPointsPerLevel = (int)elementDefinitionDto.LevelableData.SpecialPointsPerLevel };
                }
                else if (elementDefinitionDto.LevelableData.MultiGenreCostPerLevels != null)
                {
                    elementDefinition = new MultiGenreDefinition { MultiGenreCostPerLevels = elementDefinitionDto.LevelableData.MultiGenreCostPerLevels.Select(x => new GenreCostPerLevel { Genre = output.Genres.Where(y => y.GenreName == x.GenreName).First(), CostPerLevel = x.CostPerLevel }).ToList() };
                }
                else
                {
                    elementDefinition = new LevelableDefinition();
                }
                ((LevelableDefinition)elementDefinition).MaxLevel = elementDefinitionDto.LevelableData.MaxLevel;
                ((LevelableDefinition)elementDefinition).EnforceMaxLevel = elementDefinitionDto.LevelableData.EnforceMaxLevel;
                ((LevelableDefinition)elementDefinition).CostPerLevel = elementDefinitionDto.LevelableData.CostPerLevel;
                ((LevelableDefinition)elementDefinition).CostPerLevelDescription = elementDefinitionDto.LevelableData.CostPerLevelDescription;

                ((LevelableDefinition)elementDefinition).Progression = output.Progressions.Where(x => x.ProgressionType == elementDefinitionDto.LevelableData.ProgressionName).FirstOrDefault();
                ((LevelableDefinition)elementDefinition).ProgressionReversed = elementDefinitionDto.LevelableData.ProgressionReversed;
                if(elementDefinitionDto.LevelableData.Variants!=null)
                {
                    ((LevelableDefinition)elementDefinition).Variants = elementDefinitionDto.LevelableData.Variants.Select(x => new VariantDefinition { VariantName = x.VariantName, CostPerLevel = x.CostPerLevel, Description = x.Description ?? "", IsDefault = x.IsDefault }).ToList();
                }               
            }
            else
            {
                throw new Exception("Unexpected definition");
            }

            elementDefinition.ElementName= elementDefinitionDto.ElementName;
            elementDefinition.ElementType = output.ElementTypes.Where(x => x.TypeName==elementDefinitionDto.ElementTypeName).First();
            elementDefinition.Stat= elementDefinitionDto.Stat;
            elementDefinition.Human=elementDefinitionDto.Human;
            elementDefinition.Description= elementDefinitionDto.Description;
            elementDefinition.PageNumbers= elementDefinitionDto.PageNumbers;            

            output.ElementDefinitions.Add(elementDefinition);
        }

        foreach(RPGElementDefinitionDto elementDefinitionDtoWithFreebies in input.ElementDefinitions.Where(x=> x.Freebies != null && x.Freebies.Count > 0))
        {
            RPGElementDefinition? elementDefinitionWithFreebies = output.ElementDefinitions.Where(x => x.ElementName == elementDefinitionDtoWithFreebies.ElementName).FirstOrDefault();
            if (elementDefinitionWithFreebies != null)
            {
                elementDefinitionWithFreebies.Freebies = elementDefinitionDtoWithFreebies.Freebies!.Select(x => new Freebie {
                    FreebieElementDefinition = output.ElementDefinitions.Where(y=>y.ElementName==x.FreebieElementDefinitionName).First(), 
                    FreeLevels = x.FreeLevels, 
                    RequiredLevels = x.RequiredLevels 
                }).ToList();
            }
        }

        foreach (RPGElementDefinitionDto elementDefinitionDtoWithChildren in input.ElementDefinitions.Where(x => x.AllowedChildrenNames != null && x.AllowedChildrenNames.Count > 0))
        {
            RPGElementDefinition? elementDefinitionWithChildren = output.ElementDefinitions.Where(x => x.ElementName == elementDefinitionDtoWithChildren.ElementName).FirstOrDefault();
            if (elementDefinitionWithChildren != null)
            {
                elementDefinitionWithChildren.AllowedChildren = elementDefinitionDtoWithChildren.AllowedChildrenNames.Select(x=>output.ElementDefinitions.Where(y=>y.ElementName==x).First()).ToList();
            }
        }

        return output;
    }

    public RPGSystemDto Serialize(RPGSystem input)
    {
        RPGSystemDto output = new RPGSystemDto {
            SystemName = input.SystemName            
        };

        output.ElementTypes = input.ElementTypes.Select(x=>new RPGElementTypeDto { TypeName=x.TypeName, TypeOrder=x.TypeOrder, BuiltIn = x.BuiltIn}).ToList();

        output.Genres = input.Genres.Select(x=>new GenreDto { GenreName=x.GenreName,GenreOrder=x.GenreOrder }).ToList();

        output.Progressions = input.Progressions.Select(x=>new ProgressionDto { ProgressionType=x.ProgressionType,Linear=x.Linear,CustomProgression=x.CustomProgression, Progressions = x.Progressions.Select(y=>new ProgressionEntryDto { ProgressionLevel=y.ProgressionLevel, Text=y.Text}).ToList()}).ToList();

        output.ElementDefinitions = new List<RPGElementDefinitionDto>();

        foreach (RPGElementDefinition elementDefinition in input.ElementDefinitions)
        {
            RPGElementDefinitionDto elementDefinitionDto = new RPGElementDefinitionDto
            {
                ElementName= elementDefinition.ElementName,
                Description= elementDefinition.Description,
                ElementTypeName=elementDefinition.ElementType.TypeName,
                Human= elementDefinition.Human,
                PageNumbers= elementDefinition.PageNumbers,
                Stat = elementDefinition.Stat                
            };

            if (elementDefinition is PointsContainerDefinition pointsContainerDefinition)
            {
                elementDefinitionDto.PointsContainerScale = pointsContainerDefinition.PointsContainerScale;
            }

            if (elementDefinition is LevelableDefinition levelableDefinition)
            {
                elementDefinitionDto.LevelableData = new LevelableDefinitionDto
                {
                    CostPerLevel = levelableDefinition.CostPerLevel,
                    CostPerLevelDescription= levelableDefinition.CostPerLevelDescription,
                    EnforceMaxLevel= levelableDefinition.EnforceMaxLevel,
                    MaxLevel= levelableDefinition.MaxLevel,
                    ProgressionReversed= levelableDefinition.ProgressionReversed                    
                };

                if (levelableDefinition.Progression != null)
                {
                    elementDefinitionDto.LevelableData.ProgressionName = levelableDefinition.Progression.ProgressionType;
                }

                if (levelableDefinition.Variants != null && levelableDefinition.Variants.Count > 0)
                {
                    elementDefinitionDto.LevelableData.Variants = levelableDefinition.Variants.Select(x => new VariantDefinitionDto { VariantName=x.VariantName, CostPerLevel=x.CostPerLevel, Description = x.Description, IsDefault=x.IsDefault }).ToList();
                }

                if(levelableDefinition is SpecialContainerDefinition specialContainerDefinition)
                {
                    elementDefinitionDto.LevelableData.SpecialPointsPerLevel = specialContainerDefinition.SpecialPointsPerLevel;
                }

                if(levelableDefinition is MultiGenreDefinition multiGenreDefinition)
                {
                    elementDefinitionDto.LevelableData.MultiGenreCostPerLevels = multiGenreDefinition.MultiGenreCostPerLevels.Select(x=>new GenreCostPerLevelDto { GenreName=x.Genre.GenreName, CostPerLevel=x.CostPerLevel }).ToList();
                }
            }

            if (elementDefinition.Freebies.Count > 0)
            {
                elementDefinitionDto.Freebies = elementDefinition.Freebies.Select(x=>new FreebieDto { FreebieElementDefinitionName=x.FreebieElementDefinition.ElementName, FreeLevels=x.FreeLevels, RequiredLevels=x.RequiredLevels}).ToList();
            }

            elementDefinitionDto.AllowedChildrenNames = elementDefinition.AllowedChildren.Select(x=>x.ElementName).ToList();

            output.ElementDefinitions.Add(elementDefinitionDto);
        }

        return output;
    }
}

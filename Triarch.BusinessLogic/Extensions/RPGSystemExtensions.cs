//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Linq;
//using System.Text;
//using System.Text.Json;
//using System.Threading.Tasks;
//using Triarch.BusinessLogic.Models.Definitions;
//using Triarch.Dtos.Definitions;

//namespace Triarch.BusinessLogic.Extensions;
//public static class RPGSystemExtensions
//{
//    private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull };

//    public static RPGSystemDto Serialize(this RPGSystem)
//    {
//        RPGSystemDto result = new RPGSystemDto { SystemName = this.ListingName ?? "", Genres = this.Genres?.Select(x => new GenreDto { GenreName = x }).ToList() ?? new List<GenreDto>(), ElementDefinitions = new List<RPGElementDefinitionDto>(), ElementTypes = new List<RPGElementTypeDto>(), Progressions = new List<ProgressionDto>() };

//        if (AttributeList != null)
//        {
//            foreach (DataListing attribute in AttributeList)
//            {
//                result.ElementDefinitions.Add(attribute.Serialize());
//            }
//        }

//        if (TypeList != null)
//        {
//            foreach (TypeListing typeListing in TypeList)
//            {
//                result.ElementTypes.Add(typeListing.Serialize());
//            }
//        }

//        if (ProgressionList != null)
//        {
//            foreach (ProgressionListing progressionListing in ProgressionList)
//            {
//                result.Progressions.Add(progressionListing.Serialize());
//            }
//        }

//        return result;
//    }

//    public static RPGSystem? JSONLoader(ListingLocation listingLocation)
//    {
//        //Load JSON data to temp object:
//        //MasterListingSerialized temp = MasterListingSerialized.JSONLoader(listingLocation);

//        string input = File.ReadAllText(listingLocation.ListingPath);

//        //Load listing:
//        RPGSystemDto? temp = JsonSerializer.Deserialize<RPGSystemDto>(input, _jsonOptions);

//        if (temp != null)
//        {


//            //Create new listing data object:
//            RPGSystem result = new RPGSystem { ListingName = temp.SystemName, Genres = temp.Genres.Select(x => x.GenreName).ToList(), ProgressionList = new List<ProgressionListing>(), AttributeList = new List<DataListing>(), TypeList = new List<TypeListing>() };

//            //Deserialize Type Listings:
//            foreach (RPGElementTypeDto typeListing in temp.ElementTypes)
//            {
//                result.TypeList.Add(new TypeListing
//                {
//                    Name = typeListing.TypeName,
//                    TypeOrder = typeListing.TypeOrder
//                }
//                );
//            }

//            //Deserialize Progression Listings:
//            foreach (ProgressionDto progression in temp.Progressions)
//            {
//                result.ProgressionList.Add(new ProgressionListing
//                {
//                    ProgressionType = progression.ProgressionType,
//                    ProgressionsList = progression.Progressions.OrderBy(x => x.ProgressionLevel).Select(x => x.Text).ToList(),
//                    CustomProgression = progression.CustomProgression
//                });
//            }

//            //Deserialize raw attribute data:
//            foreach (RPGElementDefinitionDto data in temp.ElementDefinitions)
//            {
//                DataListing? newData = null;

//                if (data.ElementTypeName == "Character")
//                {
//                    newData = new CharacterDataListing(data);
//                }
//                else if (data.ElementName == "Companion")
//                {
//                    newData = new CompanionDataListing(data);
//                }
//                else if (data.PointsContainerScale != null)
//                {
//                    newData = new PointsContainerDataListing(data);
//                }
//                else if (data.LevelableData?.SpecialPointsPerLevel != null)
//                {
//                    newData = new SpecialContainerDataListing(data);
//                }
//                else if (data.LevelableData?.Variants != null)
//                {
//                    newData = new LevelableDataListing(data);
//                }
//                else if (data.LevelableData?.MultiGenreCostPerLevels != null)
//                {
//                    newData = new MultiGenreDataListing(data);
//                }
//                else if (data.LevelableData != null)
//                {
//                    newData = new LevelableDataListing(data);
//                }
//                else
//                {
//                    throw new Exception("Unexpected data listing");
//                }

//                if (newData is LevelableDataListing levelableDataListing)
//                {
//                    levelableDataListing.Progression = result.ProgressionList.Where(x => x.ProgressionType == levelableDataListing.ProgressionName).FirstOrDefault();
//                }

//                if (newData != null)
//                {
//                    result.AttributeList.Add(newData);
//                }
//            }

//            //Link children and freebies:
//            foreach (var attribute in temp.ElementDefinitions)
//            {
//                if (attribute.AllowedChildrenNames != null || attribute.Freebies != null)
//                {
//                    DataListing? Parent = result.AttributeList.Find(x => x.Name == attribute.ElementName);
//                    if (Parent != null)
//                    {
//                        //Link Children:                    
//                        if (attribute.AllowedChildrenNames != null && attribute.AllowedChildrenNames.Count > 0)
//                        {
//                            foreach (string Child in attribute.AllowedChildrenNames)
//                            {
//                                DataListing? childListing = result.AttributeList.Find(x => x.Name == Child);
//                                if (childListing != null)
//                                {
//                                    Parent.AddChild(childListing);
//                                }
//                            }
//                        }
//                        Parent.RefreshFilteredPotentialChildren("All");
//                    }
//                }
//            }

//            return result;
//        }

//        return null;
//    }

//    public void CreateJSON(string outputPath)
//    {
//        //Code to write out JSON data files.   
//        //Should not be being called at present - debugging only:
//        RPGSystemDto output = Serialize();
//        string outputText = JsonSerializer.Serialize(output, _jsonOptions);
//        File.WriteAllText(outputPath, outputText);
//    }
//}

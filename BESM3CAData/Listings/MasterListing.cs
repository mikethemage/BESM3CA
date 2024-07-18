using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Triarch.Dtos.Definitions;


namespace BESM3CAData.Listings
{
    public class MasterListing
    {
        public RPGSystemDto Serialize()
        {
            RPGSystemDto result = new RPGSystemDto { SystemName = this.ListingName, Genres = this.Genres.Select(x=>new GenreDto { GenreName=x}).ToList(), ElementDefinitions = new List<RPGElementDefinitionDto>(), ElementTypes = new List<RPGElementTypeDto>(), Progressions = new List<ProgressionDto>() };

            foreach (DataListing attribute in AttributeList)
            {
                result.ElementDefinitions.Add(attribute.Serialize());
            }

            foreach (TypeListing typeListing in TypeList)
            {
                result.ElementTypes.Add(typeListing.Serialize());
            }

            foreach (ProgressionListing progressionListing in ProgressionList)
            {
                result.Progressions.Add(progressionListing.Serialize());
            }

            return result;
        }

        //Properties:
        public List<DataListing> AttributeList { get; set; }

        public List<TypeListing> TypeList { get; set; }
        public string ListingName { get; set; }

        public List<string> Genres { get; set; }

        public List<ProgressionListing> ProgressionList { get; set; }

        public string GetProgression(string progressionType, int rank)
        {
            ProgressionListing SelectedProgression = ProgressionList.Find(n => n.ProgressionType == progressionType);
            if (SelectedProgression == null)
            {
                return "";
            }
            else
            {
                return SelectedProgression.GetProgressionValue(rank);
            }
        }

        //Member functions:
        public static MasterListing JSONLoader(ListingLocation listingLocation)
        {
            //Load JSON data to temp object:
            //MasterListingSerialized temp = MasterListingSerialized.JSONLoader(listingLocation);

            string input = File.ReadAllText(listingLocation.ListingPath);

            //Load listing:
            RPGSystemDto temp = JsonSerializer.Deserialize<RPGSystemDto>(input, new JsonSerializerOptions { PropertyNameCaseInsensitive=true, DefaultIgnoreCondition=System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull});


            //Create new listing data object:
            MasterListing result = new MasterListing { ListingName = temp.SystemName, Genres = temp.Genres.Select(x => x.GenreName).ToList(), ProgressionList = new List<ProgressionListing>(), AttributeList = new List<DataListing>(), TypeList = new List<TypeListing>() };

            //Deserialize Type Listings:
            foreach (RPGElementTypeDto typeListing in temp.ElementTypes)
            {
                result.TypeList.Add(new TypeListing
                {
                    ID = typeListing.Id,
                    Name = typeListing.TypeName,
                    TypeOrder = typeListing.TypeOrder
                }
                );
            }

            //Deserialize Progression Listings:
            foreach (ProgressionDto progression in temp.Progressions)
            {
                result.ProgressionList.Add(new ProgressionListing
                {
                    ProgressionType = progression.ProgressionType,
                    ProgressionsList = progression.Progressions.OrderBy(x=>x.ProgressionLevel).Select(x=>x.Text).ToList(),
                    CustomProgression = progression.CustomProgression
                });
            }

            //Deserialize raw attribute data:
            foreach (RPGElementDefinitionDto data in temp.ElementDefinitions)
            {
                DataListing newData = null;

                if (data.ElementTypeName == "Character")
                {
                    newData = new CharacterDataListing(data);
                }
                else if (data.ElementName == "Companion")
                {
                    newData = new CompanionDataListing(data);
                }
                else if (data.PointsContainerScale != null)
                {
                    newData = new PointsContainerDataListing(data);
                }
                else if (data.LevelableData?.SpecialPointsPerLevel != null)
                {
                    newData = new SpecialContainerDataListing(data);                    
                }
                else if (data.LevelableData?.Variants != null)
                {
                    newData = new LevelableDataListing(data);                    
                }
                else if (data.LevelableData?.MultiGenreCostPerLevels != null)
                {
                    newData = new MultiGenreDataListing(data);
                }
                else if (data.LevelableData != null)
                {
                    newData = new LevelableDataListing(data);                                       
                }
                else
                {
                    throw new Exception("Unexpected data listing");
                }

                if(newData is LevelableDataListing levelableDataListing)
                {
                    levelableDataListing.Progression = result.ProgressionList.Where(x => x.ProgressionType == levelableDataListing.ProgressionName).FirstOrDefault();
                }

                if (newData != null)
                {
                    result.AttributeList.Add(newData);
                }
            }

            //Link children and freebies:
            foreach (var attribute in temp.ElementDefinitions)
            {
                if (attribute.AllowedChildrenNames != null || attribute.Freebies != null)
                {
                    DataListing Parent = result.AttributeList.Find(x => x.ID == attribute.Id);

                    //Link Children:                    
                    if (attribute.AllowedChildrenNames.Count > 0)
                    {
                        foreach (string Child in attribute.AllowedChildrenNames)
                        {
                            Parent.AddChild(result.AttributeList.Find(x => x.Name == Child));                            
                        }
                    }
                    Parent.RefreshFilteredPotentialChildren("All");

                    //if (Parent is IFreebieDataListing freebieDataListing)
                    //{
                    //    if(freebieDataListing.Freebies != null)
                    //    {
                    //        foreach (FreebieListing freebie in freebieDataListing.Freebies)
                    //        {
                    //            freebie.SubAttribute = result.AttributeList.Where(x => x.Name == freebie.SubAttributeName).FirstOrDefault();
                    //        }
                    //    }                    
                    //}
                }
            }

            return result;
        }

        public void CreateJSON(string outputPath)
        {
            //Code to write out JSON data files.   
            //Should not be being called at present - debugging only:
            RPGSystemDto output = Serialize();
            string outputText = JsonSerializer.Serialize(output, new JsonSerializerOptions { PropertyNameCaseInsensitive=true, DefaultIgnoreCondition=System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull });
            File.WriteAllText(outputPath, outputText);            
        }
    }
}

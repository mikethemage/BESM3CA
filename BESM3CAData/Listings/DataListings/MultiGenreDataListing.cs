using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BESM3CAData.Model;
using BESM3CAData.Control;
using Triarch.Dtos.Definitions;

namespace BESM3CAData.Listings
{
    public class MultiGenreDataListing : LevelableDataListing
    {
        //Properties:
        public List<int>? GenrePoints { get; private set; }


        //Constructors:
        public MultiGenreDataListing(RPGElementDefinitionDto data) : base(data)
        {
            if(data.LevelableData?.MultiGenreCostPerLevels!=null)
            {
                GenrePoints = data.LevelableData.MultiGenreCostPerLevels.Select(x => x.CostPerLevel).ToList();
            }            
        }


        //Methods:
        public override DataNode CreateNode(string notes, RPGEntity controller, bool isLoading, int level = 1, int freeLevels = 0, int requiredLevels = 0, bool isFreebie=false)
        {
            return new MultiGenreDataNode(this, notes, controller, isLoading, level, freeLevels,requiredLevels,isFreebie);
        }

        public override RPGElementDefinitionDto Serialize()
        {
            RPGElementDefinitionDto result = base.Serialize();
            if (result.LevelableData != null)
            {
                result.LevelableData.MultiGenreCostPerLevels = new List<GenreCostPerLevelDto>();

                if (GenrePoints is not null)
                {
                    for (int i = 0; i < GenrePoints.Count; i++)
                    {
                        result.LevelableData.MultiGenreCostPerLevels.Add(new GenreCostPerLevelDto
                        {
                            CostPerLevel = GenrePoints[i],
                            GenreName = "FIX ME" //Todo - fix this!
                        });
                    }
                }
            }
            return result;
        }
    }
}

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
        public List<int> GenrePoints { get; private set; }


        //Constructors:
        public MultiGenreDataListing(RPGElementDefinitionDto data) : base(data)
        {
            GenrePoints = data.LevelableData.MultiGenreCostPerLevels.Select(x=>x.CostPerLevel).ToList();
        }


        //Methods:
        public override DataNode CreateNode(string notes, RPGEntity controller, int level = 1, int pointAdj = 0)
        {
            return new MultiGenreDataNode(this, notes, controller, level, pointAdj);
        }

        public override RPGElementDefinitionDto Serialize()
        {
            RPGElementDefinitionDto result = base.Serialize();
            result.LevelableData.MultiGenreCostPerLevels = new List<GenreCostPerLevelDto>();

            for (int i = 0; i < GenrePoints.Count ; i++)
            {
                result.LevelableData.MultiGenreCostPerLevels.Add(new GenreCostPerLevelDto
                {
                    CostPerLevel = GenrePoints[i],
                    GenreName = "FIX ME" //Todo - fix this!
                });
            }
            
            return result;
        }
    }
}

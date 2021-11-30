using BESM3CAData.Listings.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BESM3CAData.Model;
using BESM3CAData.Control;

namespace BESM3CAData.Listings
{
    public class MultiGenreDataListing : LevelableDataListing
    {
        //Properties:
        public List<int> GenrePoints { get; private set; }


        //Constructors:
        public MultiGenreDataListing(DataListingSerialized data) : base(data)
        {
            GenrePoints = data.GenrePoints;
        }


        //Methods:
        public override DataNode CreateNode(string notes, RPGEntity controller, int level = 1, int pointAdj = 0)
        {
            return new MultiGenreDataNode(this, notes, controller, level, pointAdj);
        }

        public override DataListingSerialized Serialize()
        {
            DataListingSerialized result = base.Serialize();
            result.MultiGenre = true;
            result.GenrePoints = GenrePoints;
            return result;
        }
    }
}

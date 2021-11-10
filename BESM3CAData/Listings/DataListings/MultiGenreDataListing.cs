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
        public List<int> GenrePoints { get; private set; }

        public override DataNode CreateNode(string notes, DataController controller, int level = 1, int pointAdj = 0)
        {
            return new MultiGenreDataNode(this, notes, controller, level, pointAdj);
        }

        public override bool MultiGenre
        {
            get { return true; }
        }

        public MultiGenreDataListing(DataListingSerialized data) : base(data)
        {
            GenrePoints = data.GenrePoints;
        }

        public override DataListingSerialized Serialize()
        {
            DataListingSerialized result = base.Serialize();
            result.GenrePoints = GenrePoints;
            return result;
        }
    }
}

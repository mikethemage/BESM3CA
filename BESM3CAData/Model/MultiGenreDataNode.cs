using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BESM3CAData.Listings;
using System.Xml;
using BESM3CAData.Control;

namespace BESM3CAData.Model
{
    public class MultiGenreDataNode : LevelableDataNode
    {
        public MultiGenreDataNode(DataController controller, string Notes = "") : base(controller, Notes)
        {

        }

        public MultiGenreDataNode(MultiGenreDataListing attribute, string notes, DataController controller, int level = 1, int pointAdj = 0) : base(attribute, notes, controller, level, pointAdj)
        {

        }
    }
}

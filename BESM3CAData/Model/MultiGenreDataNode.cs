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

        public override void InvalidateGenrePoints()
        {
            if (_dataListing is MultiGenreDataListing skillDataListing && skillDataListing.GenrePoints != null)
            {
                PointsUpToDate = false;
                UpdatePointsPerLevel();
            }

            base.InvalidateGenrePoints();
        }

        protected override void UpdatePointsPerLevel()
        {
            if (AssociatedController.SelectedGenreIndex > -1 && _dataListing is MultiGenreDataListing skillDataListing && skillDataListing.GenrePoints != null && skillDataListing.GenrePoints.Count > AssociatedController.SelectedGenreIndex)
            {
                PointsPerLevel = skillDataListing.GenrePoints[AssociatedController.SelectedGenreIndex];
            }
            else if (_dataListing is LevelableDataListing levelableDataListing)
            {
                PointsPerLevel = levelableDataListing.CostperLevel;
            }
            else
            {
                PointsPerLevel = 0;
            }
        }
    }
}

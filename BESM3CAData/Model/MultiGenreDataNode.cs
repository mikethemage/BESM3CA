using BESM3CAData.Control;
using BESM3CAData.Listings;

namespace BESM3CAData.Model
{
    public class MultiGenreDataNode : LevelableDataNode
    {
        //Constructors:
        public MultiGenreDataNode(DataController controller, string Notes = "") : base(controller, Notes)
        {

        }

        public MultiGenreDataNode(MultiGenreDataListing attribute, string notes, DataController controller, int level = 1, int pointAdj = 0) : base(attribute, notes, controller, level, pointAdj)
        {

        }


        //Methods:
        public override void InvalidateGenrePoints()
        {
            if (AssociatedListing is MultiGenreDataListing skillDataListing && skillDataListing.GenrePoints != null)
            {
                PointsUpToDate = false;
                UpdatePointsPerLevel();
            }

            base.InvalidateGenrePoints();
        }

        protected override void UpdatePointsPerLevel()
        {
            if (AssociatedController.SelectedGenreIndex > -1 && AssociatedListing is MultiGenreDataListing skillDataListing && skillDataListing.GenrePoints != null && skillDataListing.GenrePoints.Count > AssociatedController.SelectedGenreIndex)
            {
                PointsPerLevel = skillDataListing.GenrePoints[AssociatedController.SelectedGenreIndex];
            }
            else if (AssociatedListing is LevelableDataListing levelableDataListing)
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

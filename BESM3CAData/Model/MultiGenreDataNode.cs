using BESM3CAData.Control;
using BESM3CAData.Listings;

namespace BESM3CAData.Model
{
    public class MultiGenreDataNode : LevelableDataNode
    {
        //Constructors:
        public MultiGenreDataNode(RPGEntity controller, string Notes = "") : base(controller, Notes)
        {

        }

        public MultiGenreDataNode(MultiGenreDataListing attribute, string notes, RPGEntity controller, bool isLoading, int level = 1, int freeLevels = 0, int requiredLevels = 0, bool isFreebie = false) : base(attribute, isLoading, notes, controller, level, freeLevels,requiredLevels, isFreebie)
        {

        }


        //Methods:
        public override void InvalidateGenrePoints()
        {
            if (AssociatedListing is MultiGenreDataListing skillDataListing && skillDataListing.GenrePoints != null)
            {
                
                UpdatePointsPerLevel();
            }

            base.InvalidateGenrePoints();
        }

        protected override void UpdatePointsPerLevel()
        {
            
            if (AssociatedController.SelectedGenreEntry != null && AssociatedListing is MultiGenreDataListing skillDataListing && skillDataListing.GenrePoints != null && skillDataListing.GenrePoints.Count > AssociatedController.SelectedGenreEntry.Index)
            {
                PointsPerLevel = skillDataListing.GenrePoints[AssociatedController.SelectedGenreEntry.Index];
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

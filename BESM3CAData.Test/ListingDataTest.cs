using BESM3CAData.Listings;
using Xunit;

namespace BESM3CAData.Test
{
    public class ListingDataTest
    {
        [Fact]
        public void ListingData_ShouldLoadDefault()
        {
            MasterListing? DefaultListing = MasterListing.JSONLoader(new ListingLocation
            {
                BuiltIn = true,
                ListingName = "BESM 3rd Edition",
                ListingPathArray = new string[] { "Datafiles", "NEW_BESM3E_Get_result.json" }
            });
            Assert.NotNull(DefaultListing);
        }

        [Fact]
        public void ListingData_ShouldHaveAttributes()
        {
            MasterListing? DefaultListing = MasterListing.JSONLoader(new ListingLocation
            {
                BuiltIn = true,
                ListingName = "BESM 3rd Edition",
                ListingPathArray = new string[] { "Datafiles", "NEW_BESM3E_Get_result.json" }
            });
            Assert.NotNull(DefaultListing?.AttributeList);
            Assert.True(DefaultListing.AttributeList.Count > 0);
        }

        [Fact]
        public void ListingData_ShouldHaveTypes()
        {
            MasterListing? DefaultListing = MasterListing.JSONLoader(new ListingLocation
            {
                BuiltIn = true,
                ListingName = "BESM 3rd Edition",
                ListingPathArray = new string[] { "Datafiles", "NEW_BESM3E_Get_result.json" }
            });
            Assert.NotNull(DefaultListing?.TypeList);
            Assert.True(DefaultListing.TypeList.Count > 0);
        }

        [Theory]
        [InlineData("Fast", 4, "1,000")]
        [InlineData("Time", 16, "Permanent")]
        [InlineData("Time", 17, "ERROR")]
        public void ListingData_GetProgressionShouldWork(string progressionType, int rank, string expected)
        {
            MasterListing? DefaultListing = MasterListing.JSONLoader(new ListingLocation
            {
                BuiltIn = true,
                ListingName = "BESM 3rd Edition",
                ListingPathArray = new string[] { "Datafiles", "NEW_BESM3E_Get_result.json" }
            });
            Assert.NotNull(DefaultListing);
            string output = DefaultListing.GetProgression(progressionType, rank);

            Assert.Equal(expected, output);
        }

    }
}

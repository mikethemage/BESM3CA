using BESM3CAData.Listings;
using Xunit;

namespace BESM3CAData.Test
{
    public class AttributeListingTest
    {
        //[Fact]
        //public void Attribute_AddChildShouldWork()
        //{
        //    DataListingSerialized testParentSerialized = new DataListingSerialized { ID = 1, Name = "Test Parent" };
        //    DataListingSerialized testChild1Serialized = new DataListingSerialized { ID = 2, Name = "Test Child1" };

        //    DataListing testParent = new LevelableDataListing(testParentSerialized);
        //    DataListing testChild1 = new LevelableDataListing(testChild1Serialized);

        //    testParent.AddChild(testChild1);

        //    Assert.Contains<DataListing>(testChild1, testParent.Children);
        //}

        //[Fact]
        //public void Attribute_GetChildrenListShouldWork()
        //{

        //    DataListingSerialized testParentSerialized = new DataListingSerialized { ID = 1, Name = "Test Parent" };
        //    DataListingSerialized testChild1Serialized = new DataListingSerialized { ID = 2, Name = "Test Child1" };
        //    DataListingSerialized testChild2Serialized = new DataListingSerialized { ID = 3, Name = "Test Child2" };

        //    DataListing testParent = new LevelableDataListing(testParentSerialized);
        //    DataListing testChild1 = new LevelableDataListing(testChild1Serialized);
        //    DataListing testChild2 = new LevelableDataListing(testChild2Serialized);

        //    testParent.AddChild(testChild1);
        //    testParent.AddChild(testChild2);

        //    DataListingSerialized testParentDeserialized = testParent.Serialize();

        //    Assert.Equal("2,3", testParentDeserialized.ChildrenList);
        //}

    }
}

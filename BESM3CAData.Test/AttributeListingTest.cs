using BESM3CAData.Listings;
using BESM3CAData.Listings.Serialization;
using Xunit;

namespace BESM3CAData.Test
{
    public class AttributeListingTest
    {
        [Fact]
        public void Attribute_AddChildShouldWork()
        {
            AttributeListingSerialized testParentSerialized = new AttributeListingSerialized { ID = 1, Name = "Test Parent" };
            AttributeListingSerialized testChild1Serialized = new AttributeListingSerialized { ID = 2, Name = "Test Child1" };

            AttributeListing testParent = AttributeListing.Deserialize(testParentSerialized);
            AttributeListing testChild1 = AttributeListing.Deserialize(testChild1Serialized);

            testParent.AddChild(testChild1);

            Assert.Contains<AttributeListing>(testChild1, testParent.Children);
        }

        [Fact]
        public void Attribute_GetChildrenListShouldWork()
        {

            AttributeListingSerialized testParentSerialized = new AttributeListingSerialized { ID = 1, Name = "Test Parent" };
            AttributeListingSerialized testChild1Serialized = new AttributeListingSerialized { ID = 2, Name = "Test Child1" };
            AttributeListingSerialized testChild2Serialized = new AttributeListingSerialized { ID = 3, Name = "Test Child2" };

            AttributeListing testParent = AttributeListing.Deserialize(testParentSerialized);
            AttributeListing testChild1 = AttributeListing.Deserialize(testChild1Serialized);
            AttributeListing testChild2 = AttributeListing.Deserialize(testChild2Serialized);

            testParent.AddChild(testChild1);
            testParent.AddChild(testChild2);

            AttributeListingSerialized testParentDeserialized= testParent.Serialize();

            Assert.Equal("2,3", testParentDeserialized.ChildrenList);
        }

    }
}

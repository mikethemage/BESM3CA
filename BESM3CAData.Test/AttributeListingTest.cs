using BESM3CAData.Templates;
using Xunit;

namespace BESM3CAData.Test
{
    public class AttributeListingTest
    {
        [Fact]
        public void Attribute_AddChildShouldWork()
        {
            AttributeListing testParent = new AttributeListing { ID = 1, Name = "Test Parent" };
            AttributeListing testChild1 = new AttributeListing { ID = 2, Name = "Test Child1" };            

            testParent.AddChild(testChild1);            

            Assert.Contains<AttributeListing>(testChild1, testParent.Children);
        }

        [Fact]
        public void Attribute_GetChildrenListShouldWork()
        {
            AttributeListing testParent = new AttributeListing { ID = 1, Name = "Test Parent" };
            AttributeListing testChild1 = new AttributeListing { ID = 2, Name = "Test Child1" };
            AttributeListing testChild2 = new AttributeListing { ID = 3, Name = "Test Child2" };

            testParent.AddChild(testChild1);
            testParent.AddChild(testChild2);

            Assert.Equal("2,3", testParent.ChildrenList);
        }

    }
}

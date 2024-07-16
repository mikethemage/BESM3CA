using BESM3CAData.Listings;
using Triarch.Dtos.Definitions;
using Xunit;

namespace BESM3CAData.Test
{
    public class AttributeListingTest
    {
        [Fact]
        public void Attribute_AddChildShouldWork()
        {
            RPGElementDefinitionDto testParentSerialized = new RPGElementDefinitionDto { Id = 1, ElementName = "Test Parent", LevelableData = new LevelableDefinitionDto { CostPerLevel=0,MaxLevel=0,EnforceMaxLevel=false } };
            RPGElementDefinitionDto testChild1Serialized = new RPGElementDefinitionDto { Id = 2, ElementName = "Test Child1", LevelableData = new LevelableDefinitionDto { CostPerLevel = 0, MaxLevel = 0, EnforceMaxLevel = false } };

            DataListing testParent = new LevelableDataListing(testParentSerialized);
            DataListing testChild1 = new LevelableDataListing(testChild1Serialized);

            testParent.AddChild(testChild1);

            Assert.Contains<DataListing>(testChild1, testParent.Children);
        }

        [Fact]
        public void Attribute_GetChildrenListShouldWork()
        {

            RPGElementDefinitionDto testParentSerialized = new RPGElementDefinitionDto { Id = 1, ElementName = "Test Parent", LevelableData = new LevelableDefinitionDto { CostPerLevel = 0, MaxLevel = 0, EnforceMaxLevel = false } };
            RPGElementDefinitionDto testChild1Serialized = new RPGElementDefinitionDto { Id = 2, ElementName = "Test Child1", LevelableData = new LevelableDefinitionDto { CostPerLevel = 0, MaxLevel = 0, EnforceMaxLevel = false } };
            RPGElementDefinitionDto testChild2Serialized = new RPGElementDefinitionDto { Id = 3, ElementName = "Test Child2", LevelableData = new LevelableDefinitionDto { CostPerLevel = 0, MaxLevel = 0, EnforceMaxLevel = false } };

            DataListing testParent = new LevelableDataListing(testParentSerialized);
            DataListing testChild1 = new LevelableDataListing(testChild1Serialized);
            DataListing testChild2 = new LevelableDataListing(testChild2Serialized);

            testParent.AddChild(testChild1);
            testParent.AddChild(testChild2);

            RPGElementDefinitionDto testParentDeserialized = testParent.Serialize();

            Assert.Equal("Test Child1,Test Child2", string.Join(',',testParentDeserialized.AllowedChildrenNames));
        }

    }
}

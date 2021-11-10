using BESM3CAData.Control;
using BESM3CAData.Listings;
using BESM3CAData.Model;
using System.Collections.Generic;
using Xunit;

namespace BESM3CAData.Test
{
    public class AttributeNodeTest
    {
        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void AttributeNode_DisplayTextContainsAttributeName(int attributePosition)
        {
            DataController testController = new DataController();
            DataListing selectedAttribute = testController.RootCharacter.GetFilteredPotentialChildren("All")[attributePosition];
            testController.RootCharacter.AddChildAttribute(selectedAttribute);
            DataNode foundAttribute = (DataNode)testController.RootCharacter.FirstChild;
            Assert.Contains(foundAttribute.Name, foundAttribute.DisplayText);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void AttributeNode_DisplayTextContainsPoints(int attributePosition)
        {
            DataController testController = new DataController();
            DataListing selectedAttribute = testController.RootCharacter.GetFilteredPotentialChildren("All")[attributePosition];
            testController.RootCharacter.AddChildAttribute(selectedAttribute);
            DataNode foundAttribute = (DataNode)testController.RootCharacter.FirstChild;
            Assert.Contains(foundAttribute.GetPoints().ToString() + " Points", foundAttribute.DisplayText);
        }

        [Theory]
        [InlineData(2, 2)]
        [InlineData(6, 1)]
        [InlineData(8, 1)]
        [InlineData(12, 12)]
        public void AttributeNode_DescriptionCalculationShouldNotFail(int attributePosition, int level)
        {
            DataController testController = new DataController();
            LevelableDataListing selectedAttribute = (LevelableDataListing)testController.RootCharacter.GetFilteredPotentialChildren("All")[attributePosition];
            testController.RootCharacter.AddChildAttribute(selectedAttribute);
            LevelableDataNode foundAttribute = (LevelableDataNode)testController.RootCharacter.FirstChild;
            for (int i = 1; i < level; i++)
            {
                foundAttribute.RaiseLevel();
            }

            Assert.DoesNotContain("NaN", foundAttribute.AttributeDescription);
        }

        [Theory]
        [InlineData("Alternate Form", "Attribute")]
        [InlineData("Armour", "Variable")]
        public void Attribute_GetTypesForFilterShouldContain(string attributeName, string expected)
        {
            DataController testController = new DataController();
            DataNode testAttribute = new DataNode(testController.SelectedListingData.AttributeList.Find(x => x.Name == attributeName), "", testController);
            List<string> output = testAttribute.GetTypesForFilter();
            Assert.True(output.Count > 0);
            Assert.Contains<string>(expected, output);
        }

    }
}

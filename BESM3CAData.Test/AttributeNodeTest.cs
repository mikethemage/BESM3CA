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

            testController.CurrentEntity.RootCharacter.AssociatedListing.RefreshFilteredPotentialChildren("All");
            DataListing selectedAttribute = testController.CurrentEntity.RootCharacter.AssociatedListing.FilteredPotentialChildren[attributePosition];


            testController.CurrentEntity.RootCharacter.AddChildAttribute(selectedAttribute);
            DataNode foundAttribute = (DataNode)testController.CurrentEntity.RootCharacter.FirstChild;
            Assert.Contains(foundAttribute.Name, foundAttribute.DisplayText);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void AttributeNode_DisplayTextContainsPoints(int attributePosition)
        {
            DataController testController = new DataController();
                        
            testController.CurrentEntity.RootCharacter.AssociatedListing.RefreshFilteredPotentialChildren("All");
            DataListing selectedAttribute = testController.CurrentEntity.RootCharacter.AssociatedListing.FilteredPotentialChildren[attributePosition];


            testController.CurrentEntity.RootCharacter.AddChildAttribute(selectedAttribute);
            DataNode foundAttribute = (DataNode)testController.CurrentEntity.RootCharacter.FirstChild;
            Assert.Contains(foundAttribute.Points.ToString() + " Points", foundAttribute.DisplayText);
        }

        [Theory]
        [InlineData(2, 2)]
        [InlineData(6, 1)]
        //[InlineData(8, 1)]
        [InlineData(12, 12)]
        public void AttributeNode_DescriptionCalculationShouldNotFail(int attributePosition, int level)
        {
            DataController testController = new DataController();           

            testController.CurrentEntity.RootCharacter.AssociatedListing.RefreshFilteredPotentialChildren("All");
            LevelableDataListing selectedAttribute = (LevelableDataListing)testController.CurrentEntity.RootCharacter.AssociatedListing.FilteredPotentialChildren[attributePosition];

            testController.CurrentEntity.RootCharacter.AddChildAttribute(selectedAttribute);
            LevelableDataNode foundAttribute = (LevelableDataNode)testController.CurrentEntity.RootCharacter.FirstChild;
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
                       
            BaseNode testAttribute = testController.SelectedListingData.AttributeList.Find(x => x.Name == attributeName).CreateNode("", testController.CurrentEntity, false);
            List<string> output = testAttribute.GetTypesForFilter();
            Assert.True(output.Count > 0);
            Assert.Contains<string>(expected, output);
        }

    }
}

using BESM3CAData.Control;
using BESM3CAData.Listings;
using BESM3CAData.Model;
using System.Collections.Generic;
using Xunit;

namespace BESM3CAData.Test
{
    public class CharacterNodeTest
    {
        [Fact]
        public void RootNode_ShouldBeCharacter()
        {
            DataController testController = new DataController();
            Assert.NotNull(testController.CurrentEntity.RootCharacter);
            Assert.IsType<CharacterNode>(testController.CurrentEntity.RootCharacter);
        }

        [Fact]
        public void Character_NameShouldBeCharacter()
        {
            DataController testController = new DataController();
            Assert.Equal("Character", testController.CurrentEntity.RootCharacter.Name);
        }

        [Fact]
        public void Character_ShouldHavePotentialChildren()
        {
            DataController testController = new DataController();       

            testController.CurrentEntity.RootCharacter.AssociatedListing.RefreshFilteredPotentialChildren("All");
            List<DataListing> foundPotentialChildren = testController.CurrentEntity.RootCharacter.AssociatedListing.FilteredPotentialChildren;

            Assert.True(foundPotentialChildren.Count > 0);
        }

        [Theory]
        [InlineData(1, 1, 1)]
        [InlineData(0, 0, 0)]
        [InlineData(3, 10, 5)]
        public void Character_BasePointsShouldBeCorrect(int body, int mind, int soul)
        {
            DataController testController = new DataController();
            ((CharacterNode)testController.CurrentEntity.RootCharacter).Body = body;
            ((CharacterNode)testController.CurrentEntity.RootCharacter).Mind = mind;
            ((CharacterNode)testController.CurrentEntity.RootCharacter).Soul = soul;
            int expectedPoints = (body + mind + soul) * 10;
            Assert.Equal(expectedPoints, testController.CurrentEntity.RootCharacter.Points);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void Character_AddChildAttributeShouldExist(int attributePosition)
        {
            DataController testController = new DataController();

            testController.CurrentEntity.RootCharacter.AssociatedListing.RefreshFilteredPotentialChildren("All");
            DataListing selectedAttribute = testController.CurrentEntity.RootCharacter.AssociatedListing.FilteredPotentialChildren[attributePosition];

            testController.CurrentEntity.RootCharacter.AddChildAttribute(selectedAttribute);

            DataNode foundAttribute = (DataNode)testController.CurrentEntity.RootCharacter.FirstChild;

            Assert.Equal(selectedAttribute.ID, foundAttribute.ID);
            Assert.Equal(selectedAttribute.Name, foundAttribute.Name);
        }

        [Theory]
        [InlineData("All")]
        [InlineData("Attribute")]
        [InlineData("Defect")]
        [InlineData("Skill")]
        public void Character_GetTypesForFilterShouldContain(string expected)
        {
            DataController testController = new DataController();
            List<string>? output = null;
            if (testController.SelectedListingData.AttributeList.Find(x => x.Name == "Character") is CharacterDataListing characterDataListing)
            {
                CharacterNode testCharacter = new CharacterNode(characterDataListing, false, "", testController.CurrentEntity, false);
                output = testCharacter.GetTypesForFilter();
            }
            Assert.NotNull(output);
            Assert.True(output.Count > 0);
            Assert.Contains<string>(expected, output);
        }
    }
}

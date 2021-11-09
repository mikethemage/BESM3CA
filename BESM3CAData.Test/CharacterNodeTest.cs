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
            Assert.NotNull(testController.RootCharacter);
            Assert.IsType<CharacterNode>(testController.RootCharacter);
        }

        [Fact]
        public void Character_NameShouldBeCharacter()
        {
            DataController testController = new DataController();
            Assert.Equal("Character", testController.RootCharacter.Name);
        }

        [Fact]
        public void Character_ShouldHavePotentialChildren()
        {
            DataController testController = new DataController();
            List<DataListing> foundPotentialChildren = testController.RootCharacter.GetFilteredPotentialChildren("All");
            Assert.True(foundPotentialChildren.Count > 0);
        }

        [Theory]
        [InlineData(1, 1, 1)]
        [InlineData(0, 0, 0)]
        [InlineData(3, 10, 5)]
        public void Character_BasePointsShouldBeCorrect(int body, int mind, int soul)
        {
            DataController testController = new DataController();
            testController.RootCharacter.Body = body;
            testController.RootCharacter.Mind = mind;
            testController.RootCharacter.Soul = soul;
            int expectedPoints = (body + mind + soul) * 10;
            Assert.Equal(expectedPoints, testController.RootCharacter.GetPoints());
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void Character_AddChildAttributeShouldExist(int attributePosition)
        {
            DataController testController = new DataController();
            DataListing selectedAttribute = testController.RootCharacter.GetFilteredPotentialChildren("All")[attributePosition];
            testController.RootCharacter.AddChildAttribute(selectedAttribute);

            AttributeNode foundAttribute = (AttributeNode)testController.RootCharacter.FirstChild;

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
            CharacterNode testCharacter = new CharacterNode(testController);
            List<string> output = testCharacter.GetTypesForFilter();
            Assert.True(output.Count > 0);
            Assert.Contains<string>(expected, output);
        }

    }
}

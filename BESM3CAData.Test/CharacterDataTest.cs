using BESM3CAData.Model;
using BESM3CAData.Templates;
using System.Collections.Generic;
using Xunit;

namespace BESM3CAData.Test
{
    public class CharacterDataTest
    {
        [Fact]
        public void RootNode_ShouldBeCharacter()
        {
            Controller testController = new Controller();
            Assert.NotNull(testController.RootCharacter);
            Assert.IsType<CharacterData>(testController.RootCharacter);
        }

        [Fact]
        public void Character_NameShouldBeCharacter()
        {
            Controller testController = new Controller();
            Assert.Equal("Character", testController.RootCharacter.Name);
        }

        [Fact]
        public void Character_ShouldHavePotentialChildren()
        {
            Controller testController = new Controller();
            List<AttributeListing> foundPotentialChildren = testController.RootCharacter.GetFilteredPotentialChildren("All");
            Assert.True(foundPotentialChildren.Count > 0);
        }

        [Theory]
        [InlineData(1, 1, 1)]
        [InlineData(0, 0, 0)]
        [InlineData(3, 10, 5)]
        public void Character_BasePointsShouldBeCorrect(int body, int mind, int soul)
        {
            Controller testController = new Controller();
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
            Controller testController = new Controller();
            AttributeListing selectedAttribute = testController.RootCharacter.GetFilteredPotentialChildren("All")[attributePosition];
            testController.RootCharacter.AddChildAttribute(selectedAttribute);

            AttributeData foundAttribute = (AttributeData)testController.RootCharacter.FirstChild;

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
            Controller testController = new Controller();
            CharacterData testCharacter = new CharacterData(testController);
            List<string> output = testCharacter.GetTypesForFilter();
            Assert.True(output.Count > 0);
            Assert.Contains<string>(expected, output);
        }

    }
}

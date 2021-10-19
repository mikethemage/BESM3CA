using BESM3CAData.Model;
using BESM3CAData.Templates;
using System.Collections.Generic;
using Xunit;

namespace BESM3CAData.Test
{
    public class AttributeDataTest
    {
        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void AttributeData_DisplayTextContainsAttributeName(int attributePosition)
        {
            Controller testController = new Controller();
            AttributeListing selectedAttribute = testController.RootCharacter.GetFilteredPotentialChildren("All")[attributePosition];
            testController.RootCharacter.AddChildAttribute(selectedAttribute);
            AttributeData foundAttribute = (AttributeData)testController.RootCharacter.FirstChild;
            Assert.Contains(foundAttribute.Name, foundAttribute.DisplayText);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void AttributeData_DisplayTextContainsPoints(int attributePosition)
        {
            Controller testController = new Controller();
            AttributeListing selectedAttribute = testController.RootCharacter.GetFilteredPotentialChildren("All")[attributePosition];
            testController.RootCharacter.AddChildAttribute(selectedAttribute);
            AttributeData foundAttribute = (AttributeData)testController.RootCharacter.FirstChild;
            Assert.Contains(foundAttribute.GetPoints().ToString() + " Points", foundAttribute.DisplayText);
        }

        [Theory]
        [InlineData(2,2)]
        [InlineData(6,1)]
        [InlineData(8,1)]
        [InlineData(12,12)]
        public void AttributeData_DescriptionCalculationShouldNotFail(int attributePosition, int level)
        {
            Controller testController = new Controller();
            AttributeListing selectedAttribute = testController.RootCharacter.GetFilteredPotentialChildren("All")[attributePosition];
            testController.RootCharacter.AddChildAttribute(selectedAttribute);
            AttributeData foundAttribute = (AttributeData)testController.RootCharacter.FirstChild;
            for (int i = 1; i < level; i++)
            {
                foundAttribute.RaiseLevel();
            }

            Assert.DoesNotContain("NaN", foundAttribute.AttributeDescription);
        }

        [Theory]        
        [InlineData("Alternate Form","Attribute")]
        [InlineData("Armour","Variable")]        
        public void Attribute_GetTypesForFilterShouldContain(string attributeName, string expected)
        {
            Controller testController = new Controller();
            AttributeData testAttribute= new AttributeData(testController.SelectedTemplate.AttributeList.Find(x => x.Name==attributeName),"",testController);
            List<string> output = testAttribute.GetTypesForFilter();
            Assert.True(output.Count > 0);
            Assert.Contains<string>(expected, output);
        }

    }
}

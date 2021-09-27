using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BESM3CAData.Model;
using BESM3CAData.Templates;
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
            ListItems selectedAttribute = testController.RootCharacter.GetFilteredPotentialChildren("All")[attributePosition];
            testController.RootCharacter.AddChildAttribute(selectedAttribute.Name, selectedAttribute.ID);
            AttributeData foundAttribute = (AttributeData)testController.RootCharacter.Children;
            Assert.Contains(foundAttribute.Name, foundAttribute.DisplayText);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void AttributeData_DisplayTextContainsPoints(int attributePosition)
        {
            Controller testController = new Controller();
            ListItems selectedAttribute = testController.RootCharacter.GetFilteredPotentialChildren("All")[attributePosition];
            testController.RootCharacter.AddChildAttribute(selectedAttribute.Name, selectedAttribute.ID);
            AttributeData foundAttribute = (AttributeData)testController.RootCharacter.Children;
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
            ListItems selectedAttribute = testController.RootCharacter.GetFilteredPotentialChildren("All")[attributePosition];
            testController.RootCharacter.AddChildAttribute(selectedAttribute.Name, selectedAttribute.ID);
            AttributeData foundAttribute = (AttributeData)testController.RootCharacter.Children;
            for (int i = 1; i < level; i++)
            {
                foundAttribute.RaiseLevel();
            }

            Assert.DoesNotContain("NaN", foundAttribute.AttributeDescription);
        }

    }
}

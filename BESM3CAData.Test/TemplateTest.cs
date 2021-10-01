using Xunit;
using BESM3CAData.Templates;
using System.Collections.Generic;

namespace BESM3CAData.Test
{
    public class TemplateTest
    {
        [Fact]
        public void Template_ShouldLoadDefaultTemplate()
        {
            TemplateData DefaultTemplate = TemplateData.JSONLoader();
            Assert.NotNull(DefaultTemplate);
        }

        [Fact]
        public void Template_ShouldHaveAttributes()
        {
            TemplateData DefaultTemplate = TemplateData.JSONLoader();
            Assert.True(DefaultTemplate.AttributeList.Count > 0);
        }

        [Fact]
        public void Template_ShouldHaveTypes()
        {
            TemplateData DefaultTemplate = TemplateData.JSONLoader();
            Assert.True(DefaultTemplate.TypeList.Count > 0);
        }

        [Theory]
        [InlineData("Fast", 4,"1,000")]
        [InlineData("Time", 16, "Permanent")]
        [InlineData("Time", 17, "ERROR")]
        public void Template_GetProgressionShouldWork(string progressionType, int rank, string expected)
        {
            TemplateData DefaultTemplate = TemplateData.JSONLoader();

            string output = DefaultTemplate.GetProgression(progressionType, rank);

            Assert.Equal(expected, output);
        }

        

        [Fact]
        public void Template_ShouldHaveValidVariants()
        {
            TemplateData DefaultTemplate = TemplateData.JSONLoader();
            if(DefaultTemplate.VariantList.Count>0)
            {
                int attID = DefaultTemplate.VariantList[0].AttributeID;
                Assert.True(attID > 0);
                AttributeListing linkedAttribute = DefaultTemplate.AttributeList.Find(x => x.ID == attID);
                Assert.NotNull(linkedAttribute);
            }
            else
            {
                AttributeListing attributeWithVariants = DefaultTemplate.AttributeList.Find(x => x.RequiresVariant == true);
                Assert.Null(attributeWithVariants);
            }
        }
    }
}

using BESM3CAData.Templates;
using Xunit;

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
                
    }
}

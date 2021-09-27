using System;
using Xunit;
using BESM3CAData.Templates;

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


    }
}

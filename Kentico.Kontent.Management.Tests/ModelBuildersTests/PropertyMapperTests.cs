using System.Collections.Generic;
using System.Reflection;
using Kentico.Kontent.Management.Modules.ModelBuilders;
using Kentico.Kontent.Management.Tests.Data;
using Newtonsoft.Json;
using NSubstitute;
using Xunit;

namespace Kentico.Kontent.Management.Tests.ModelBuildersTests
{
    public class PropertyMapperTests
    {
        [Fact]
        public void GetNameMapping_ReturnsCorrectNames()
        {
            var expected = new Dictionary<string, string>
            {
                { "Title", "title" },
                { "PostDate", "post_date" },
                { "UrlPattern", "url_pattern" },
                { "BodyCopy", "body_copy" },
                { "TeaserImage", "teaser_image" },
                { "RelatedArticles", "related_articles" },
                { "Personas", "personas" },
                { "MetaDescription", "meta_description" },
                { "MetaKeywords", "meta_keywords" },
                { "Summary", "summary" },
            };

            var actual = new PropertyMapper().GetNameMapping(typeof(ComplexTestModel));

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("TextElement", "text_element", "text_element", true)]
        public void IsMatch_WithJsonProperty_ReturnsCorrectResult(string name, string codename, string elementName, bool expected)
        {
            var propertyInfo = Substitute.For<PropertyInfo>();
            var attr = new JsonPropertyAttribute(codename);
            propertyInfo.Name.Returns(name);
            propertyInfo.GetCustomAttributes(typeof(JsonPropertyAttribute)).Returns(new []{attr});

            var actual = new PropertyMapper().IsMatch(propertyInfo, elementName);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("text_element")]
        [InlineData("rich_text_element")]
        public void IsMatch_WithJsonIgnore_ReturnsFalse(string elementName)
        {
            var propertyInfo = Substitute.For<PropertyInfo>();
            var attr = new JsonIgnoreAttribute();
            propertyInfo.GetCustomAttributes(typeof(JsonIgnoreAttribute)).Returns(new[] { attr });

            var actual = new PropertyMapper().IsMatch(propertyInfo, elementName);

            Assert.False(actual);
        }

        [Theory]
        [InlineData("TextElement", "text_element", true)]
        [InlineData("TextElement", "textelement", true)]
        [InlineData("Textelement", "textelement", true)]
        [InlineData("TextLement", "text_element", false)]
        public void IsMatch_WithoutJsonProperty_ReturnsCorrectResult(string name, string elementName, bool expected)
        {
            var propertyInfo = Substitute.For<PropertyInfo>();
            propertyInfo.Name.Returns(name);

            var actual = new PropertyMapper().IsMatch(propertyInfo, elementName);

            Assert.Equal(expected, actual);
        }

    }
}

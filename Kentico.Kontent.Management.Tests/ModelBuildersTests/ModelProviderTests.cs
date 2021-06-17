using System;
using System.Collections.Generic;
using System.Linq;
using Kentico.Kontent.Management.Models.Assets;
using Kentico.Kontent.Management.Models.Items;
using Kentico.Kontent.Management.Models.StronglyTyped;
using Kentico.Kontent.Management.Modules.ActionInvoker;
using Kentico.Kontent.Management.Modules.ModelBuilders;
using Kentico.Kontent.Management.Tests.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Kentico.Kontent.Management.Tests.ModelBuildersTests
{
    public class ModelProviderTests
    {
        private readonly IModelProvider _modelProvider;

        public ModelProviderTests()
        {
            _modelProvider = new ModelProvider();
        }

        [Fact]
        public void GetContentItemVariantModel_ReturnsExpected()
        {
            var expected = GetTestModel();
            var model = new ContentItemVariantModel
            {
                Elements = ToDynamic(expected)
            };
            var actual = _modelProvider.GetContentItemVariantModel<ComplexTestModel>(model).Elements;

            AssertElements(expected, actual);
        }

        [Fact]
        public void GetContentItemVariantUpsertModel_ReturnsExpected()
        {
            var model = GetTestModel();
            var expected = ToDynamic(model);
            var actual = _modelProvider.GetContentItemVariantUpsertModel(model).Elements;

            AssertElements(expected, actual);
        }

        private static void AssertElements(ComplexTestModel expected, ComplexTestModel actual)
        {
            AssertIdentifiers(expected.TeaserImage.Select(x => x.Id.Value), actual.TeaserImage.Select(x => x.Id.Value));
            Assert.Equal(expected.PostDate, actual.PostDate);
            Assert.Equal(expected.UrlPattern, actual.UrlPattern);
            AssertIdentifiers(expected.RelatedArticles?.Select(x => x.Id.Value), actual.RelatedArticles?.Select(x => x.Id.Value));
            Assert.Equal(expected.BodyCopy, actual.BodyCopy);
            AssertIdentifiers(expected.Personas?.Select(x => x.Id.Value), actual.Personas?.Select(x => x.Id.Value));
            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(expected.MetaDescription, actual.MetaDescription);
            Assert.Equal(expected.MetaKeywords, actual.MetaKeywords);
            Assert.Equal(expected.Summary, actual.Summary);
        }

        private static ComplexTestModel GetTestModel()
        {
            return new ComplexTestModel
            {
                Title = "text",
                PostDate = DateTime.Now,
                UrlPattern = new UrlSlug{ Value = "urlslug", Mode = "custom"},
                BodyCopy = "RichText",
                TeaserImage = new[] { Guid.NewGuid(), Guid.NewGuid() }.Select(AssetIdentifier.ById).ToArray(),
                RelatedArticles = new[] { Guid.NewGuid(), Guid.NewGuid() }.Select(ContentItemIdentifier.ById).ToArray(),
                Personas = new[] { Guid.NewGuid(), Guid.NewGuid() }.Select(TaxonomyTermIdentifier.ById).ToList(),
            };
        }

        private static dynamic ToDynamic(object value)
        {
            var serialized = JsonConvert.SerializeObject(value, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            return JsonConvert.DeserializeObject<dynamic>(serialized, new JsonSerializerSettings { Converters = new JsonConverter[] { new DynamicObjectJsonConverter() } });
        }

        private static void AssertElements(IDictionary<string, object> expected, IDictionary<string, object> actual)
        {
            Assert.True(JToken.DeepEquals(JToken.FromObject(expected), JToken.FromObject(actual)));
        }

        private static void AssertIdentifiers(IEnumerable<Guid> expected, IEnumerable<Guid> actual)
        {
            if (expected == null && actual == null) return;
            if (expected == null || actual == null)
            {
                Assert.True(false, "Null check");
            }
            Assert.Equal(expected, actual);
        }
    }
}
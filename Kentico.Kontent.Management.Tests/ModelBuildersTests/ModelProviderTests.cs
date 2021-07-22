using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Kentico.Kontent.Management.Models.Assets;
using Kentico.Kontent.Management.Modules.Extensions;
using Kentico.Kontent.Management.Models.Items;
using Kentico.Kontent.Management.Models.StronglyTyped;
using Kentico.Kontent.Management.Modules.ActionInvoker;
using Kentico.Kontent.Management.Modules.ModelBuilders;
using Kentico.Kontent.Management.Tests.Data;
using Newtonsoft.Json;
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
            var expected = model;
            var actual = _modelProvider.GetContentItemVariantUpsertModel(model).Elements;

            AssertElements(expected, actual);
        }

        private void AssertElements(ComplexTestModel expected, IEnumerable<dynamic> actual)
        {
            var type = typeof(ComplexTestModel);

            var elementProperties = type.GetProperties()
                .Where(prop => prop.IsDefined(typeof(KontentElementIdAttribute), false))
                .ToList();

            foreach (var prop in elementProperties)
            {
                // TODO we might want to get specific elements to assert rather then iterate through them - rewrite to specific elements
                // TODO fix GetContentItemVariantUpsertModel_ReturnsExpected
                var expectedValue = expected.GetType().GetProperty(prop.Name).GetValue(expected);
                var elementId = expected.GetType().GetProperty(prop.Name)?.GetKontentElementId();
                Assert.Equal(expectedValue, actual.Single(x => x.element.id == elementId).value);
            }
        }

        private static void AssertElements(ComplexTestModel expected, ComplexTestModel actual)
        {
            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(expected.PostDate, actual.PostDate);
            Assert.Equal(expected.UrlPattern.Mode, actual.UrlPattern.Mode);
            Assert.Equal(expected.UrlPattern.Value, actual.UrlPattern.Value);
            Assert.Equal(expected.BodyCopy, actual.BodyCopy);
            AssertIdentifiers(expected.Personas?.Select(x => x.Id.Value), actual.Personas?.Select(x => x.Id.Value));
            AssertIdentifiers(expected.RelatedArticles?.Select(x => x.Id.Value), actual.RelatedArticles?.Select(x => x.Id.Value));
            AssertIdentifiers(expected.Personas?.Select(x => x.Id.Value), actual.Personas?.Select(x => x.Id.Value));

        }

        private static ComplexTestModel GetTestModel()
        {
            return new ComplexTestModel
            {
                Title = "text",
                PostDate = DateTime.Now,
                UrlPattern = new UrlSlug { Value = "urlslug", Mode = "custom" },
                BodyCopy = "RichText",
                TeaserImage = new[] { Guid.NewGuid(), Guid.NewGuid() }.Select(AssetIdentifier.ById).ToArray(),
                RelatedArticles = new[] { Guid.NewGuid(), Guid.NewGuid() }.Select(ContentItemIdentifier.ById).ToArray(),
                Personas = new[] { Guid.NewGuid(), Guid.NewGuid() }.Select(TaxonomyTermIdentifier.ById).ToList(),
            };
        }

        private IEnumerable<dynamic> ToDynamic(ComplexTestModel model)
        {
            var type = typeof(ComplexTestModel);

            var elements = new List<dynamic> {
                new
                {
                    element = new { id = type.GetProperty("Title")?.GetKontentElementId() },
                    value = model.Title
                },
                new
                {
                    element = new { id = type.GetProperty("PostDate")?.GetKontentElementId() },
                    value = model.PostDate
                },
                new
                {
                    element = new { id = type.GetProperty("UrlPattern")?.GetKontentElementId() },
                    value = model.UrlPattern.Value,
                    mode = model.UrlPattern.Mode
                },
                new
                {
                    element = new { id = type.GetProperty("BodyCopy")?.GetKontentElementId() },
                    value = model.BodyCopy
                },
                new
                {
                    element = new { id = type.GetProperty("TeaserImage")?.GetKontentElementId() },
                    value = model.TeaserImage
                },
                new
                {
                    element = new { id = type.GetProperty("RelatedArticles")?.GetKontentElementId()},
                    value = model.RelatedArticles
                },
                new
                {
                    element = new { id = type.GetProperty("Personas")?.GetKontentElementId() },
                    value = model.Personas
                }
            };

            var serialized = JsonConvert.SerializeObject(elements, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            return JsonConvert.DeserializeObject<IEnumerable<dynamic>>(serialized, new JsonSerializerSettings { Converters = new JsonConverter[] { new DynamicObjectJsonConverter() } });
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
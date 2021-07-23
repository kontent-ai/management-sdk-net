using System;
using System.Collections.Generic;
using System.Linq;
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
            var type = model.GetType();

            var upsertVariantElements = _modelProvider.GetContentItemVariantUpsertModel(model).Elements;

            var titleValue = upsertVariantElements.SingleOrDefault(elementObject =>
                 elementObject.element.id == type.GetProperty(nameof(model.Title))?.GetKontentElementId()
            ).value;

            var postDateValue = upsertVariantElements.SingleOrDefault(elementObject =>
                 elementObject.element.id == type.GetProperty(nameof(model.PostDate))?.GetKontentElementId()
            ).value;

            var urlPatternElement = upsertVariantElements.SingleOrDefault(elementObject =>
                 elementObject.element.id == type.GetProperty(nameof(model.UrlPattern))?.GetKontentElementId()
            );

            var bodyCopyValue = upsertVariantElements.SingleOrDefault(elementObject =>
                 elementObject.element.id == type.GetProperty(nameof(model.BodyCopy))?.GetKontentElementId()
            ).value;

            var teaserImage = upsertVariantElements.SingleOrDefault(elementObject =>
                elementObject.element.id == type.GetProperty(nameof(model.TeaserImage))?.GetKontentElementId()
            ).value as IEnumerable<AssetIdentifier>;

            var personaValue = upsertVariantElements.SingleOrDefault(elementObject =>
                 elementObject.element.id == type.GetProperty(nameof(model.Personas))?.GetKontentElementId()
            ).value as IEnumerable<TaxonomyTermIdentifier>;

            var relatedArticles = upsertVariantElements.SingleOrDefault(elementObject =>
                 elementObject.element.id == type.GetProperty(nameof(model.RelatedArticles))?.GetKontentElementId()
            ).value as IEnumerable<ContentItemIdentifier>;

            Assert.Equal(model.Title, titleValue);
            Assert.Equal(model.PostDate, postDateValue);
            Assert.Equal(model.UrlPattern.Value, urlPatternElement.value);
            Assert.Equal(model.UrlPattern.Mode, urlPatternElement.mode);
            Assert.Equal(model.BodyCopy, bodyCopyValue);
            AssertIdentifiers(model.TeaserImage.Select(x => x.Id.Value), teaserImage.Select(x => x.Id.Value));
            AssertIdentifiers(model.RelatedArticles.Select(x => x.Id.Value), relatedArticles.Select(x => x.Id.Value));
            AssertIdentifiers(model.Personas.Select(x => x.Id.Value), personaValue.Select(x => x.Id.Value));
        }

        private static void AssertElements(ComplexTestModel expected, ComplexTestModel actual)
        {
            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(expected.PostDate, actual.PostDate);
            Assert.Equal(expected.UrlPattern.Mode, actual.UrlPattern.Mode);
            Assert.Equal(expected.UrlPattern.Value, actual.UrlPattern.Value);
            Assert.Equal(expected.BodyCopy, actual.BodyCopy);
            AssertIdentifiers(expected.TeaserImage?.Select(x => x.Id.Value), actual.TeaserImage?.Select(x => x.Id.Value));
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
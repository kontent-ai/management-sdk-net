using System;
using System.Collections.Generic;
using System.Linq;
using Kentico.Kontent.Management.Models.Assets;
using Kentico.Kontent.Management.Modules.Extensions;
using Kentico.Kontent.Management.Models.Items;
using Kentico.Kontent.Management.Models.Items.Elements;
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
                Elements = PrepareMockDynamicResponse(expected)
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

            var ratingValue = upsertVariantElements.SingleOrDefault(elementObject =>
                 elementObject.element.id == type.GetProperty(nameof(model.Rating))?.GetKontentElementId()
            ).value;

            var selectedFormValue = upsertVariantElements.SingleOrDefault(elementObject =>
                    elementObject.element.id == type.GetProperty(nameof(model.SelectedForm))?.GetKontentElementId()
            ).value;

            var postDateValue = upsertVariantElements.SingleOrDefault(elementObject =>
                 elementObject.element.id == type.GetProperty(nameof(model.PostDate))?.GetKontentElementId()
            ).value;

            var urlPatternElement = upsertVariantElements.SingleOrDefault(elementObject =>
                 elementObject.element.id == type.GetProperty(nameof(model.UrlPattern))?.GetKontentElementId()
            );

            // var bodyCopyValue = upsertVariantElements.SingleOrDefault(elementObject =>
            //      elementObject.element.id == type.GetProperty(nameof(model.BodyCopy))?.GetKontentElementId()
            // ).value;

            var teaserImageValue = upsertVariantElements.SingleOrDefault(elementObject =>
                elementObject.element.id == type.GetProperty(nameof(model.TeaserImage))?.GetKontentElementId()
            ).value as IEnumerable<AssetIdentifier>;

            // var personaValue = upsertVariantElements.SingleOrDefault(elementObject =>
            //      elementObject.element.id == type.GetProperty(nameof(model.Personas))?.GetKontentElementId()
            // ).value as IEnumerable<TaxonomyTermIdentifier>;

            // var relatedArticles = upsertVariantElements.SingleOrDefault(elementObject =>
            //      elementObject.element.id == type.GetProperty(nameof(model.RelatedArticles))?.GetKontentElementId()
            // ).value as IEnumerable<ContentItemIdentifier>;

            Assert.Equal(model.Title.Value, titleValue);
            Assert.Equal(model.Rating.Value, ratingValue);
            Assert.Equal(model.SelectedForm.Value, selectedFormValue);
            Assert.Equal(model.PostDate.Value, postDateValue);
            Assert.Equal(model.UrlPattern.Value, urlPatternElement.value);
            Assert.Equal(model.UrlPattern.Mode, urlPatternElement.mode);
            // Assert.Equal(model.BodyCopy, bodyCopyValue);
            AssertIdentifiers(model.TeaserImage.Value.Select(x => x.Id.Value), teaserImageValue.Select(x => x.Id.Value));
            // AssertIdentifiers(model.RelatedArticles.Select(x => x.Id.Value), relatedArticles.Select(x => x.Id.Value));
            // AssertIdentifiers(model.Personas.Select(x => x.Id.Value), personaValue.Select(x => x.Id.Value));
        }

        private static void AssertElements(ComplexTestModel expected, ComplexTestModel actual)
        {
            Assert.Equal(expected.Title.Value, actual.Title.Value);
            Assert.Equal(expected.Rating.Value, actual.Rating.Value);
            Assert.Equal(expected.PostDate.Value, actual.PostDate.Value);
            Assert.Equal(expected.UrlPattern.Mode, actual.UrlPattern.Mode);
            Assert.Equal(expected.UrlPattern.Value, actual.UrlPattern.Value);
            // Assert.Equal(expected.BodyCopy, actual.BodyCopy);
            AssertIdentifiers(expected.TeaserImage?.Value?.Select(x => x.Id.Value), actual.TeaserImage?.Value.Select(x => x.Id.Value));
            // AssertIdentifiers(expected.RelatedArticles?.Select(x => x.Id.Value), actual.RelatedArticles?.Select(x => x.Id.Value));
            // AssertIdentifiers(expected.Personas?.Select(x => x.Id.Value), actual.Personas?.Select(x => x.Id.Value));
        }

        private static ComplexTestModel GetTestModel()
        {
            return new ComplexTestModel
            {
                Title = new TextElement { Value = "text" },
                Rating = new NumberElement { Value = 3.14m },
                SelectedForm = new CustomElement { Value = "{\"formId\": 42}" },
                PostDate = new DateTimeElement() { Value = new DateTime(2017, 7, 4) },
                UrlPattern = new UrlSlugElement { Value = "urlslug", Mode = "custom" },
                BodyCopy = "RichText",
                TeaserImage = new AssetElement { Value = new[] { AssetIdentifier.ById(Guid.NewGuid()), AssetIdentifier.ById(Guid.NewGuid()) } },
                RelatedArticles = new[] { Guid.NewGuid(), Guid.NewGuid() }.Select(ContentItemIdentifier.ById).ToArray(),
                Personas = new[] { Guid.NewGuid(), Guid.NewGuid() }.Select(TaxonomyTermIdentifier.ById).ToList(),
            };
        }

        private IEnumerable<dynamic> PrepareMockDynamicResponse(ComplexTestModel model)
        {
            var type = typeof(ComplexTestModel);

            var elements = new List<dynamic> {
                new
                {
                    element = new { id = type.GetProperty(nameof(ComplexTestModel.Title))?.GetKontentElementId() },
                    value = model.Title.Value
                },
                new
                {
                    element = new { id = type.GetProperty(nameof(ComplexTestModel.Rating))?.GetKontentElementId() },
                    value = model.Rating.Value
                },
                new
                {
                    element = new { id = type.GetProperty(nameof(ComplexTestModel.SelectedForm))?.GetKontentElementId() },
                    value = model.SelectedForm.Value
                },
                new
                {
                    element = new { id = type.GetProperty(nameof(ComplexTestModel.PostDate))?.GetKontentElementId() },
                    value = model.PostDate.Value
                },
                new
                {
                    element = new { id = type.GetProperty(nameof(ComplexTestModel.UrlPattern))?.GetKontentElementId() },
                    value = model.UrlPattern.Value,
                    mode = model.UrlPattern.Mode
                },
                // new
                // {
                //     element = new { id = type.GetProperty(nameof(ComplexTestModel.BodyCopy))?.GetKontentElementId() },
                //     value = model.BodyCopy
                // },
                new
                {
                    element = new { id = type.GetProperty(nameof(ComplexTestModel.TeaserImage))?.GetKontentElementId() },
                    value = model.TeaserImage.Value
                },
                // new
                // {
                //     element = new { id = type.GetProperty(nameof(ComplexTestModel.RelatedArticles))?.GetKontentElementId()},
                //     value = model.RelatedArticles
                // },
                // new
                // {
                //     element = new { id = type.GetProperty(nameof(ComplexTestModel.Personas))?.GetKontentElementId() },
                //     value = model.Personas
                // }
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
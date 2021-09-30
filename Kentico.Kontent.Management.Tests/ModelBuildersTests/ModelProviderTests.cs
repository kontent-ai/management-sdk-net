using System;
using System.Collections.Generic;
using System.Linq;
using Kentico.Kontent.Management.Models.Assets;
using Kentico.Kontent.Management.Modules.Extensions;
using Kentico.Kontent.Management.Modules.ActionInvoker;
using Kentico.Kontent.Management.Modules.ModelBuilders;
using Kentico.Kontent.Management.Tests.Data;
using Newtonsoft.Json;
using Xunit;
using Kentico.Kontent.Management.Models;
using Kentico.Kontent.Management.Models.LanguageVariants;
using Kentico.Kontent.Management.Models.Shared;
using Kentico.Kontent.Management.Models.LanguageVariants.Elements;

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
            var model = new LanguageVariantModel
            {
                Elements = PrepareMockDynamicResponse(expected)
            };
            var actual = _modelProvider.GetLanguageVariantModel<ComplexTestModel>(model).Elements;

            Assert.Equal(expected.Title.Value, actual.Title.Value);
            Assert.Equal(expected.Rating.Value, actual.Rating.Value);
            Assert.Equal(expected.PostDate.Value, actual.PostDate.Value);
            Assert.Equal(expected.UrlPattern.Mode, actual.UrlPattern.Mode);
            Assert.Equal(expected.UrlPattern.Value, actual.UrlPattern.Value);
            Assert.Equal(expected.BodyCopy.Value, actual.BodyCopy.Value);
            Assert.Single(actual.BodyCopy.Components);
            AssertIdentifiers(expected.BodyCopy.Components.Select(x => x.Id), actual.BodyCopy.Components.Select(x => x.Id));
            AssertIdentifiers(expected.BodyCopy.Components.Select(x => x.Type.Id.Value), actual.BodyCopy.Components.Select(x => x.Type.Id.Value));
            AssertIdentifiers(expected.RelatedArticles.Value.Select(x => x.Id.Value), actual.RelatedArticles.Value.Select(x => x.Id.Value));
            AssertIdentifiers(expected.TeaserImage.Value.Select(x => x.Id.Value), actual.TeaserImage.Value.Select(x => x.Id.Value));
            AssertIdentifiers(expected.Options.Value.Select(x => x.Id.Value), actual.Options.Value.Select(x => x.Id.Value));
            AssertIdentifiers(expected.Personas.Value.Select(x => x.Id.Value), actual.Personas.Value?.Select(x => x.Id.Value));
        }

        [Fact]
        public void GetContentItemVariantUpsertModel_ReturnsExpected()
        {
            var model = GetTestModel();
            var type = model.GetType();

            var upsertVariantElements = _modelProvider.GetLanguageVariantUpsertModel(model).Elements;

            var titleValue = upsertVariantElements.SingleOrDefault(elementObject =>
                 elementObject.element.id == type.GetProperty(nameof(model.Title))?.GetKontentElementId()
            ).value;

            var ratingValue = upsertVariantElements.SingleOrDefault(elementObject =>
                 elementObject.element.id == type.GetProperty(nameof(model.Rating))?.GetKontentElementId()
            ).value;

            var selectedForm = upsertVariantElements.SingleOrDefault(elementObject =>
                    elementObject.element.id == type.GetProperty(nameof(model.SelectedForm))?.GetKontentElementId()
            );

            var postDateValue = upsertVariantElements.SingleOrDefault(elementObject =>
                 elementObject.element.id == type.GetProperty(nameof(model.PostDate))?.GetKontentElementId()
            ).value;

            var urlPatternElement = upsertVariantElements.SingleOrDefault(elementObject =>
                 elementObject.element.id == type.GetProperty(nameof(model.UrlPattern))?.GetKontentElementId()
            );

            var bodyCopyElement = upsertVariantElements.SingleOrDefault(elementObject =>
                 elementObject.element.id == type.GetProperty(nameof(model.BodyCopy))?.GetKontentElementId()
            );

            var relatedArticlesValue = upsertVariantElements.SingleOrDefault(elementObject =>
                 elementObject.element.id == type.GetProperty(nameof(model.RelatedArticles))?.GetKontentElementId()
            ).value as IEnumerable<Reference>;

            var teaserImageValue = upsertVariantElements.SingleOrDefault(elementObject =>
                elementObject.element.id == type.GetProperty(nameof(model.TeaserImage))?.GetKontentElementId()
            ).value as IEnumerable<Reference>;

            var personaValue = upsertVariantElements.SingleOrDefault(elementObject =>
                 elementObject.element.id == type.GetProperty(nameof(model.Personas))?.GetKontentElementId()
            ).value as IEnumerable<Reference>;

            var optionsValue = upsertVariantElements.SingleOrDefault(elementObject =>
                 elementObject.element.id == type.GetProperty(nameof(model.Options))?.GetKontentElementId()
            ).value as IEnumerable<Reference>;

            Assert.Equal(model.Title.Value, titleValue);
            Assert.Equal(model.Rating.Value, ratingValue);
            Assert.Equal(model.SelectedForm.Value, selectedForm.value);
            Assert.Equal(model.SelectedForm.SearchableValue, selectedForm.searchable_value);
            Assert.Equal(model.PostDate.Value, postDateValue);
            Assert.Equal(model.UrlPattern.Value, urlPatternElement.value);
            Assert.Equal(model.UrlPattern.Mode, urlPatternElement.mode);
            Assert.Equal(model.BodyCopy.Value, bodyCopyElement.value);
            Assert.Single(bodyCopyElement.components as IEnumerable<ComponentModel>);
            AssertIdentifiers(model.BodyCopy.Components.Select(x => x.Id), (bodyCopyElement.components as IEnumerable<ComponentModel>)?.Select(x => x.Id));
            AssertIdentifiers(model.RelatedArticles.Value.Select(x => x.Id.Value), relatedArticlesValue.Select(x => x.Id.Value));
            AssertIdentifiers(model.TeaserImage.Value.Select(x => x.Id.Value), teaserImageValue.Select(x => x.Id.Value));
            AssertIdentifiers(model.Personas.Value.Select(x => x.Id.Value), personaValue.Select(x => x.Id.Value));
            AssertIdentifiers(model.Options.Value.Select(x => x.Id.Value), optionsValue.Select(x => x.Id.Value));
        }

        private static ComplexTestModel GetTestModel()
        {
            var componentId = Guid.NewGuid();
            var contentTypeId = Guid.NewGuid();
            return new ComplexTestModel
            {
                Title = new TextElement { Value = "text" },
                Rating = new NumberElement { Value = 3.14m },
                SelectedForm = new CustomElement
                {
                    Value = "{\"formId\": 42}",
                    SearchableValue = "Almighty form!",

                },
                PostDate = new DateTimeElement() { Value = new DateTime(2017, 7, 4) },
                UrlPattern = new UrlSlugElement { Value = "urlslug", Mode = "custom" },
                BodyCopy = new RichTextElement
                {
                    Value = $"<p>Rich Text</p><object type=\"application/kenticocloud\" data-type=\"component\" data-id=\"{componentId}\"></object>",
                    Components = new ComponentModel[]
                    {
                        new ComponentModel
                        {
                            Id = componentId,
                            Type = Reference.ById(contentTypeId),
                            Elements = new BaseElement[]
                            {
                                new TextElement { Value = "text" }
                            }
                        }
                    }
                },
                TeaserImage = new AssetElement { Value = new[] { Reference.ById(Guid.NewGuid()), Reference.ById(Guid.NewGuid()) } },
                RelatedArticles = new LinkedItemsElement { Value = new[] { Guid.NewGuid(), Guid.NewGuid() }.Select(Reference.ById).ToArray() },
                Personas = new TaxonomyElement { Value = new[] { Guid.NewGuid(), Guid.NewGuid() }.Select(Reference.ById).ToList() },
                Options = new MultipleChoiceElement { Value = new[] { Guid.NewGuid(), Guid.NewGuid() }.Select(Reference.ById).ToList() },
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
                    value = model.SelectedForm.Value,
                    searchable_value = model.SelectedForm.SearchableValue
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
                new
                {
                    element = new { id = type.GetProperty(nameof(ComplexTestModel.BodyCopy))?.GetKontentElementId() },
                    value = model.BodyCopy.Value,
                    components = model.BodyCopy.Components
                },
                new
                {
                    element = new { id = type.GetProperty(nameof(ComplexTestModel.RelatedArticles))?.GetKontentElementId()},
                    value = model.RelatedArticles.Value
                },
                new
                {
                    element = new { id = type.GetProperty(nameof(ComplexTestModel.TeaserImage))?.GetKontentElementId() },
                    value = model.TeaserImage.Value
                },
                new
                {
                    element = new { id = type.GetProperty(nameof(ComplexTestModel.Personas))?.GetKontentElementId() },
                    value = model.Personas.Value
                },
                new
                {
                    element = new { id = type.GetProperty(nameof(ComplexTestModel.Options))?.GetKontentElementId() },
                    value = model.Options.Value
                },
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
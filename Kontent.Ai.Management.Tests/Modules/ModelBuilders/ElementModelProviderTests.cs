using Kontent.Ai.Management.Models.LanguageVariants;
using Kontent.Ai.Management.Models.LanguageVariants.Elements;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Modules.ActionInvoker;
using Kontent.Ai.Management.Modules.Extensions;
using Kontent.Ai.Management.Modules.ModelBuilders;
using Kontent.Ai.Management.Tests.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Kontent.Ai.Management.Tests.Modules.ModelBuilders;

public class ElementModelProviderTests
{
    private readonly IElementModelProvider _elementModelProvider;

    public ElementModelProviderTests()
    {
        _elementModelProvider = new ElementModelProvider();
    }

    [Fact]
    public void GetStronglyTypedElements_ReturnsExpected()
    {
        var expected = GetTestModel();
        var dynamicElements = PrepareMockDynamicResponse(expected);
        var actual = _elementModelProvider.GetStronglyTypedElements<ComplexTestModel>(dynamicElements);

        Assert.Equal(expected.Title.Value, actual.Title.Value);
        Assert.Equal(expected.Rating.Value, actual.Rating.Value);
        Assert.Equal(expected.PostDate.Value, actual.PostDate.Value);
        Assert.Equal(expected.PostDate.DisplayTimeZone, actual.PostDate.DisplayTimeZone);
        Assert.Equal(expected.UrlPattern.Mode, actual.UrlPattern.Mode);
        Assert.Equal(expected.UrlPattern.Value, actual.UrlPattern.Value);
        Assert.Equal(expected.BodyCopy.Value, actual.BodyCopy.Value);
        Assert.Single(actual.BodyCopy.Components);
        AssertIdentifiers(expected.BodyCopy.Components.Select(x => x.Id), actual.BodyCopy.Components.Select(x => x.Id));
        AssertIdentifiers(expected.BodyCopy.Components.Select(x => x.Type.Id.Value), actual.BodyCopy.Components.Select(x => x.Type.Id.Value));

        AssertIdentifiers(expected.RelatedArticles.Value.Where(x => x.Id != null).Select(x => x.Id.Value), actual.RelatedArticles.Value.Where(x => x.Id != null).Select(x => x.Id.Value));
        AssertIdentifiers(expected.RelatedArticles.Value.Where(x => !string.IsNullOrEmpty(x.Codename)).Select(x => x.Codename), actual.RelatedArticles.Value.Where(x => !string.IsNullOrEmpty(x.Codename)).Select(x => x.Codename));
        AssertIdentifiers(expected.RelatedArticles.Value.Where(x => !string.IsNullOrEmpty(x.ExternalId)).Select(x => x.ExternalId), actual.RelatedArticles.Value.Where(x => !string.IsNullOrEmpty(x.ExternalId)).Select(x => x.ExternalId));

        AssertIdentifiers(expected.TeaserImage.Value.Where(x => x.Id != null).Select(x => x.Id.Value), actual.TeaserImage.Value.Where(x => x.Id != null).Select(x => x.Id.Value));
        AssertIdentifiers(expected.TeaserImage.Value.Where(x => !string.IsNullOrEmpty(x.Codename)).Select(x => x.Codename), actual.TeaserImage.Value.Where(x => !string.IsNullOrEmpty(x.Codename)).Select(x => x.Codename));
        AssertIdentifiers(expected.TeaserImage.Value.Where(x => !string.IsNullOrEmpty(x.ExternalId)).Select(x => x.ExternalId), actual.TeaserImage.Value.Where(x => !string.IsNullOrEmpty(x.ExternalId)).Select(x => x.ExternalId));

        for (var i = 0; i < expected.TeaserImage.Value.Count(); i++)
        {
            AssertIdentifiers(
                expected.TeaserImage.Value.ElementAt(i).Renditions.Select(x => x.Id.Value),
                actual.TeaserImage.Value.ElementAt(i).Renditions.Select(x => x.Id.Value));
        }

        AssertIdentifiers(expected.Personas.Value.Where(x => x.Id != null).Select(x => x.Id.Value), actual.Personas.Value.Where(x => x.Id != null).Select(x => x.Id.Value));
        AssertIdentifiers(expected.Personas.Value.Where(x => !string.IsNullOrEmpty(x.Codename)).Select(x => x.Codename), actual.Personas.Value.Where(x => !string.IsNullOrEmpty(x.Codename)).Select(x => x.Codename));
        AssertIdentifiers(expected.Personas.Value.Where(x => !string.IsNullOrEmpty(x.ExternalId)).Select(x => x.ExternalId), actual.Personas.Value.Where(x => !string.IsNullOrEmpty(x.ExternalId)).Select(x => x.ExternalId));

        AssertIdentifiers(expected.Options.Value.Where(x => x.Id != null).Select(x => x.Id.Value), actual.Options.Value.Where(x => x.Id != null).Select(x => x.Id.Value));
        AssertIdentifiers(expected.Options.Value.Where(x => !string.IsNullOrEmpty(x.Codename)).Select(x => x.Codename), actual.Options.Value.Where(x => !string.IsNullOrEmpty(x.Codename)).Select(x => x.Codename));
        AssertIdentifiers(expected.Options.Value.Where(x => !string.IsNullOrEmpty(x.ExternalId)).Select(x => x.ExternalId), actual.Options.Value.Where(x => !string.IsNullOrEmpty(x.ExternalId)).Select(x => x.ExternalId));
    }

    [Fact]
    public void GetLanguageVariantUpsertModel_ReturnsExpected()
    {
        var model = GetTestModel();
        var type = model.GetType();

        var dynamicElements = _elementModelProvider.GetDynamicElements(model);

        var title = dynamicElements.SingleOrDefault(elementObject => elementObject.element.id == type.GetProperty(nameof(model.Title))?.GetKontentElementId() );
        var titleValue = ((TextElement)TextElement.FromDynamic(title, Models.Types.Elements.ElementMetadataType.Text)).Value;

        var rating = dynamicElements.SingleOrDefault(elementObject => elementObject.element.id == type.GetProperty(nameof(model.Rating))?.GetKontentElementId());
        var ratingValue = ((NumberElement)NumberElement.FromDynamic(rating, Models.Types.Elements.ElementMetadataType.Number)).Value;

        var selectedForm = dynamicElements.SingleOrDefault(elementObject => elementObject.element.id == type.GetProperty(nameof(model.SelectedForm))?.GetKontentElementId());
        var selectedFormElement = ((CustomElement)CustomElement.FromDynamic(selectedForm, Models.Types.Elements.ElementMetadataType.Custom));

        var postDate = dynamicElements.SingleOrDefault(elementObject => elementObject.element.id == type.GetProperty(nameof(model.PostDate))?.GetKontentElementId());
        var postDateElement = ((DateTimeElement)DateTimeElement.FromDynamic(postDate, Models.Types.Elements.ElementMetadataType.DateTime));

        var urlPattern = dynamicElements.SingleOrDefault(elementObject => elementObject.element.id == type.GetProperty(nameof(model.UrlPattern))?.GetKontentElementId());
        var urlPatternElement = ((UrlSlugElement)UrlSlugElement.FromDynamic(urlPattern, Models.Types.Elements.ElementMetadataType.UrlSlug));

        var bodyCopyElement = dynamicElements.SingleOrDefault(elementObject => elementObject.element.id == type.GetProperty(nameof(model.BodyCopy))?.GetKontentElementId());

        var relatedArticles = dynamicElements.SingleOrDefault(elementObject => elementObject.element.id == type.GetProperty(nameof(model.RelatedArticles))?.GetKontentElementId());
        var relatedArticlesValue = ((LinkedItemsElement)LinkedItemsElement.FromDynamic(relatedArticles, Models.Types.Elements.ElementMetadataType.LinkedItems)).Value;

        var teaserImage = dynamicElements.SingleOrDefault(elementObject => elementObject.element.id == type.GetProperty(nameof(model.TeaserImage))?.GetKontentElementId());
        var teaserImageValue = ((AssetElement)AssetElement.FromDynamic(teaserImage, Models.Types.Elements.ElementMetadataType.Asset)).Value;

        var persona = dynamicElements.SingleOrDefault(elementObject => elementObject.element.id == type.GetProperty(nameof(model.Personas))?.GetKontentElementId());
        var personaValue = ((TaxonomyElement)TaxonomyElement.FromDynamic(persona, Models.Types.Elements.ElementMetadataType.Taxonomy)).Value;

        var options = dynamicElements.SingleOrDefault(elementObject => elementObject.element.id == type.GetProperty(nameof(model.Options))?.GetKontentElementId());
        var optionsValue = ((MultipleChoiceElement)MultipleChoiceElement.FromDynamic(options, Models.Types.Elements.ElementMetadataType.MultipleChoice)).Value;

        Assert.Equal(model.Title.Value, titleValue);
        Assert.Equal(model.Rating.Value, ratingValue);
        Assert.Equal(model.SelectedForm.Value, selectedFormElement.Value);
        Assert.Equal(model.SelectedForm.SearchableValue, selectedFormElement.SearchableValue);
        Assert.Equal(model.PostDate.Value, postDateElement.Value);
        Assert.Equal(model.PostDate.DisplayTimeZone, postDateElement.DisplayTimeZone);
        Assert.Equal(model.UrlPattern.Value, urlPatternElement.Value);
        Assert.Equal(model.UrlPattern.Mode, urlPatternElement.Mode);
        Assert.Equal(model.BodyCopy.Value, bodyCopyElement.value);
        Assert.Single(bodyCopyElement.components as IEnumerable<ComponentModel>);
        AssertIdentifiers(model.BodyCopy.Components.Select(x => x.Id), (bodyCopyElement.components as IEnumerable<ComponentModel>)?.Select(x => x.Id));

        AssertIdentifiers(model.RelatedArticles.Value.Where(x => x.Id != null).Select(x => x.Id.Value), relatedArticlesValue.Where(x => x.Id != null).Select(x => x.Id.Value));
        AssertIdentifiers(model.RelatedArticles.Value.Where(x => !string.IsNullOrEmpty(x.Codename)).Select(x => x.Codename), relatedArticlesValue.Where(x => !string.IsNullOrEmpty(x.Codename)).Select(x => x.Codename));
        AssertIdentifiers(model.RelatedArticles.Value.Where(x => !string.IsNullOrEmpty(x.ExternalId)).Select(x => x.ExternalId), relatedArticlesValue.Where(x => !string.IsNullOrEmpty(x.ExternalId)).Select(x => x.ExternalId));

        AssertIdentifiers(model.TeaserImage.Value.Where(x => x.Id != null).Select(x => x.Id.Value), teaserImageValue.Where(x => x.Id != null).Select(x => x.Id.Value));
        AssertIdentifiers(model.TeaserImage.Value.Where(x => !string.IsNullOrEmpty(x.Codename)).Select(x => x.Codename), teaserImageValue.Where(x => !string.IsNullOrEmpty(x.Codename)).Select(x => x.Codename));
        AssertIdentifiers(model.TeaserImage.Value.Where(x => !string.IsNullOrEmpty(x.ExternalId)).Select(x => x.ExternalId), teaserImageValue.Where(x => !string.IsNullOrEmpty(x.ExternalId)).Select(x => x.ExternalId));

        for (var i = 0; i < model.TeaserImage.Value.Count(); i++)
        {
            AssertIdentifiers(
                model.TeaserImage.Value.ElementAt(i).Renditions.Select(x => x.Id.Value),
                teaserImageValue.ElementAt(i).Renditions.Select(x => x.Id.Value));
        }

        AssertIdentifiers(model.Personas.Value.Where(x => x.Id != null).Select(x => x.Id.Value), personaValue.Where(x => x.Id != null).Select(x => x.Id.Value));
        AssertIdentifiers(model.Personas.Value.Where(x => !string.IsNullOrEmpty(x.Codename)).Select(x => x.Codename), personaValue.Where(x => !string.IsNullOrEmpty(x.Codename)).Select(x => x.Codename));
        AssertIdentifiers(model.Personas.Value.Where(x => !string.IsNullOrEmpty(x.ExternalId)).Select(x => x.ExternalId), personaValue.Where(x => !string.IsNullOrEmpty(x.ExternalId)).Select(x => x.ExternalId));

        AssertIdentifiers(model.Options.Value.Where(x => x.Id != null).Select(x => x.Id.Value), optionsValue.Where(x => x.Id != null).Select(x => x.Id.Value));
        AssertIdentifiers(model.Options.Value.Where(x => !string.IsNullOrEmpty(x.Codename)).Select(x => x.Codename), optionsValue.Where(x => !string.IsNullOrEmpty(x.Codename)).Select(x => x.Codename));
        AssertIdentifiers(model.Options.Value.Where(x => !string.IsNullOrEmpty(x.ExternalId)).Select(x => x.ExternalId), optionsValue.Where(x => !string.IsNullOrEmpty(x.ExternalId)).Select(x => x.ExternalId));
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
            PostDate = new DateTimeElement() { Value = new DateTime(2017, 7, 4), DisplayTimeZone = "Australia/Sydney" },
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
            TeaserImage = new AssetElement
            {
                Value =
                [
                    new AssetWithRenditionsReference(Reference.ById(Guid.NewGuid()), [Reference.ById(Guid.NewGuid())]),
                    new AssetWithRenditionsReference(Reference.ById(Guid.NewGuid()), [Reference.ById(Guid.NewGuid()), Reference.ById(Guid.NewGuid()),]),
                    new AssetWithRenditionsReference(Reference.ById(Guid.NewGuid()), []),
                    new AssetWithRenditionsReference(Reference.ByCodename("asset-with-rendition"), []),
                    new AssetWithRenditionsReference(Reference.ByExternalId("asset-with-external-id"), []),
                ]
            },
            RelatedArticles = new LinkedItemsElement 
            {
                Value =
                [
                    Reference.ById(Guid.NewGuid()),
                    Reference.ById(Guid.NewGuid()),
                    Reference.ByCodename("related-item-by-codename"),
                    Reference.ByExternalId("related-item-by-external-id")
                ]
            },
            Personas = new TaxonomyElement 
            { 
                Value = 
                [
                    Reference.ById(Guid.NewGuid()),
                    Reference.ById(Guid.NewGuid()),
                    Reference.ByCodename("taxonomy-item-by-codename"),
                    Reference.ByExternalId("taxonomy-item-by-external-id")
                ]
            },
            Options = new MultipleChoiceElement
            {
                Value =
                [
                    Reference.ById(Guid.NewGuid()),
                    Reference.ById(Guid.NewGuid()),
                    Reference.ByCodename("multi-choice-item-by-codename"),
                    Reference.ByExternalId("multi-choice-item-by-external-id")
                ]
            },
        };
    }

    private static IEnumerable<dynamic> PrepareMockDynamicResponse(ComplexTestModel model)
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
                value = model.PostDate.Value,
                display_timezone = model.PostDate.DisplayTimeZone
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

    private static void AssertIdentifiers<T>(IEnumerable<T> expected, IEnumerable<T> actual)
    {
        if (expected == null && actual == null)
        {
            return;
        }

        if (expected == null || actual == null)
        {
            Assert.True(false, "Null check");
        }
        Assert.Equal(expected, actual);
    }
}
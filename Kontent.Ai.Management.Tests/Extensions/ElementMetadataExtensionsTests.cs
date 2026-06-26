using AwesomeAssertions;
using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.Types;
using Kontent.Ai.Management.Models.Types.Elements;

namespace Kontent.Ai.Management.Tests.Extensions;

public class ElementMetadataExtensionsTests
{
    [Fact]
    public void ToElement_AllElementsArePresent()
    {
        var elements = GetElementMetadata();

        var types = typeof(ElementMetadataBase).Assembly
            .GetTypes()
            .Where(t => !t.IsAbstract && t.IsClass && t.IsSubclassOf(typeof(ElementMetadataBase)));

        elements.Select(x => x.GetType()).Should().BeEquivalentTo(types, $"Please make sure that every content type element is created in method {nameof(GetElementMetadata)}");
    }

    [Fact]
    public void ToElement_MatchingType_ReturnsSameInstance()
    {
        ElementMetadataBase element = new TextElementMetadataModel { Name = "text" };

        element.ToElement<TextElementMetadataModel>().Should().BeSameAs(element);
    }

    [Fact]
    public void ToElement_MismatchedType_Throws()
    {
        ElementMetadataBase element = new TextElementMetadataModel { Name = "text" };

        element.Invoking(e => e.ToElement<NumberElementMetadataModel>())
            .Should().Throw<InvalidOperationException>();
    }

    public static List<ElementMetadataBase> GetElementMetadata() => new()
    {
        new AssetElementMetadataModel
        {
            Name = "MyAsset",
            AllowedFileTypes = FileType.Any,
            Codename = ""
        },
        new CustomElementMetadataModel
        {
            Name= "name",
            Codename = "codename",
            Guidelines = "guidelines",
            JsonParameters = "das,adas",
            SourceUrl = "https://example.com/custom-element",
        },
        new ContentTypeSnippetElementMetadataModel
        {
            Codename = "contenttypesnippet_codename",
            ExternalId = "contentTypeSnippet_externalId",
            SnippetIdentifier = Reference.ByCodename("codename"),
        },
        new DateTimeElementMetadataModel
        {
            Codename = "datetimeelement_codename",
            ExternalId = "DateTimeElementSnippet_externalId",
            IsRequired = false,
            Name = "DateTimename",
        },
        new GuidelinesElementMetadataModel
        {
            Codename = "guidelines_codename",
            ExternalId = "guidelines_external_id",
            Guidelines = "<h3>Guidelines</h3>",
        },
        new LinkedItemsElementMetadataModel
        {
            Codename = "linkeditemselementcodename",
            IsRequired = true,
            ItemCountLimit = new LimitModel { Value = 10, Condition = LimitType.AtMost },
            Name = "LinkedItemsElementName",
        },
        new MultipleChoiceElementMetadataModel
        {
            Name = "Is special Delivery",
            Codename = "multiple_choice_element_codename",
            IsRequired = false,
            Mode = MultipleChoiceMode.Single,
            Options = new[] {
                        new MultipleChoiceOptionModel
                        {
                            Name = "Yes",
                            Codename = "yes"
                        }
                    },
        },
        new NumberElementMetadataModel
        {
            Codename = "numberrlementcodename",
            ExternalId = "NumberElementexternal_id",
            Guidelines = "<h3>NumberElement</h3>",
            Name = "NumberElementName",
        },
        new RichTextElementMetadataModel
        {
            Codename = "richtextelementcodename",
            ExternalId = "RichTextElementexternal_id",
            Guidelines = "<h3>RichTextElement</h3>",
            Name = "RichTextElementName",
        },
        new TaxonomyElementMetadataModel
        {
            Codename = "taxonomyelementcodename",
            ExternalId = "TaxonomyElementMetadata_id",
            Guidelines = "<h3>TaxonomyElement</h3>",
            TaxonomyGroup = Reference.ById(Guid.Empty),
        },
        new TextElementMetadataModel
        {
            Codename = "textelementmetadatacodename",
            Name = "TextElementMetadataName",
            IsRequired = false,
            ValidationRegex = new ValidationRegexModel
            {
                IsActive = true,
                Regex = "^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\\s./0-9]*$",
                Flags = "i",
                ValidationMessage = "Type a value matching the pattern required in this element."
            }
        },
        new UrlSlugElementMetadataModel
        {
            Codename = "urlslugrlementcodename",
            Name = "UrlSlugElementMetadataName",
            IsRequired = false,
            ValidationRegex = new ValidationRegexModel
            {
                IsActive = true,
                Regex = "^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\\s./0-9]*$",
                Flags = "i",
                ValidationMessage = "Type a value matching the pattern required in this element."
            },
            DependsOn = new UrlSlugDependency { Element = Reference.ByCodename("textelementmetadatacodename") },
        },
        new SubpagesElementMetadataModel
        {
            Codename = "subpagescodename",
            Name = "SubpagesElementName",
            IsRequired = false,
            ItemCountLimit = new LimitModel { Value = 2, Condition = LimitType.AtMost },
            AllowedContentTypes = new[] { Reference.ById(Guid.Empty) }
        }
    };
}

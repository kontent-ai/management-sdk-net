using FluentAssertions;
using Kentico.Kontent.Management.Extensions;
using Kentico.Kontent.Management.Models.Shared;
using Kentico.Kontent.Management.Models.Types;
using Kentico.Kontent.Management.Models.Types.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Kentico.Kontent.Management.Tests.Extensions;

public class ElementMetadataExtensionTests
{
    [Fact]
    public void ToElement_AllElementsArePresent()
    {
        var elements = GetElementMetadata();

        var types = typeof(ElementMetadataBase).Assembly
            .GetTypes()
            .Where(t => !t.IsAbstract && t.IsClass && t.IsSubclassOf(typeof(ElementMetadataBase))).Distinct();

        elements.Select(x => x.GetType()).Should().BeEquivalentTo(types, $"Please make sure that every content type element is created in method {nameof(GetElementMetadata)}");
    }

    [Fact]
    public void ToElement_AllElementsCasted()
    {
        var elements = GetElementMetadata();

        foreach (var element in elements)
        {
            Action action = element switch
            {
                TextElementMetadataModel e => () => { var result = e.ToElement<TextElementMetadataModel>(); result.Should().NotBeNull(); },
                NumberElementMetadataModel e => () => { var result = e.ToElement<NumberElementMetadataModel>(); result.Should().NotBeNull(); },
                AssetElementMetadataModel e => () => { var result = e.ToElement<AssetElementMetadataModel>(); result.Should().NotBeNull(); },
                CustomElementMetadataModel e => () => { var result = e.ToElement<CustomElementMetadataModel>(); result.Should().NotBeNull(); },
                ContentTypeSnippetElementMetadataModel e => () => { var result = e.ToElement<ContentTypeSnippetElementMetadataModel>(); result.Should().NotBeNull(); },
                DateTimeElementMetadataModel e => () => { var result = e.ToElement<DateTimeElementMetadataModel>(); result.Should().NotBeNull(); },
                GuidelinesElementMetadataModel e => () => { var result = e.ToElement<GuidelinesElementMetadataModel>(); result.Should().NotBeNull(); },
                LinkedItemsElementMetadataModel e => () => { var result = e.ToElement<LinkedItemsElementMetadataModel>(); result.Should().NotBeNull(); },
                MultipleChoiceElementMetadataModel e => () => { var result = e.ToElement<MultipleChoiceElementMetadataModel>(); result.Should().NotBeNull(); },
                RichTextElementMetadataModel e => () => { var result = e.ToElement<RichTextElementMetadataModel>(); result.Should().NotBeNull(); },
                TaxonomyElementMetadataModel e => () => { var result = e.ToElement<TaxonomyElementMetadataModel>(); result.Should().NotBeNull(); },
                UrlSlugElementMetadataModel e => () => { var result = e.ToElement<UrlSlugElementMetadataModel>(); result.Should().NotBeNull(); },
                SubpagesElementMetadataModel e => () => { var result = e.ToElement<SubpagesElementMetadataModel>(); result.Should().NotBeNull(); },
                _ => throw new Exception("There is content type element that is not tested")
            };

            action.Should().NotThrow();
        }
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
                    JsonParameters = "das,adas"
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
                    ItemCountLimit = new LimitModel {Value = 2, Condition = LimitType.AtMost},
                    AllowedContentTypes = new[] { Reference.ById(Guid.Empty) }
                }
            };
}

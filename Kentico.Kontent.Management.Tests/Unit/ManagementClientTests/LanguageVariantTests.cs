using Kentico.Kontent.Management.Models.LanguageVariants;
using Kentico.Kontent.Management.Models.LanguageVariants.Elements;
using Kentico.Kontent.Management.Models.Shared;
using Kentico.Kontent.Management.Tests.Unit.Base;
using Kentico.Kontent.Management.Tests.Unit.Data;
using Kentico.Kontent.Management.Modules.Extensions;
using Kentico.Kontent.Management.Models.StronglyTyped;
using System;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using System.Dynamic;
using System.Collections.Generic;
using System.Linq;

namespace Kentico.Kontent.Management.Tests.Unit.ManagementClientTests
{
    public class LanguageVariantTests : IClassFixture<FileSystemFixture>
    {
        private FileSystemFixture _fileSystemFixture;

        public LanguageVariantTests(FileSystemFixture fileSystemFixture)
        {
            _fileSystemFixture = fileSystemFixture;
            _fileSystemFixture.SetSubFolder("LanguageVariant");
        }

        [Fact]
        public async Task GetStronglyTypedLanguageVariantAsync_ById_LanguageId_GetVariant()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("LanguageVariant.json");

            var expected = GetExpectedComplexTestModel();

            var itemIdentifier = Reference.ById(Guid.Parse("4b628214-e4fe-4fe0-b1ff-955df33e1515"));
            var languageIdentifier = Reference.ById(Guid.Parse("78dbefe8-831b-457e-9352-f4c4eacd5024"));
            var identifier = new LanguageVariantIdentifier(itemIdentifier, languageIdentifier);

            var response = await client.GetLanguageVariantAsync<ComplexTestModel>(identifier);

            response.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task UpsertVariant_ById_LanguageId_UpdatesVariant()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("LanguageVariant.json");

            var expected = GetExpectedLanguageVariantModel();

            var upsertModel = new LanguageVariantUpsertModel { Elements = expected.Elements };

            var itemIdentifier = Reference.ById(Guid.Parse("4b628214-e4fe-4fe0-b1ff-955df33e1515"));
            var languageIdentifier = Reference.ById(Guid.Parse("78dbefe8-831b-457e-9352-f4c4eacd5024"));
            var identifier = new LanguageVariantIdentifier(itemIdentifier, languageIdentifier);

            var response = await client.UpsertLanguageVariantAsync(identifier, upsertModel);

            response.Should().BeEquivalentTo(expected);
        }

        private static LanguageVariantModel GetExpectedLanguageVariantModel()
        {
            var complexModel = GetExpectedComplexTestModel().Elements;

            return new LanguageVariantModel
            {
                Item = Reference.ById(Guid.Parse("4b628214-e4fe-4fe0-b1ff-955df33e1515")),
                Language = Reference.ById(Guid.Parse("78dbefe8-831b-457e-9352-f4c4eacd5024")),
                LastModified = DateTimeOffset.Parse("2021-11-06T13:57:26.7069564Z").UtcDateTime,
                WorkflowStep = Reference.ById(Guid.Parse("eee6db3b-545a-4785-8e86-e3772c8756f9")),
                Elements = new dynamic[]
                {
                    GetRichTextAsDynamic(complexModel.BodyCopy.Element.Id, complexModel.BodyCopy.Value, complexModel.BodyCopy.Components),
                    GetElementAsDynamic(complexModel.MetaDescription.Element.Id, complexModel.MetaDescription.Value),
                    GetElementAsDynamic(complexModel.MetaKeywords.Element.Id, complexModel.MetaKeywords.Value),
                    GetArrayElementAsDynamic(complexModel.Options.Element.Id, complexModel.Options.Value),
                    GetArrayElementAsDynamic(complexModel.Personas.Element.Id, complexModel.Personas.Value),
                    GetElementAsDynamic(complexModel.PostDate.Element.Id, complexModel.PostDate.Value),
                    GetArrayElementAsDynamic(complexModel.RelatedArticles.Element.Id, complexModel.RelatedArticles.Value),
                    GetElementAsDynamic(complexModel.Rating.Element.Id, complexModel.Rating.Value),
                    GetCustomElementAsDynamic(complexModel.SelectedForm.Element.Id, complexModel.SelectedForm.Value, complexModel.SelectedForm.SearchableValue),
                    GetElementAsDynamic(complexModel.Summary.Element.Id, complexModel.Summary.Value),
                    GetArrayElementAsDynamic(complexModel.TeaserImage.Element.Id, complexModel.TeaserImage.Value),
                    GetElementAsDynamic(complexModel.Title.Element.Id, complexModel.Title.Value),
                    GetUrlSlugAsDynamic(complexModel.UrlPattern.Element.Id, complexModel.UrlPattern.Value, complexModel.UrlPattern.Mode),
                    GetArrayElementAsDynamic(complexModel.Cafe.Element.Id, complexModel.Cafe.Value),
                }
            };
        }

        private static LanguageVariantModel<ComplexTestModel> GetExpectedComplexTestModel() => new()
        {
            Item = Reference.ById(Guid.Parse("4b628214-e4fe-4fe0-b1ff-955df33e1515")),
            Language = Reference.ById(Guid.Parse("78dbefe8-831b-457e-9352-f4c4eacd5024")),
            LastModified = DateTimeOffset.Parse("2021-11-06T13:57:26.7069564Z").UtcDateTime,
            WorkflowStep = Reference.ById(Guid.Parse("eee6db3b-545a-4785-8e86-e3772c8756f9")),
            Elements = new()
            {
                BodyCopy = new RichTextElement
                {
                    Element = Reference.ById(typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.BodyCopy)).GetKontentElementId()),
                    Value = $"<h1>Light Roasts</h1><p>Usually roasted for 6 - 8 minutes or simply until achieving a light brown color.This method is used for milder coffee varieties and for coffee tasting.This type of roasting allows the natural characteristics of each coffee to show.The aroma of coffees produced from light roasts is usually more intense.The cup itself is more acidic and the concentration of caffeine is higher.</p><object type=\"application/kenticocloud\" data-type=\"component\" data-id=\"{Guid.Parse("46c05bd9-d418-4507-836c-9accc5a39db3")}\"></object>",
                    Components = new ComponentModel[]
                {
                    new ComponentModel
                    {
                        Id = Guid.Parse("46c05bd9-d418-4507-836c-9accc5a39db3"),
                        Type = Reference.ById(Guid.Parse("17ff8a28-ebe6-5c9d-95ea-18fe1ff86d2d")),
                        Elements = GetComponentElementsAsDynamic()
                    }
                }
                },
                MetaDescription = new TextElement
                {
                    Element = Reference.ById(typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.MetaDescription)).GetKontentElementId()),
                    Value = "MetaDescription"
                },
                MetaKeywords = new TextElement
                {
                    Element = Reference.ById(typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.MetaKeywords)).GetKontentElementId()),
                    Value = "MetaKeywords"
                },
                Options = new MultipleChoiceElement
                {
                    Element = Reference.ById(typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.Options)).GetKontentElementId()),
                    Value = new[]
                {
                    Reference.ById(Guid.Parse("00c0f86a-7c51-4e60-abeb-a150e9092e53")),
                    Reference.ById(Guid.Parse("8972dc90-ae2e-416e-995d-95df6c77e3b2"))
                }
                },
                Personas = new TaxonomyElement
                {
                    Element = Reference.ById(typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.Personas)).GetKontentElementId()),
                    Value = new[] { Reference.ById(Guid.Parse("6e8b18d5-c5e3-5fc1-9014-44c18ef5f5d8")) }
                },
                PostDate = new DateTimeElement
                {
                    Element = Reference.ById(typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.PostDate)).GetKontentElementId()),
                    Value = new DateTime(2017, 7, 4)
                },
                RelatedArticles = new LinkedItemsElement
                {
                    Element = Reference.ById(typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.RelatedArticles)).GetKontentElementId()),
                    Value = new[] { Reference.ById(Guid.Parse("4b628214-e4fe-4fe0-b1ff-955df33e1515")) }
                },
                Rating = new NumberElement
                {
                    Value = 3.14m,
                    Element = Reference.ById(typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.Rating)).GetKontentElementId()),
                },
                SelectedForm = new CustomElement
                {
                    Element = Reference.ById(typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.SelectedForm)).GetKontentElementId()),
                    Value = "{\"formId\": 42}",
                    SearchableValue = "Almighty form!"
                },
                Summary = new TextElement
                {
                    Element = Reference.ById(typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.Summary)).GetKontentElementId()),
                    Value = "Summary"
                },
                TeaserImage = new AssetElement
                {
                    Element = Reference.ById(typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.TeaserImage)).GetKontentElementId()),
                    Value = new[] { Reference.ById(Guid.Parse("5c08a538-5b58-44eb-81ef-43fb37eeb815")) },
                },
                Title = new TextElement
                {
                    Element = Reference.ById(typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.Title)).GetKontentElementId()),
                    Value = "On Roasts"
                },
                UrlPattern = new UrlSlugElement
                {
                    Element = Reference.ById(typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.UrlPattern)).GetKontentElementId()),
                    Value = "on-roasts",
                    Mode = "custom"
                },
                Cafe = new SubpagesElement
                {
                    Element = Reference.ById(typeof(ComplexTestModel).GetProperty(nameof(ComplexTestModel.Cafe)).GetKontentElementId()),
                    Value = new[] { Reference.ById(Guid.Parse("4b628214-e4fe-4fe0-b1ff-955df33e1515")) }
                }
            }
        };

        private static dynamic GetRichTextAsDynamic(Guid? elementId, string value, IEnumerable<ComponentModel> compoments)
        {
            dynamic element = new ExpandoObject();
            element.element = GetElement(elementId.Value.ToString("d"));
            element.value = value;
            element.components = compoments.Select(x => GetRichTextComponentAsDynamic(x));

            return element;
        }

        private static dynamic GetRichTextComponentAsDynamic(ComponentModel component)
        {
            dynamic dynamicComponent = new ExpandoObject();
            dynamicComponent.id = component.Id.ToString("d");
            dynamicComponent.type = GetElement(component.Type.Id.ToString());
            dynamicComponent.elements = component.Elements;

            return dynamicComponent;
        }

        private static dynamic[] GetComponentElementsAsDynamic()
        {
            dynamic component1 = new ExpandoObject();
            component1.element = GetElement(typeof(TweetTestModel).GetProperty(nameof(TweetTestModel.TweetLink)).GetKontentElementId().ToString()); ;
            component1.value = "https://twitter.com/ChrastinaOndrej/status/1417105245935706123";

            dynamic component2 = new ExpandoObject();
            component2.element = GetElement(typeof(TweetTestModel).GetProperty(nameof(TweetTestModel.Theme)).GetKontentElementId().ToString());
            component2.value = new[] { GetElement("061e69f7-0965-5e37-97bc-29963cfaebe8") };

            dynamic component3 = new ExpandoObject();
            component3.element = GetElement(typeof(TweetTestModel).GetProperty(nameof(TweetTestModel.DisplayOptions)).GetKontentElementId().ToString());
            component3.value = new[] { GetElement("dd78b09e-4337-599c-9701-20a0a165c63b") };

            return new dynamic[] { component1, component2, component3 };
        }

        private static dynamic GetArrayElementAsDynamic(Guid? elementId, IEnumerable<Reference> value)
        {
            dynamic element = new ExpandoObject();
            element.element = GetElement(elementId.Value.ToString("d"));
            element.value = value.Select(x => GetElement(x.Id.Value.ToString("d")));

            return element;
        }

        private static dynamic GetElementAsDynamic(Guid? elementId, dynamic value)
        {
            dynamic element = new ExpandoObject();
            element.element = GetElement(elementId.Value.ToString("d"));
            element.value = value;

            return element;
        }

        private static dynamic GetCustomElementAsDynamic(Guid? elementId, string value, string searchableValue)
        {
            dynamic element = new ExpandoObject();
            element.element = GetElement(elementId.Value.ToString("d"));
            element.value = value;
            element.searchable_value = searchableValue;

            return element;
        }

        private static dynamic GetUrlSlugAsDynamic(Guid? elementId, string value, string mode)
        {
            dynamic element = new ExpandoObject();
            element.element = GetElement(elementId.Value.ToString("d"));
            element.value = value;
            element.mode = mode;

            return element;
        }

        private static dynamic GetElement(string elementId)
        {
            dynamic element = new ExpandoObject();
            element.id = elementId;
            return element;
        }
    }
}

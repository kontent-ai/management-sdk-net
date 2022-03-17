using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Kentico.Kontent.Management.Models.LanguageVariants;
using Kentico.Kontent.Management.Models.LanguageVariants.Elements;
using Kentico.Kontent.Management.Models.Shared;
using Kentico.Kontent.Management.Modules.Extensions;

namespace Kentico.Kontent.Management.Tests.Data
{
    /// <summary>
    /// Represents dynamic and strongly typed objects of element data located in './ElementsData/ElementsData.json'.
    /// </summary>
    internal static class ElementsData
    {
        public static dynamic[] GetExpectedDynamicElements()
        {
            var complexModel = GetExpectedStronglyTypedElementsModel();

            return new[]
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
            };
        }

        public static ComplexTestModel GetExpectedStronglyTypedElementsModel() => new()
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
                Value = new[]
                {
                    new AssetWithRenditionsReference(Reference.ById(Guid.Parse("5c08a538-5b58-44eb-81ef-43fb37eeb815")), Array.Empty<Reference>()),
                    new AssetWithRenditionsReference(Reference.ById(Guid.Parse("39c947ab-78ee-4de0-9bbd-8b79008111cc")), new []
                    {
                        Reference.ById(Guid.Parse("043d8f8b-22cb-4322-a1de-8a96c57548a3")), 
                        Reference.ById(Guid.Parse("7538b9b1-bb5f-493e-b9ab-24578e2a55f5")), 
                    }),
                },
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
        
        private static dynamic GetArrayElementAsDynamic(Guid? elementId, IEnumerable<AssetWithRenditionsReference> value)
        {
            dynamic element = new ExpandoObject();
            element.element = GetElement(elementId.Value.ToString("d"));
            element.value = value.Select(GetAssetWithRenditionsAsDynamic);

            dynamic GetAssetWithRenditionsAsDynamic(AssetWithRenditionsReference reference)
            {
                dynamic result = new ExpandoObject();
                result.id = reference.Id.Value.ToString("d");
                result.renditions = reference.Renditions.Select(x => GetElement(x.Id.Value.ToString("d")));

                return result;
            }

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
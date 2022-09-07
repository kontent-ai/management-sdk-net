using System;
using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.Types.Elements;

namespace Kontent.Ai.Management.Models.LanguageVariants.Elements;

/// <summary>
/// Represents the base class of an element in the language variant.
/// </summary>
public abstract class BaseElement
{
    /// <summary>
    /// Gets or sets the value of the element.
    /// </summary>
    [JsonProperty("element", Required = Required.Always)]
    public Reference Element { get; set; }

    /// <summary>
    /// Transforms the element in a language variant to the dynamic object.
    /// </summary>
    public abstract dynamic ToDynamic();

    /// <summary>
    /// Transforms the dynamic object to the element.
    /// </summary>
    public static BaseElement FromDynamic(dynamic source, ElementMetadataType elementType) => elementType switch
    {
        ElementMetadataType.Text => FromDynamic(source, typeof(TextElement)),
        ElementMetadataType.RichText => FromDynamic(source, typeof(RichTextElement)),
        ElementMetadataType.Number => FromDynamic(source, typeof(NumberElement)),
        ElementMetadataType.MultipleChoice => FromDynamic(source, typeof(MultipleChoiceElement)),
        ElementMetadataType.DateTime => FromDynamic(source, typeof(DateTimeElement)),
        ElementMetadataType.Asset => FromDynamic(source, typeof(AssetElement)),
        ElementMetadataType.LinkedItems => FromDynamic(source, typeof(LinkedItemsElement)),
        ElementMetadataType.Taxonomy => FromDynamic(source, typeof(TaxonomyElement)),
        ElementMetadataType.UrlSlug => FromDynamic(source, typeof(UrlSlugElement)),
        ElementMetadataType.Custom => FromDynamic(source, typeof(CustomElement)),
        ElementMetadataType.Subpages => FromDynamic(source, typeof(SubpagesElement)),
        _ => throw new InvalidOperationException("Unknown element type"),
    };

    /// <summary>
    /// Transforms the dynamic object to the element.
    /// </summary>
    public static BaseElement FromDynamic(dynamic source, Type type)
    {
        ValidateSource(source);

        try
        {
            if (type == typeof(TextElement))
            {
                return new TextElement
                {
                    Element = Reference.FromDynamic(source.element),
                    Value = source?.value,
                };
            }
            else if (type == typeof(NumberElement))
            {
                return new NumberElement
                {
                    Element = Reference.FromDynamic(source.element),
                    Value = Convert.ToDecimal(source.value),
                };
            }
            else if (type == typeof(RichTextElement))
            {
                return new RichTextElement
                {
                    Element = Reference.FromDynamic(source.element),
                    Value = source.value,
                    Components = (source.components as IEnumerable<dynamic>)?.Select(component => new ComponentModel
                    {
                        Id = Guid.Parse(component.id),
                        Type = Reference.ById(Guid.Parse(component.type.id)),
                        Elements = component.elements as IEnumerable<dynamic>
                    })
                };
            }
            else if (type == typeof(AssetElement))
            {
                return new AssetElement
                {
                    Element = Reference.FromDynamic(source.element),
                    Value = (source.value as IEnumerable<dynamic>)?
                        .Select(assetWithRenditionsReferences =>
                            new AssetWithRenditionsReference(
                                Reference.ById(Guid.Parse(assetWithRenditionsReferences.id)),
                                (assetWithRenditionsReferences.renditions as IEnumerable<dynamic>)?.Select<dynamic, Reference>(renditionIdentifier => Reference.ById(Guid.Parse(renditionIdentifier.id))))),
                };
            }
            else if (type == typeof(DateTimeElement))
            {
                return new DateTimeElement
                {
                    Element = Reference.FromDynamic(source.element),
                    Value = Convert.ToDateTime(source.value),
                    DisplayTimeZone = source.display_timezone?.ToString()
                };
            }
            else if (type == typeof(LinkedItemsElement))
            {
                return new LinkedItemsElement
                {
                    Element = Reference.FromDynamic(source.element),
                    Value = (source.value as IEnumerable<dynamic>)?.Select<dynamic, Reference>(identifier => Reference.ById(Guid.Parse(identifier.id)))
                };
            }
            else if (type == typeof(MultipleChoiceElement))
            {
                return new MultipleChoiceElement
                {
                    Element = Reference.FromDynamic(source.element),
                    Value = (source.value as IEnumerable<dynamic>)?.Select<dynamic, Reference>(identifier => Reference.ById(Guid.Parse(identifier.id)))
                };
            }
            else if (type == typeof(TaxonomyElement))
            {
                return new TaxonomyElement
                {
                    Element = Reference.FromDynamic(source.element),
                    Value = (source.value as IEnumerable<dynamic>)?.Select<dynamic, Reference>(identifier => Reference.ById(Guid.Parse(identifier.id)))
                };
            }
            else if (type == typeof(UrlSlugElement))
            {
                return new UrlSlugElement
                {
                    Element = Reference.FromDynamic(source.element),
                    Mode = source.mode?.ToString(),
                    Value = source.value?.ToString()
                };
            }
            else if (type == typeof(CustomElement))
            {
                return new CustomElement
                {
                    Element = Reference.FromDynamic(source.element),
                    Value = source.value?.ToString(),
                    SearchableValue = source.searchable_value?.ToString()
                };
            }
            else if (type == typeof(SubpagesElement))
            {
                return new SubpagesElement
                {
                    Element = Reference.FromDynamic(source.element),
                    Value = (source.value as IEnumerable<dynamic>)?.Select<dynamic, Reference>(identifier => Reference.ById(Guid.Parse(identifier.id)))
                };
            }
        }
        catch (Exception exception)
        {
            throw new DataMisalignedException(
                "Object could not be converted to the strongly-typed element. Please check if it has expected properties with expected type",
                exception);
        }

        throw new ArgumentOutOfRangeException($"{type} is not a valid element");
    }

    private static void ValidateSource(dynamic source)
    {
        if (source == null || !DynamicExtensions.HasProperty(source, "element") || !DynamicExtensions.HasProperty(source, "value"))
        {
            throw new DataMisalignedException("Element does not contain reference or value.");
        }
    }
}

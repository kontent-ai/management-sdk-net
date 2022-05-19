using System;
using Kentico.Kontent.Management.Models.Shared;
using Kentico.Kontent.Management.Models.Types.Elements;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Kentico.Kontent.Management.Models.LanguageVariants.Elements;

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
        if (type == typeof(TextElement))
        {
            return new TextElement
            {
                // TODO extend by codename + external ID
                // check with ondrej if really needed as it does not make sense
                Element = Reference.ById(Guid.Parse(source?.element?.id)),
                Value = source?.value,
            };
        }
        else if (type == typeof(NumberElement))
        {
            return new NumberElement
            {
                Element = Reference.ById(Guid.Parse(source?.element?.id)),
                Value = Convert.ToDecimal(source?.value),
            };
        }
        else if (type == typeof(RichTextElement))
        {
            return new RichTextElement
            {
                Element = Reference.ById(Guid.Parse(source?.element?.id)),
                Value = source?.value,
                Components = (source?.components as IEnumerable<dynamic>)?.Select(component => new ComponentModel
                {
                    Id = Guid.Parse(component.id),
                    Type = Reference.ById(Guid.Parse(component.type.id)),
                    Elements = (component.elements as IEnumerable<dynamic>)
                })
            };
        }
        else if (type == typeof(AssetElement))
        {
            return new AssetElement
            {
                Element = Reference.ById(Guid.Parse(source?.element?.id)),
                Value = (source?.value as IEnumerable<dynamic>)?
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
                Element = Reference.ById(Guid.Parse(source?.element?.id)),
                Value = Convert.ToDateTime(source?.value)
            };
        }
        else if (type == typeof(LinkedItemsElement))
        {
            return new LinkedItemsElement
            {
                Element = Reference.ById(Guid.Parse(source?.element?.id)),
                Value = (source?.value as IEnumerable<dynamic>)?.Select<dynamic, Reference>(identifier => Reference.ById(Guid.Parse(identifier.id)))
            };
        }
        else if (type == typeof(MultipleChoiceElement))
        {
            return new MultipleChoiceElement
            {
                Element = Reference.ById(Guid.Parse(source?.element?.id)),
                Value = (source?.value as IEnumerable<dynamic>).Select<dynamic, Reference>(identifier => Reference.ById(Guid.Parse(identifier.id)))
            };
        }
        else if (type == typeof(TaxonomyElement))
        {
            return new TaxonomyElement
            {
                Element = Reference.ById(Guid.Parse(source?.element?.id)),
                Value = (source?.value as IEnumerable<dynamic>)?.Select<dynamic, Reference>(identifier => Reference.ById(Guid.Parse(identifier.id)))
            };
        }
        else if (type == typeof(UrlSlugElement))
        {
            return new UrlSlugElement
            {
                Element = Reference.ById(Guid.Parse(source?.element?.id)),
                Mode = source?.mode?.ToString(),
                Value = source?.value?.ToString()
            };
        }
        else if (type == typeof(CustomElement))
        {
            return new CustomElement
            {
                Element = Reference.ById(Guid.Parse(source?.element?.id)),
                Value = source?.value?.ToString(),
                SearchableValue = source?.searchable_value?.ToString()
            };
        }
        else if (type == typeof(SubpagesElement))
        {
            return new SubpagesElement
            {
                Element = Reference.ById(Guid.Parse(source?.element?.id)),
                Value = (source?.value as IEnumerable<dynamic>)?.Select<dynamic, Reference>(identifier => Reference.ById(Guid.Parse(identifier.id)))
            };
        }

        throw new ArgumentOutOfRangeException($"{type} is not a valid element");
    }

    /// <summary>
    /// Get dynamic representation of the element reference.
    /// </summary>
    protected dynamic GetDynamicReference()
    {
        if (Element.Id != null)
        {
            return new
            {
                id = Element.Id,
            };
        }

        if (Element.Codename != null)
        {
            return new
            {
                codename = Element.Codename,
            };
        }

        if (Element.ExternalId != null)
        {
            return new
            {
                external_id = Element.ExternalId,
            };
        }

        throw new DataMisalignedException("Element reference does not contain any identifier.");
    }
}

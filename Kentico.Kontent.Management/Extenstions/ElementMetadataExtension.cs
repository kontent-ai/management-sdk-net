using Kentico.Kontent.Management.Models.Types.Elements;
using System;

namespace Kentico.Kontent.Management.Extenstions
{
    /// <summary>
    /// Extensions methods for the ElementMetadataBase class.
    /// </summary>
    public static class ElementMetadataExtension
    {
        /// <summary>
        /// Transform the base class to the specific element.
        /// </summary>
        /// <param name="source">ElementMetadataBase class</param>
        /// <returns></returns>
        public static T ToElement<T>(this ElementMetadataBase source) where T : ElementMetadataBase
        {
            if(source == null)
            {
                return null;
            }

            switch (source.Type)
            {
                case ElementMetadataType.Undefined:
                        throw new InvalidOperationException($"Type {source.Type} cannot be converted.");
                case ElementMetadataType.Text:
                    if (source.Type != ElementMetadataType.Text)
                    {
                        throw new InvalidOperationException($"Type {source.Type} cannot be converted to {nameof(TextElementMetadataModel)}");
                    }

                    return source as T;
                case ElementMetadataType.RichText:
                    if (source.Type != ElementMetadataType.RichText)
                    {
                        throw new InvalidOperationException($"Type {source.Type} cannot be converted to {nameof(RichTextElementMetadataModel)}");
                    }

                    return source as T;
                case ElementMetadataType.Number:
                    if (source.Type != ElementMetadataType.Number)
                    {
                        throw new InvalidOperationException($"Type {source.Type} cannot be converted to {nameof(NumberElementMetadataModel)}");
                    }

                    return source as T;
                case ElementMetadataType.MultipleChoice:
                    if (source.Type != ElementMetadataType.MultipleChoice)
                    {
                        throw new InvalidOperationException($"Type {source.Type} cannot be converted to {nameof(MultipleChoiceElementMetadataModel)}");
                    }

                    return source as T;
                case ElementMetadataType.DateTime:
                    if (source.Type != ElementMetadataType.DateTime)
                    {
                        throw new InvalidOperationException($"Type {source.Type} cannot be converted to {nameof(DateTimeElementMetadataModel)}");
                    }

                    return source as T;
                case ElementMetadataType.Asset:
                    if (source.Type != ElementMetadataType.Asset)
                    {
                        throw new InvalidOperationException($"Type {source.Type} cannot be converted to {nameof(AssetElementMetadataModel)}");
                    }

                    return source as T;
                case ElementMetadataType.LinkedItems:
                    if (source.Type != ElementMetadataType.LinkedItems)
                    {
                        throw new InvalidOperationException($"Type {source.Type} cannot be converted to {nameof(LinkedItemsElementMetadataModel)}");
                    }

                    return source as T;
                case ElementMetadataType.Guidelines:
                    if (source.Type != ElementMetadataType.Guidelines)
                    {
                        throw new InvalidOperationException($"Type {source.Type} cannot be converted to {nameof(GuidelinesElementMetadataModel)}");
                    }

                    return source as T;
                case ElementMetadataType.Taxonomy:
                    if (source.Type != ElementMetadataType.Taxonomy)
                    {
                        throw new InvalidOperationException($"Type {source.Type} cannot be converted to {nameof(TaxonomyElementMetadataModel)}");
                    }

                    return source as T;
                case ElementMetadataType.UrlSlug:
                    if (source.Type != ElementMetadataType.UrlSlug)
                    {
                        throw new InvalidOperationException($"Type {source.Type} cannot be converted to {nameof(UrlSlugElementMetadataModel)}");
                    }

                    return source as T;
                case ElementMetadataType.Snippet:
                    if (source.Type != ElementMetadataType.Snippet)
                    {
                        throw new InvalidOperationException($"Type {source.Type} cannot be converted to {nameof(ContentTypeSnippetElementMetadataModel)}");
                    }

                    return source as T;
                case ElementMetadataType.Custom:
                    if (source.Type != ElementMetadataType.Custom)
                    {
                        throw new InvalidOperationException($"Type {source.Type} cannot be converted to {nameof(CustomElementMetadataModel)}");
                    }

                    return source as T;
                default: throw new InvalidOperationException($"Type {source.Type} cannot be converted to any known element");
            }
        }
    }
}

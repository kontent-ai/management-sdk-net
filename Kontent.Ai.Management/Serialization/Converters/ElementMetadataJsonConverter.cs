using Kontent.Ai.Management.Models.Types.Elements;
using System.Text.Json;

namespace Kontent.Ai.Management.Serialization.Converters;

internal sealed class ElementMetadataJsonConverter : PolymorphicJsonConverter<ElementMetadataBase, ElementMetadataType>
{
    protected override string DiscriminatorPropertyName => "type";

    protected override Type ResolveType(ElementMetadataType discriminator) => discriminator switch
    {
        ElementMetadataType.Text => typeof(TextElementMetadataModel),
        ElementMetadataType.RichText => typeof(RichTextElementMetadataModel),
        ElementMetadataType.Number => typeof(NumberElementMetadataModel),
        ElementMetadataType.MultipleChoice => typeof(MultipleChoiceElementMetadataModel),
        ElementMetadataType.DateTime => typeof(DateTimeElementMetadataModel),
        ElementMetadataType.Asset => typeof(AssetElementMetadataModel),
        ElementMetadataType.LinkedItems => typeof(LinkedItemsElementMetadataModel),
        ElementMetadataType.Guidelines => typeof(GuidelinesElementMetadataModel),
        ElementMetadataType.Taxonomy => typeof(TaxonomyElementMetadataModel),
        ElementMetadataType.UrlSlug => typeof(UrlSlugElementMetadataModel),
        ElementMetadataType.ContentTypeSnippet => typeof(ContentTypeSnippetElementMetadataModel),
        ElementMetadataType.Custom => typeof(CustomElementMetadataModel),
        ElementMetadataType.Subpages => typeof(SubpagesElementMetadataModel),
        _ => throw new JsonException($"No element metadata subtype for '{discriminator}'."),
    };
}

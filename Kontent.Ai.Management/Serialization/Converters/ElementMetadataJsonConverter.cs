using Kontent.Ai.Management.Models.Types.Elements;
using System.Text.Json;

namespace Kontent.Ai.Management.Serialization.Converters;

internal sealed class ElementMetadataJsonConverter : PolymorphicJsonConverter<ElementMetadataBase>
{
    protected override string DiscriminatorPropertyName => "type";

    protected override Type ResolveType(string discriminator) => discriminator switch
    {
        "text" => typeof(TextElementMetadataModel),
        "rich_text" => typeof(RichTextElementMetadataModel),
        "number" => typeof(NumberElementMetadataModel),
        "multiple_choice" => typeof(MultipleChoiceElementMetadataModel),
        "date_time" => typeof(DateTimeElementMetadataModel),
        "asset" => typeof(AssetElementMetadataModel),
        "modular_content" => typeof(LinkedItemsElementMetadataModel),
        "guidelines" => typeof(GuidelinesElementMetadataModel),
        "taxonomy" => typeof(TaxonomyElementMetadataModel),
        "url_slug" => typeof(UrlSlugElementMetadataModel),
        "snippet" => typeof(ContentTypeSnippetElementMetadataModel),
        "custom" => typeof(CustomElementMetadataModel),
        "subpages" => typeof(SubpagesElementMetadataModel),
        _ => throw new JsonException($"Unknown element metadata type '{discriminator}'."),
    };
}

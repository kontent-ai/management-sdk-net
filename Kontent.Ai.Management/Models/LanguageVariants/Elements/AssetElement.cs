using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.LanguageVariants.Elements;

/// <summary>Value of an asset element: the referenced assets, optionally with renditions.</summary>
public sealed record AssetElement : BaseElement
{
    /// <summary>The referenced assets.</summary>
    [JsonPropertyName("value")]
    public IEnumerable<AssetWithRenditionsReference>? Value { get; init; }
}

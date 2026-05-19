using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.Environments;

/// <summary>
/// Represents options for copying entities
/// </summary>
public sealed record CopyDataOptions
{
    /// <summary>
    /// Gets an option to copy content items and assets.
    /// </summary>
    [JsonPropertyName("content_items_assets")]
    public bool ContentItemsAssets { get; init; }

    /// <summary>
    /// Gets an option to copy version history of content items.
    /// </summary>
    [JsonPropertyName("content_item_version_history")]
    public bool ContentItemVersionHistory { get; init; }
}
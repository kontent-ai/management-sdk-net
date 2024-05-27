using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.Environments;

/// <summary>
/// Represents options for copying entities
/// </summary>
public class CopyDataOptions
{
    /// <summary>
    /// Gets or sets an option to copy content items and assets.
    /// </summary>
    [JsonProperty("content_items_assets")]
    public bool ContentItemsAssets { get; set; }

    /// <summary>
    /// Gets or sets an option to copy version history of content items.
    /// </summary>
    [JsonProperty("content_item_version_history")]
    public bool ContentItemVersionHistory { get; set; }
}
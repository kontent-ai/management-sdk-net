using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.Assets;

/// <summary>
/// A taxonomy assignment on an asset — a taxonomy group and the terms selected from it. The environment's
/// (singleton) asset type defines which taxonomy groups are available; the Management API does not expose the
/// asset type itself, so the groups and terms are referenced directly.
/// </summary>
public sealed record AssetElement
{
    /// <summary>
    /// Reference to the taxonomy group.
    /// </summary>
    [JsonPropertyName("element")]
    public Reference Element { get; init; }

    /// <summary>
    /// References to the selected taxonomy terms; empty when none are assigned.
    /// </summary>
    [JsonPropertyName("value")]
    public IEnumerable<Reference> Value { get; init; }
}

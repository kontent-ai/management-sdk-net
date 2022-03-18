using Kentico.Kontent.Management.Models.Shared;
using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Collections.Patch;

/// <summary>
/// Represents move operation.
/// More info: https://kontent.ai/learn/reference/management-api-v2#operation/modify-collections
/// </summary>
public sealed class CollectionMovePatchModel : CollectionOperationBaseModel
{
    /// <summary>
    /// Represents move operation.
    /// </summary>
    public override string Op => "move";

    /// <summary>
    /// Gets or sets the reference of the collection to move.
    /// </summary>
    [JsonProperty("reference")]
    public Reference Reference { get; set; }

    /// <summary>
    /// Gets or sets reference of the existing collection before which you want to add the new collection.
    /// Note: The before and after properties are mutually exclusive.
    /// </summary>
    [JsonProperty("before")]
    public Reference Before { get; set; }

    /// <summary>
    /// Gets or sets reference of the existing collection after which you want to add the new collection.
    /// Note: The before and after properties are mutually exclusive.
    /// </summary>
    [JsonProperty("after")]
    public Reference After { get; set; }
}

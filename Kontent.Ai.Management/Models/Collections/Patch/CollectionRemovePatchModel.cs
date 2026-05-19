using Kontent.Ai.Management.Models.Shared;
using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.Collections.Patch;

/// <summary>
/// Represents the remove operation.
/// More info: https://kontent.ai/learn/reference/management-api-v2#operation/modify-collections
/// </summary>
public sealed record CollectionRemovePatchModel : CollectionOperationBaseModel
{
    /// <summary>
    /// Represents the remove operation.
    /// </summary>
    public override string Op => "remove";

    /// <summary>
    /// Represents the reference of the collection which should be removed.
    /// </summary>
    [JsonPropertyName("reference")]
    public Reference CollectionIdentifier { get; init; }
}

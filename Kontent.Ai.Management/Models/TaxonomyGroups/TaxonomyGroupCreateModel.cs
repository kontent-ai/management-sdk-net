using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.TaxonomyGroups;

/// <summary>
/// Represents the taxonomy group create model.
/// </summary>
public sealed record TaxonomyGroupCreateModel : TaxonomyBaseModel
{
    /// <summary>
    /// Gets terms in the taxonomy group.
    /// </summary>
    [JsonPropertyName("terms")]
    public IEnumerable<TaxonomyTermCreateModel> Terms { get; init; }
}

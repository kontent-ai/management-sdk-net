using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.TaxonomyGroups;

/// <summary>
/// Represents the taxonomy group create model.
/// </summary>
public sealed record TaxonomyGroupCreateModel : TaxonomyBaseModel
{
    /// <summary>
    /// Gets terms in the taxonomy group.
    /// </summary>
    [JsonProperty("terms", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public IEnumerable<TaxonomyTermCreateModel> Terms { get; init; }
}

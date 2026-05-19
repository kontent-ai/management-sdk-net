using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.TaxonomyGroups;

/// <summary>
/// Represents the taxonomy term model.
/// </summary>
public sealed record TaxonomyTermModel : TaxonomyBaseModel
{
    /// <summary>
    /// Gets the taxonomy group's internal ID.
    /// </summary>
    [JsonProperty("id")]
    public Guid Id { get; init; }

    /// <summary>
    /// Gets terms in the taxonomy group.
    /// </summary>
    [JsonProperty("terms")]
    public IEnumerable<TaxonomyTermModel> Terms { get; init; }
}

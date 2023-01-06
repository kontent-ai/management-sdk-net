using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.TaxonomyGroups;

/// <summary>
/// Represents the taxonomy term model.
/// </summary>
public class TaxonomyTermModel : TaxonomyBaseModel
{
    /// <summary>
    /// Gets or sets the taxonomy group's internal ID.
    /// </summary>
    [JsonProperty("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets terms in the taxonomy group.
    /// </summary>
    [JsonProperty("terms")]
    public IEnumerable<TaxonomyTermModel> Terms { get; set; }
}

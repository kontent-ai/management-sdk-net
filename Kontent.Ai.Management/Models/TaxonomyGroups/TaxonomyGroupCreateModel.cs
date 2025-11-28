using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.TaxonomyGroups;

/// <summary>
/// Represents the taxonomy group create model.
/// </summary>
public class TaxonomyGroupCreateModel : TaxonomyBaseModel
{
    /// <summary>
    /// Gets or sets terms in the taxonomy group.
    /// </summary>
    [JsonProperty("terms", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public IEnumerable<TaxonomyTermCreateModel> Terms { get; set; }
}

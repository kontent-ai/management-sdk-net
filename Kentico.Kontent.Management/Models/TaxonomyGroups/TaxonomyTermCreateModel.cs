using System.Collections.Generic;
using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.TaxonomyGroups;

/// <summary>
/// Represents the taxonomy term create model.
/// </summary>
public class TaxonomyTermCreateModel : TaxonomyBaseModel
{
    /// <summary>
    /// Gets or sets terms in the taxonomy group.
    /// </summary>
    [JsonProperty("terms")]
    public IEnumerable<TaxonomyTermCreateModel> Terms { get; set; }
}

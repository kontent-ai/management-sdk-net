using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.ItemWithVariant;

/// <summary>
/// Represents the request model for bulk-getting items with variants.
/// </summary>
public class ItemWithVariantBulkGetRequestModel
{
    /// <summary>
    /// Gets or sets the variant identifiers (item + language pairs).
    /// </summary>
    [JsonProperty("variants")]
    public IEnumerable<VariantIdentifierModel> Variants { get; set; }
}

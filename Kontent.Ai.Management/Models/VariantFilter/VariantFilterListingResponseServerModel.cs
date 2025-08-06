using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.VariantFilter;

/// <summary>
/// Represents the variant filter response model.
/// </summary>
[JsonObject]
internal class VariantFilterListingResponseServerModel : IListingResponse<VariantFilterItemModel>
{
    /// <summary>
    /// Gets or sets the variant filter data.
    /// </summary>
    [JsonProperty("data")]
    public IEnumerable<VariantFilterItemModel> Data { get; set; }

    /// <summary>
    /// Gets or sets the pagination response.
    /// </summary>
    [JsonProperty("pagination")]
    public PaginationResponseModel Pagination { get; set; }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<VariantFilterItemModel> GetEnumerator() => Data.GetEnumerator();
}
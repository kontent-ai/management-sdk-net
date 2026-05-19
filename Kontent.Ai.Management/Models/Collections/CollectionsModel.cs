using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.Collections;

/// <summary>
/// Represents content collections
/// </summary>
public sealed record CollectionsModel
{
    /// <summary>
    /// Gets the list of content collections
    /// </summary>
    [JsonProperty("collections")]
    public IEnumerable<CollectionModel> Collections { get; init; }

    /// <summary>
    /// Gets the ISO-8601 formatted date and time of the last change to content collections.
    /// This property can be null if the collections were not changed yet.
    /// </summary>
    [JsonProperty("last_modified")]
    public DateTime? LastModified { get; init; }
}

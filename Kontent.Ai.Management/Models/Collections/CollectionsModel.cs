using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.Collections;

/// <summary>
/// Represents content collections
/// </summary>
public sealed record CollectionsModel
{
    /// <summary>
    /// Gets the list of content collections
    /// </summary>
    [JsonPropertyName("collections")]
    public IEnumerable<CollectionModel> Collections { get; init; }

    /// <summary>
    /// Gets the ISO-8601 formatted date and time of the last change to content collections.
    /// This property can be null if the collections were not changed yet.
    /// </summary>
    [JsonPropertyName("last_modified")]
    public DateTime? LastModified { get; init; }
}

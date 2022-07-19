using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Modules.ActionInvoker;
using Newtonsoft.Json;
using System;

namespace Kontent.Ai.Management.Models.Types.Elements;

/// <summary>
/// Represents the base class for elements in types.
/// </summary>
[JsonConverter(typeof(ElementMetadataConverter))]
public abstract class ElementMetadataBase
{
    /// <summary>
    /// Represents the type of the content type element.
    /// </summary>
    [JsonProperty("type", Required = Required.Always)]
    public abstract ElementMetadataType Type { get; }

    /// <summary>
    /// Gets or sets the element's display name.
    /// </summary>
    [JsonProperty("external_id")]
    public string ExternalId { get; set; }

    /// <summary>
    /// Gets or sets the element's internal ID.
    /// </summary>
    [JsonProperty("id")]
    public Guid Id { get; private set; }

    /// <summary>
    /// Gets or sets the element's codename.
    /// Unless specified, initially generated from the element's name.
    /// </summary>
    [JsonProperty("codename")]
    public string Codename { get; set; }

    /// <summary>
    /// Gets or sets the content group where the element is used in.
    /// </summary>
    [JsonProperty("content_group", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public Reference ContentGroup { get; set; }

    /// <summary>
    /// Gets or sets element is non-localizable
    /// </summary>
    [JsonProperty("is_non_localizable")]
    public bool IsNonLocalizable { get; set; }
}

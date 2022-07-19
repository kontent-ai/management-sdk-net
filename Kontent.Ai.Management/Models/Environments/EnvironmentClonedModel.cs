using Newtonsoft.Json;
using System;

namespace Kontent.Ai.Management.Models.Environments;

/// <summary>
/// Represents result of environment cloning.
/// </summary>
public class EnvironmentClonedModel
{
    /// <summary>
    /// Gets or sets the ID of the cloned environment.
    /// </summary>
    [JsonProperty("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the Management API key.
    /// </summary>
    [JsonProperty("management_api_key")]
    public string ManagementApiKey { get; set; }

    /// <summary>
    /// Gets or sets the Delivery preview API key.
    /// </summary>
    [JsonProperty("delivery_preview_api_key")]
    public string DeliveryPreviewApiKey { get; set; }

    /// <summary>
    /// Gets or sets the Secured delivery API key.
    /// </summary>
    [JsonProperty("secured_delivery_api_key")]
    public string SecuredDeliveryApiKey { get; set; }
}

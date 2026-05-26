namespace Kontent.Ai.Management.Models.Environments;

/// <summary>
/// Represents result of environment cloning.
/// </summary>
public sealed record EnvironmentClonedModel
{
    /// <summary>
    /// Gets the ID of the cloned environment.
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    /// <summary>
    /// Gets the Management API key.
    /// </summary>
    [JsonPropertyName("management_api_key")]
    public string ManagementApiKey { get; init; }

    /// <summary>
    /// Gets the Delivery preview API key.
    /// </summary>
    [JsonPropertyName("delivery_preview_api_key")]
    public string DeliveryPreviewApiKey { get; init; }

    /// <summary>
    /// Gets the Secured delivery API key.
    /// </summary>
    [JsonPropertyName("secured_delivery_api_key")]
    public string SecuredDeliveryApiKey { get; init; }
}

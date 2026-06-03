namespace Kontent.Ai.Management.Models.Environments;

/// <summary>
/// Result of cloning an environment.
/// </summary>
public sealed record EnvironmentClonedModel
{
    /// <summary>
    /// ID of the cloned environment.
    /// </summary>
    [JsonPropertyName("id")]
    public required Guid Id { get; init; }

    /// <summary>
    /// Management API key for the cloned environment.
    /// </summary>
    [JsonPropertyName("management_api_key")]
    public required string ManagementApiKey { get; init; }

    /// <summary>
    /// Delivery preview API key. Null when the project has no preview Delivery API key (the key type is deprecated in favor of scoped Delivery API keys).
    /// </summary>
    [JsonPropertyName("delivery_preview_api_key")]
    public string? DeliveryPreviewApiKey { get; init; }

    /// <summary>
    /// Secured delivery API key. Null when the project has no secured Delivery API key (the key type is deprecated in favor of scoped Delivery API keys).
    /// </summary>
    [JsonPropertyName("secured_delivery_api_key")]
    public string? SecuredDeliveryApiKey { get; init; }
}

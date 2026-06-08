using System.ComponentModel.DataAnnotations;

namespace Kontent.Ai.Management.Configuration;

/// <summary>
/// Configuration for <see cref="ManagementClient"/>. Bind from <c>IConfiguration</c> or construct directly.
/// </summary>
public sealed class ManagementOptions : IValidatableObject
{
    /// <summary>
    /// Gets or sets the Production endpoint address. Optional, defaults to "https://manage.kontent.ai/{0}".
    /// </summary>
    [Url]
    public string Endpoint { get; set; } = "https://manage.kontent.ai/{0}";

    /// <summary>
    /// Gets or sets the Production endpoint address for V2 management API. Optional, defaults to "https://manage.kontent.ai/v2/{0}".
    /// </summary>
    [Url]
    public string EndpointV2 { get; set; } = "https://manage.kontent.ai/v2/{0}";

    /// <summary>
    /// Gets or sets the environment identifier (GUID).
    /// </summary>
    [Required]
    public string? EnvironmentId { get; set; }

    /// <summary>
    /// Gets or sets the subscription identifier. Required only for subscription-scoped endpoints.
    /// </summary>
    public string? SubscriptionId { get; set; }

    /// <summary>
    /// Gets or sets the Management API key.
    /// </summary>
    [Required]
    public string? ApiKey { get; set; }

    /// <summary>
    /// Gets or sets whether the default resilience pipeline is active. Defaults to <c>true</c>. Set to <c>false</c>
    /// to bypass all retry/backoff behaviour without uninstalling the handler.
    /// </summary>
    public bool EnableResilience { get; set; } = true;

    /// <inheritdoc />
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!Guid.TryParse(EnvironmentId, out var environmentGuid))
        {
            yield return new ValidationResult(
                $"Provided string is not a valid environment identifier ({EnvironmentId}). Haven't you accidentally passed the API key instead of the environment identifier?",
                [nameof(EnvironmentId)]);
        }
        else if (environmentGuid == Guid.Empty)
        {
            yield return new ValidationResult(
                "EnvironmentId cannot be an empty GUID.",
                [nameof(EnvironmentId)]);
        }
    }
}

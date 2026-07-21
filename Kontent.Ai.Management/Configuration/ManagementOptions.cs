using System.ComponentModel.DataAnnotations;

namespace Kontent.Ai.Management.Configuration;

/// <summary>
/// Configuration for <see cref="ManagementClient"/>. Bind from <c>IConfiguration</c> or construct directly.
/// </summary>
public sealed class ManagementOptions : IValidatableObject
{
    /// <summary>
    /// The configuration section name <c>AddManagementClient</c> binds by default. Design-time tools that resolve
    /// the SDK's configuration from the same sources (e.g. a model-pull CLI) should probe this section.
    /// </summary>
    public const string DefaultConfigurationSectionName = "ManagementOptions";

    /// <summary>
    /// Gets or sets the base address of the Management API. Optional; defaults to <c>https://manage.kontent.ai</c>.
    /// The SDK appends the versioned, scoped path (<c>/v2/projects/{id}</c> or <c>/v2/subscriptions/{id}</c>).
    /// </summary>
    [Url]
    public string Endpoint { get; set; } = "https://manage.kontent.ai";

    /// <summary>
    /// Gets or sets the environment identifier (GUID).
    /// </summary>
    public string? EnvironmentId { get; set; }

    /// <summary>
    /// Gets or sets the subscription identifier. Required only for subscription-scoped endpoints.
    /// </summary>
    public string? SubscriptionId { get; set; }

    /// <summary>
    /// Gets or sets the Management API key.
    /// </summary>
    public string? ApiKey { get; set; }

    /// <summary>
    /// Gets or sets whether the default resilience pipeline is active. Defaults to <c>true</c>. Set to <c>false</c>
    /// to bypass all retry/backoff behaviour without uninstalling the handler.
    /// </summary>
    public bool EnableResilience { get; set; } = true;

    /// <inheritdoc />
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(ApiKey))
        {
            yield return new ValidationResult(
                "ApiKey cannot be empty.",
                [nameof(ApiKey)]);
        }

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

        if (!string.IsNullOrWhiteSpace(SubscriptionId)
            && (!Guid.TryParse(SubscriptionId, out var subscriptionGuid) || subscriptionGuid == Guid.Empty))
        {
            yield return new ValidationResult(
                $"Provided string is not a valid subscription identifier ({SubscriptionId}).",
                [nameof(SubscriptionId)]);
        }
    }
}

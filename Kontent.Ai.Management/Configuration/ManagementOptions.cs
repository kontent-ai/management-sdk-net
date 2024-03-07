using Kontent.Ai.Management.Modules.ModelBuilders;

namespace Kontent.Ai.Management.Configuration;

/// <summary>
/// Keeps settings which are provided by customer or have default values, used in <see cref="ManagementClient"/>.
/// </summary>
public class ManagementOptions
{
    /// <summary>
    /// Gets or sets the Production endpoint address. Optional, defaults to "https://manage.kontent.ai/{0}".
    /// </summary>
    public string Endpoint { get; set; } = "https://manage.kontent.ai/{0}";

    /// <summary>
    /// Gets or sets the Production endpoint address for V2 management API. Optional, defaults to "https://manage.kontent.ai/v2/{0}".
    /// </summary>
    public string EndpointV2 { get; set; } = "https://manage.kontent.ai/v2/{0}";

    /// <summary>
    /// Gets or sets the Environment identifier.
    /// </summary>
    public string EnvironmentId { get; set; }

    /// <summary>
    /// Gets or sets the Subscription identifier.
    /// </summary>
    public string SubscriptionId { get; set; }

    /// <summary>
    /// Gets or sets the Preview API key.
    /// </summary>
    public string ApiKey { get; set; }

    /// <summary>
    /// Gets or sets the Model provider for strongly typed models
    /// </summary>
    public IModelProvider ModelProvider { get; set; }

    /// <summary>
    /// Gets or sets whether HTTP requests will use a retry logic.
    /// </summary>
    public bool EnableResilienceLogic { get; set; } = true;

    /// <summary>
    /// Gets or sets the maximum retry attempts.
    /// </summary>
    public int MaxRetryAttempts { get; set; } = 5;
}

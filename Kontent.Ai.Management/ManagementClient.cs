using Kontent.Ai.Management.Api;
using Kontent.Ai.Management.Configuration;
using Kontent.Ai.Management.Exceptions;
using Kontent.Ai.Management.Modules.ModelBuilders;

namespace Kontent.Ai.Management;

/// <summary>
/// Executes requests against the Kontent.ai Management API.
/// </summary>
public sealed partial class ManagementClient : IManagementClient
{
    private const int MAX_FILE_SIZE_MB = 100;

    private readonly IManagementApi _managementApi;
    private readonly ISubscriptionApi _subscriptionApi;
    private readonly IModelProvider _modelProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="ManagementClient"/> class for managing content of the specified environment.
    /// </summary>
    /// <param name="managementOptions">The settings of the Kontent.ai environment.</param>
    public ManagementClient(ManagementOptions managementOptions)
    {
        ArgumentNullException.ThrowIfNull(managementOptions);

        if (string.IsNullOrEmpty(managementOptions.EnvironmentId))
        {
            throw new ArgumentException("Kontent.ai environment identifier is not specified.", nameof(managementOptions));
        }

        if (!Guid.TryParse(managementOptions.EnvironmentId, out _))
        {
            throw new ArgumentException($"Provided string is not a valid environment identifier ({managementOptions.EnvironmentId}). Haven't you accidentally passed the API key instead of the environment identifier?", nameof(managementOptions));
        }

        if (string.IsNullOrEmpty(managementOptions.ApiKey))
        {
            throw new ArgumentException("The API key is not specified.", nameof(managementOptions));
        }

        _managementApi = ManagementApiFactory.Create(managementOptions);
        _subscriptionApi = ManagementApiFactory.CreateSubscription(managementOptions);
        _modelProvider = managementOptions.ModelProvider ?? new ModelProvider();
    }

    internal ManagementClient(IManagementApi managementApi, ISubscriptionApi subscriptionApi, IModelProvider modelProvider = null)
    {
        _managementApi = managementApi;
        _subscriptionApi = subscriptionApi;
        _modelProvider = modelProvider ?? new ModelProvider();
    }

    // The response is not disposed here — the test harness reuses canned HttpResponseMessage instances across calls,
    // and Refit has already fully read the body to populate `IApiResponse<T>.Content` by the time we get here.
    private static T EnsureSuccess<T>(IApiResponse<T> response)
    {
        ThrowIfNotSuccess(response);
        return response.Content!;
    }

    private static void EnsureSuccess(IApiResponse response) => ThrowIfNotSuccess(response);

    private static void ThrowIfNotSuccess(IApiResponse response)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var error = response.Error;
        throw new ManagementException(
            error?.StatusCode ?? response.StatusCode,
            error?.ReasonPhrase ?? response.ReasonPhrase,
            error?.Content ?? "CM API returned server error.");
    }
}

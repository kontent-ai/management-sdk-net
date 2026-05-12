using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Kontent.Ai.Management.Api;
using Kontent.Ai.Management.Configuration;
using Kontent.Ai.Management.Exceptions;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Modules.ActionInvoker;
using Kontent.Ai.Management.Modules.HttpClient;
using Kontent.Ai.Management.Modules.ModelBuilders;
using Kontent.Ai.Management.Modules.ResiliencePolicy;
using Kontent.Ai.Management.Modules.UrlBuilder;

namespace Kontent.Ai.Management;

/// <summary>
/// Executes requests against the Kontent.ai Management API.
/// </summary>
public sealed partial class ManagementClient : IManagementClient
{
    private const int MAX_FILE_SIZE_MB = 100;

    // Domains are being migrated to the Refit-backed _managementApi one at a time; the rest still go through _actionInvoker.
    private readonly ActionInvoker _actionInvoker;
    private readonly IManagementApi _managementApi;
    private readonly EndpointUrlBuilder _urlBuilder;
    private readonly IModelProvider _modelProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="ManagementClient"/> class for managing content of the specified environment.
    /// </summary>
    /// <param name="ManagementOptions">The settings of the Kontent.ai environment.</param>
    public ManagementClient(ManagementOptions ManagementOptions)
        : this(ManagementOptions, new System.Net.Http.HttpClient())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ManagementClient"/> class for managing content of the specified environment.
    /// </summary>
    /// <param name="ManagementOptions">The settings of the Kontent.ai environment.</param>
    /// <param name="httpClient">Custom HttpClient instance to use for HTTP requests.</param>
    public ManagementClient(ManagementOptions ManagementOptions, System.Net.Http.HttpClient httpClient)
    {
        ArgumentNullException.ThrowIfNull(ManagementOptions);

        if (string.IsNullOrEmpty(ManagementOptions.EnvironmentId))
        {
            throw new ArgumentException("Kontent.ai environment identifier is not specified.", nameof(ManagementOptions.EnvironmentId));
        }

        if (!Guid.TryParse(ManagementOptions.EnvironmentId, out _))
        {
            throw new ArgumentException($"Provided string is not a valid environment identifier ({ManagementOptions.EnvironmentId}). Haven't you accidentally passed the API key instead of the environment identifier?", nameof(ManagementOptions.EnvironmentId));
        }

        if (string.IsNullOrEmpty(ManagementOptions.ApiKey))
        {
            throw new ArgumentException("The API key is not specified.", nameof(ManagementOptions.ApiKey));
        }


        _urlBuilder = new EndpointUrlBuilder(ManagementOptions);
        _actionInvoker = new ActionInvoker(
            new ManagementHttpClient(new Modules.HttpClient.HttpClient(httpClient), new DefaultResiliencePolicyProvider(ManagementOptions.MaxRetryAttempts), ManagementOptions.EnableResilienceLogic),
            new MessageCreator(ManagementOptions.ApiKey));
        // The supplied httpClient feeds the legacy path only; the Refit path builds its own client until the migration completes.
        _managementApi = ManagementApiFactory.Create(ManagementOptions);
        _modelProvider = ManagementOptions.ModelProvider ?? new ModelProvider();
    }

    internal ManagementClient(EndpointUrlBuilder urlBuilder, ActionInvoker actionInvoker, IManagementApi managementApi, IModelProvider modelProvider = null)
    {
        _urlBuilder = urlBuilder ?? throw new ArgumentNullException(nameof(urlBuilder));
        _actionInvoker = actionInvoker ?? throw new ArgumentNullException(nameof(actionInvoker));
        _managementApi = managementApi;
        _modelProvider = modelProvider ?? new ModelProvider();
    }

    private async Task<IListingResponse<TModel>> GetNextListingPageAsync<TListingResponse, TModel>(string continuationToken, string url)
        where TListingResponse : IListingResponse<TModel>
    {
        var headers = new Dictionary<string, string>
        {
            { "x-continuation", continuationToken }
        };
        var response = await _actionInvoker.InvokeReadOnlyMethodAsync<TListingResponse>(url, HttpMethod.Get, headers);

        return response;
    }

    // The response is not disposed here — the legacy SDK never disposed responses either, and the test harness reuses
    // the same HttpResponseMessage instances across calls. (Refit has already fully read the body for IApiResponse<T>.)
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

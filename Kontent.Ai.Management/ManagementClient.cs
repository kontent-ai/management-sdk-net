using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Kontent.Ai.Management.Modules.HttpClient;
using Kontent.Ai.Management.Modules.ResiliencePolicy;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Modules.ActionInvoker;
using Kontent.Ai.Management;
using Kontent.Ai.Management.Configuration;
using Kontent.Ai.Management.Modules.ModelBuilders;
using Kontent.Ai.Management.Modules.UrlBuilder;

namespace Kontent.Ai.Management;

/// <summary>
/// Executes requests against the Kontent.ai Management API.
/// </summary>
public sealed partial class ManagementClient : IManagementClient
{
    private const int MAX_FILE_SIZE_MB = 100;

    private readonly ActionInvoker _actionInvoker;
    private readonly EndpointUrlBuilder _urlBuilder;
    private readonly IModelProvider _modelProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="ManagementClient"/> class for managing content of the specified project.
    /// </summary>
    /// <param name="ManagementOptions">The settings of the Kontent.ai project.</param>
    public ManagementClient(ManagementOptions ManagementOptions)
    {
        if (ManagementOptions == null)
        {
            throw new ArgumentNullException(nameof(ManagementOptions));
        }

        if (string.IsNullOrEmpty(ManagementOptions.ProjectId))
        {
            throw new ArgumentException("Kontent.ai project identifier is not specified.", nameof(ManagementOptions.ProjectId));
        }

        if (!Guid.TryParse(ManagementOptions.ProjectId, out _))
        {
            throw new ArgumentException($"Provided string is not a valid project identifier ({ManagementOptions.ProjectId}). Haven't you accidentally passed the API key instead of the project identifier?", nameof(ManagementOptions.ProjectId));
        }

        if (string.IsNullOrEmpty(ManagementOptions.ApiKey))
        {
            throw new ArgumentException("The API key is not specified.", nameof(ManagementOptions.ApiKey));
        }


        _urlBuilder = new EndpointUrlBuilder(ManagementOptions);
        _actionInvoker = new ActionInvoker(
            new ManagementHttpClient(new DefaultResiliencePolicyProvider(ManagementOptions.MaxRetryAttempts), ManagementOptions.EnableResilienceLogic),
            new MessageCreator(ManagementOptions.ApiKey));
        _modelProvider = ManagementOptions.ModelProvider ?? new ModelProvider();
    }

    internal ManagementClient(EndpointUrlBuilder urlBuilder, ActionInvoker actionInvoker, IModelProvider modelProvider = null)
    {
        _urlBuilder = urlBuilder ?? throw new ArgumentNullException(nameof(urlBuilder));
        _actionInvoker = actionInvoker ?? throw new ArgumentNullException(nameof(actionInvoker));
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
}

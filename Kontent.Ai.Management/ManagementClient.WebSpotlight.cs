using Kontent.Ai.Management.Models.WebSpotlight;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kontent.Ai.Management;

/// <summary>
/// Executes requests against the Kontent.ai Management API.
/// </summary>
public partial class ManagementClient
{
    /// <inheritdoc />
    public async Task<WebSpotlightModel> ActivateWebSpotlightAsync(WebSpotlightActivateModel webSpotlightActivateModel)
    {
        ArgumentNullException.ThrowIfNull(webSpotlightActivateModel);

        var endpointUrl = _urlBuilder.BuildWebSpotlightUrl();
        return await _actionInvoker.InvokeMethodAsync<WebSpotlightActivateModel, WebSpotlightModel>(endpointUrl, HttpMethod.Put, webSpotlightActivateModel);
    }
    
    /// <inheritdoc />
    public async Task<WebSpotlightModel> ActivateWebSpotlightAsync()
    {
        var endpointUrl = _urlBuilder.BuildWebSpotlightUrl();
        return await _actionInvoker.InvokeReadOnlyMethodAsync<WebSpotlightModel>(endpointUrl, HttpMethod.Put);
    }
    
    /// <inheritdoc />
    public async Task<WebSpotlightModel> DeactivateWebSpotlightAsync()
    {
        var endpointUrl = _urlBuilder.BuildWebSpotlightUrl();
        return await _actionInvoker.InvokeReadOnlyMethodAsync<WebSpotlightModel>(endpointUrl, HttpMethod.Put);
    }
    
    /// <inheritdoc />
    public async Task<WebSpotlightModel> GetWebSpotlightStatusAsync()
    {
        var endpointUrl = _urlBuilder.BuildWebSpotlightUrl();
        return await _actionInvoker.InvokeReadOnlyMethodAsync<WebSpotlightModel>(endpointUrl, HttpMethod.Get);
    }
}
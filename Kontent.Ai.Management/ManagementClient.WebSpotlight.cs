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
    [Obsolete("Web Spotlight has been sunset. Use Live Preview instead.")]
    public async Task<WebSpotlightModel> ActivateWebSpotlightAsync(WebSpotlightActivateModel webSpotlightActivateModel)
    {
        ArgumentNullException.ThrowIfNull(webSpotlightActivateModel);

        var endpointUrl = _urlBuilder.BuildWebSpotlightUrl();
        return await _actionInvoker.InvokeMethodAsync<WebSpotlightActivateModel, WebSpotlightModel>(endpointUrl, HttpMethod.Put, webSpotlightActivateModel);
    }

    /// <inheritdoc />
    [Obsolete("Web Spotlight has been sunset. Use Live Preview instead.")]
    public async Task<WebSpotlightModel> DeactivateWebSpotlightAsync()
    {
        var endpointUrl = _urlBuilder.BuildWebSpotlightUrl();
        return await _actionInvoker.InvokeReadOnlyMethodAsync<WebSpotlightModel>(endpointUrl, HttpMethod.Put);
    }
    
    /// <inheritdoc />
    [Obsolete("Web Spotlight has been sunset. Use Live Preview instead.")]
    public async Task<WebSpotlightModel> GetWebSpotlightStatusAsync()
    {
        var endpointUrl = _urlBuilder.BuildWebSpotlightUrl();
        return await _actionInvoker.InvokeReadOnlyMethodAsync<WebSpotlightModel>(endpointUrl, HttpMethod.Get);
    }
}
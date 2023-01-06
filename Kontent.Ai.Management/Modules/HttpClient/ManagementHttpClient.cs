using Kontent.Ai.Management.Exceptions;
using Kontent.Ai.Management.Modules.ActionInvoker;
using Kontent.Ai.Management.Modules.ResiliencePolicy;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kontent.Ai.Management.Modules.HttpClient;

internal class ManagementHttpClient : IManagementHttpClient
{
    private readonly IHttpClient _httpClient;
    private readonly IResiliencePolicyProvider _resiliencePolicyProvider;
    private readonly bool _enableResilienceLogic;

    public ManagementHttpClient(
        IHttpClient httpClient,
        IResiliencePolicyProvider resiliencePolicyProvider,
        bool enableResilienceLogic)
    {
        _httpClient = httpClient;
        _resiliencePolicyProvider = resiliencePolicyProvider;
        _enableResilienceLogic = enableResilienceLogic;
    }

    public ManagementHttpClient(
        IResiliencePolicyProvider resiliencePolicyProvider,
        bool enableResilienceLogic)
    {
        _httpClient = new HttpClient();
        _resiliencePolicyProvider = resiliencePolicyProvider;
        _enableResilienceLogic = enableResilienceLogic;
    }

    public async Task<HttpResponseMessage> SendAsync(
        IMessageCreator messageCreator,
        string endpointUrl,
        HttpMethod method,
        HttpContent requestContent = null,
        Dictionary<string, string> headers = null)
    {
        HttpResponseMessage response = null;

        if (_enableResilienceLogic)
        {
            if (_resiliencePolicyProvider == null)
            {
                throw new ArgumentNullException(
                    nameof(_resiliencePolicyProvider),
                    $"{nameof(_enableResilienceLogic)} is set to true but {nameof(_resiliencePolicyProvider)} is null.");
            }

            if (_resiliencePolicyProvider.Policy == null)
            {
                throw new ArgumentNullException(
                    nameof(_resiliencePolicyProvider.Policy),
                    $"{nameof(_resiliencePolicyProvider)}'s {nameof(_resiliencePolicyProvider.Policy)} is null.");
            }

            // Use the resilience logic.
            var policyResult = await _resiliencePolicyProvider.Policy.ExecuteAndCaptureAsync(() =>
                SendHttpMessage(messageCreator, endpointUrl, method, requestContent, headers));

            response = policyResult.FinalHandledResult ?? policyResult.Result;
        }
        else
        {
            // Omit using the resilience logic completely.
            response = await SendHttpMessage(messageCreator, endpointUrl, method, requestContent, headers);
        }

        if (response.IsSuccessStatusCode)
        {
            return response;
        }

        var responseContent = response.Content;

        throw responseContent != null ?
            new ManagementException(response, await response.Content.ReadAsStringAsync()) :
            new ManagementException(response, "CM API returned server error.");
    }

    private Task<HttpResponseMessage> SendHttpMessage(
        IMessageCreator messageCreator,
        string endpointUrl,
        HttpMethod method,
        HttpContent content,
        Dictionary<string, string> headers)
    {
        var message = messageCreator.CreateMessage(method, endpointUrl, content, headers);

        return _httpClient.SendAsync(message);
    }
}

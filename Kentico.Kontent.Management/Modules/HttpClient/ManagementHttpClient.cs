using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Kentico.Kontent.Management.Exceptions;
using Kentico.Kontent.Management.Modules.ActionInvoker;
using Kentico.Kontent.Management.Modules.Extensions;
using Kentico.Kontent.Management.Modules.ResiliencePolicy;
using Newtonsoft.Json.Linq;

namespace Kentico.Kontent.Management.Modules.HttpClient
{
    internal class ManagementHttpClient : IManagementHttpClient
    {
        private IHttpClient _httpClient;
        private IResiliencePolicyProvider _resiliencePolicyProvider;
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
                {
                    return SendHttpMessage(messageCreator, endpointUrl, method, requestContent, headers);
                });

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

            throw (responseContent != null) ?
                new ManagementException(response, await response.Content.ReadAsStringAsync()) :
                new ManagementException(response, "CM API returned server error.");
        }

        private Task<HttpResponseMessage> SendHttpMessage(
            IMessageCreator messageCreator,
            string endpointUrl,
            HttpMethod method,
            HttpContent content,
            Dictionary<string,string> headers)
        {
            HttpRequestMessage message = messageCreator.CreateMessage(method, endpointUrl, content, headers);

            return _httpClient.SendAsync(message);
        }

        private async Task<JObject> GetResponseContent(HttpResponseMessage httpResponseMessage)
        {
            if (httpResponseMessage?.StatusCode == HttpStatusCode.OK)
            {
                var content = await httpResponseMessage.Content?.ReadAsStringAsync();

                return JObject.Parse(content);
            }

            string faultContent = null;

            // The null-coallescing operator causes tests to fail for NREs, hence the "if" statement.
            if (httpResponseMessage?.Content != null)
            {
                faultContent = await httpResponseMessage.Content.ReadAsStringAsync();
            }

            throw new ManagementException(httpResponseMessage, faultContent);
        }
    }
}

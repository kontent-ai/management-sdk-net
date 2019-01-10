using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using KenticoCloud.ContentManagement.Exceptions;
using KenticoCloud.ContentManagement.Modules.ActionInvoker;
using KenticoCloud.ContentManagement.Modules.Extensions;
using KenticoCloud.ContentManagement.Modules.ResiliencePolicy;
using Newtonsoft.Json.Linq;

namespace KenticoCloud.ContentManagement.Modules.HttpClient
{
    internal class ContentManagementHttpClient : IContentManagementHttpClient
    {
        private IHttpClient _httpClient;
        private IResiliencePolicyProvider _resiliencePolicyProvider;
        private readonly bool _enableResilienceLogic;

        public ContentManagementHttpClient(
            IHttpClient httpClient,
            IResiliencePolicyProvider resiliencePolicyProvider,
            bool enableResilienceLogic)
        {
            _httpClient = httpClient;
            _resiliencePolicyProvider = resiliencePolicyProvider;
            _enableResilienceLogic = enableResilienceLogic;
        }

        public ContentManagementHttpClient(
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
            HttpContent requestContent = null)
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
                    return SendHttpMessage(messageCreator, endpointUrl, method, requestContent);
                });

                response = policyResult.FinalHandledResult ?? policyResult.Result;
            }
            else
            {
                // Omit using the resilience logic completely.
                response = await SendHttpMessage(messageCreator, endpointUrl, method, requestContent);
            }

            if (response.IsSuccessStatusCode)
            {
                return response;
            }

            var responseContent = response.Content;

            throw (responseContent != null) ?
                new ContentManagementException(response, await response.Content.ReadAsStringAsync()) :
                new ContentManagementException(response, "CM API returned server error.");
        }

        private Task<HttpResponseMessage> SendHttpMessage(
            IMessageCreator messageCreator,
            string endpointUrl,
            HttpMethod method,
            HttpContent content)
        {
            var message = messageCreator.CreateMessage(method, endpointUrl, content);
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

            throw new ContentManagementException(httpResponseMessage, faultContent);
        }
    }
}

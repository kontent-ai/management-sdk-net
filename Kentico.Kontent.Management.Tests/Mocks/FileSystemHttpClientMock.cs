using Kentico.Kontent.Management.Modules.ActionInvoker;
using Kentico.Kontent.Management.Modules.Extensions;
using Kentico.Kontent.Management.Modules.HttpClient;
using Kentico.Kontent.Management.Modules.ResiliencePolicy;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace Kentico.Kontent.Management.Tests.Mocks
{
    public class FileSystemHttpClientMock : IManagementHttpClient
    {
        private const string PROJECT_ID_REPLACEMENT = "{PROJECT_ID}";
        private const string SDK_ID_REPLACEMENT = "{SDK_ID}";
        private const string API_KEY_REPLACEMENT = "{API_KEY}";

        private readonly ManagementOptions _options;
        private readonly bool _saveToFileSystem;
        private readonly string _directoryName;
        private int _counter = 0;

        public IManagementHttpClient _nativeClient = new ManagementHttpClient(
            new DefaultResiliencePolicyProvider(Constants.DEFAULT_MAX_RETRIES),
            Constants.ENABLE_RESILIENCE_POLICY);

        public FileSystemHttpClientMock(ManagementOptions options, bool saveToFileSystem, string testName)
        {
            _saveToFileSystem = saveToFileSystem;
            _options = options;
            _directoryName = testName;
        }

        public async Task<HttpResponseMessage> SendAsync(
            IMessageCreator messageCreator,
            string endpointUrl,
            HttpMethod method,
            HttpContent content = null,
            Dictionary<string, string> headers = null)
        {
            var message = messageCreator.CreateMessage(method, endpointUrl, content, headers);
            var serializedRequest = SerializeRequest(message);
            var serializedRequestContent = await SerializeContent(message.Content);
            var folderPath = GetMockFileFolder(method.ToString());

            if (_saveToFileSystem)
            {
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                else
                {
                    // Cleanup previously recorded data at first request to avoid data overlap upon change
                    Directory.Delete(folderPath, true);
                    Directory.CreateDirectory(folderPath);
                }

                var response = await _nativeClient.SendAsync(messageCreator, endpointUrl, method, content, headers);

                File.WriteAllText(Path.Combine(folderPath, "request.json"), serializedRequest);
                File.WriteAllText(Path.Combine(folderPath, "request_content.json"), serializedRequestContent);

                
                var serializedResponse = SerializeResponse(response);
                var serializedResponseContent = await SerializeContent(response.Content);

                File.WriteAllText(Path.Combine(folderPath, "response.json"), serializedResponse);
                File.WriteAllText(Path.Combine(folderPath, "response_content.json"), serializedResponseContent);


                _counter++;
                return response;
            }
            else
            {
                Assert.Equal(serializedRequest, File.ReadAllText(Path.Combine(folderPath, "request.json")), ignoreLineEndingDifferences: true, ignoreWhiteSpaceDifferences: true);
                Assert.Equal(serializedRequestContent, File.ReadAllText(Path.Combine(folderPath, "request_content.json")), ignoreLineEndingDifferences: true, ignoreWhiteSpaceDifferences: true);

                var serializedResponse = ApplyData(File.ReadAllText(Path.Combine(folderPath, "response.json")));
                var serializedResponseContent = File.ReadAllText(Path.Combine(folderPath, "response_content.json"));

                var deserializationSettings = new JsonSerializerSettings
                {
                    ContractResolver = new IgnoreHttpContentContractResolver()
                };
                var response = JsonConvert.DeserializeObject<HttpResponseMessage>(serializedResponse, deserializationSettings);
                response.Content = new StringContent(serializedResponseContent);

                _counter++;
                return response;
            }

        }

        private string SerializeResponse(HttpResponseMessage response)
        {
            return MakeAgnostic(response.RequestMessage.Headers, () => JsonConvert.SerializeObject(response));
        }

        private string SerializeRequest(HttpRequestMessage request)
        {
            return MakeAgnostic(request.Headers, () => JsonConvert.SerializeObject(request));
        }

        private string MakeAgnostic(HttpRequestHeaders headers, Func<string> serialize)
        {
            List<KeyValuePair<string, IEnumerable<string>>> tempHeaders = new();
            tempHeaders.AddRange(headers.Select(x => x));

            if (headers.Contains("X-KC-SDKID"))
            {
                headers.Remove("X-KC-SDKID");
                headers.TryAddWithoutValidation("X-KC-SDKID", new[] { SDK_ID_REPLACEMENT });
            }

            if (headers.Contains("Authorization"))
            {
                headers.Remove("Authorization");
                headers.TryAddWithoutValidation("Authorization", new[] { API_KEY_REPLACEMENT });
            }

            var result = serialize().Replace(_options.ProjectId, PROJECT_ID_REPLACEMENT);

            headers.Clear();
            tempHeaders.ForEach(x => headers.Add(x.Key, x.Value));

            return result;
        }

        private string ApplyData(string data)
        {
            data = data.Replace(PROJECT_ID_REPLACEMENT, _options.ProjectId).Replace(API_KEY_REPLACEMENT, _options.ApiKey);
            return data.Replace(SDK_ID_REPLACEMENT, HttpRequestHeadersExtensions.GetSdkTrackingHeader());
        }

        private async Task<string> SerializeContent(HttpContent content)
        {
            if (content == null)
            {
                return string.Empty;
            }

            var text = await content.ReadAsStringAsync();
            text = Regex.Replace(text, @"\\r\\n", string.Empty);
            return Regex.Replace(text, @"\\n", string.Empty);
        }

        private string GetMockFileFolder(string methodName)
        {
            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            var rootPath = Path.Combine(projectDirectory, "Data");
            var testPath = Path.Combine(rootPath, _directoryName);
            return Path.Combine(testPath, $"{_counter:d}_{methodName}");
        }
    }
}

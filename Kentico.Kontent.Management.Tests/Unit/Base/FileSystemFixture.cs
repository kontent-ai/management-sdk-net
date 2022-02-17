using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Kentico.Kontent.Management.Modules.ActionInvoker;
using Kentico.Kontent.Management.Modules.HttpClient;
using Kentico.Kontent.Management.UrlBuilder;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NSubstitute;

namespace Kentico.Kontent.Management.Tests.Unit.Base
{
    public class FileSystemFixture : IDisposable
    {

        /// <summary>
        /// ID of the test project.
        /// </summary>
        public const string PROJECT_ID = "a9931a80-9af4-010b-0590-ecb1273cf1b8";
        private string _folder = "";
        private IConfiguration _configuration;
        private ManagementOptions _managementOptions;
        private EndpointUrlBuilder _urlBuilder;
        private MessageCreator _messageCreator;

        public FileSystemFixture()
        {
            _configuration = new ConfigurationBuilder()
                .AddUserSecrets<FileSystemFixture>()
                .Build();
            _managementOptions = new ManagementOptions()
            {
                ApiKey = _configuration.GetValue<string>("ApiKey", "Dummy_API_key"),
                ProjectId = _configuration.GetValue<string>("ProjectId", PROJECT_ID)
            };
            _urlBuilder = new EndpointUrlBuilder(_managementOptions);
            _messageCreator = new MessageCreator(_managementOptions.ApiKey);
        }

        public void SetSubFolder(string folder)
        {
            _folder = folder;
        }

        public IManagementClient CreateMockClient(IManagementHttpClient httpClient)
        {
            var actionInvoker = new ActionInvoker(httpClient, _messageCreator);
            return new ManagementClient(_urlBuilder, actionInvoker);
        }

        public IManagementClient CreateMockClientWithResponse(params string[] responseFileNames)
        {
            List<HttpResponseMessage> responses = new();
            foreach(var responseFileName in responseFileNames)
            {
                string dataPath = Path.Combine(Environment.CurrentDirectory, "Unit", "Data", _folder);

                var responsePath = Path.Combine(dataPath, responseFileName);
                var result = new HttpResponseMessage();
                result.Content = new StringContent(File.ReadAllText(responsePath));

                responses.Add(result);
            }

            var mockedHttpClient = Substitute.For<IManagementHttpClient>();
            mockedHttpClient.SendAsync(Arg.Any<IMessageCreator>(), Arg.Any<string>(), Arg.Any<HttpMethod>(), Arg.Any<HttpContent>(), Arg.Any<Dictionary<string, string>>())
             .Returns(responses.First(),responses.Skip(1).ToArray());
            return CreateMockClient(mockedHttpClient);
        }

        public IManagementClient CreateMockClientWithoutResponse()
        {
            var mockedHttpClient = Substitute.For<IManagementHttpClient>();
            mockedHttpClient.SendAsync(Arg.Any<IMessageCreator>(), Arg.Any<string>(), Arg.Any<HttpMethod>(), Arg.Any<HttpContent>(), Arg.Any<Dictionary<string, string>>())
             .Returns(x =>
             {
                 var result = new HttpResponseMessage();

                 return Task.FromResult<HttpResponseMessage>(result);
             });
            return CreateMockClient(mockedHttpClient);
        }

        public IList<T> GetItemsOfExpectedListingResponse<T>(params string[] responseFileNames) 
        {
            List<T> result = new();

            foreach (var responseFileName in responseFileNames)
            {
                string filePath = Path.Combine(Environment.CurrentDirectory, "Unit", "Data", _folder, responseFileName);

                var serverResponse = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(filePath));

                //listing endpoints always have two children. First are items and second is pagination
                var items = serverResponse.Values().First();

                result.AddRange(JsonConvert.DeserializeObject<List<T>>(items.ToString()));
            }

            return result;
        }

        public T GetExpectedResponse<T>(string responseFileName)
        {
            string filePath = Path.Combine(Environment.CurrentDirectory, "Unit", "Data", _folder, responseFileName);

            return JsonConvert.DeserializeObject<T>(File.ReadAllText(filePath));
        }

        public void Dispose()
        {
        }
    }
}
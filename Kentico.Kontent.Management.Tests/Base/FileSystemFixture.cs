using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Kentico.Kontent.Management.Configuration;
using Kentico.Kontent.Management.Modules.ActionInvoker;
using Kentico.Kontent.Management.Modules.HttpClient;
using Kentico.Kontent.Management.Modules.UrlBuilder;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NSubstitute;

namespace Kentico.Kontent.Management.Tests.Base
{
    public class FileSystemFixture : IDisposable
    {
        public string Endpoint => "https://manage.kontent.ai/v2";
        public string PROJECT_ID => "a9931a80-9af4-010b-0590-ecb1273cf1b8";
        public string SUBCRIPTION_ID => "9c7b9841-ea99-48a7-a46d-65b2549d6c0";

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
                ApiKey = "Dummy_API_key",
                ProjectId = PROJECT_ID,
                SubscriptionId = SUBCRIPTION_ID
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

        public IManagementClient CreateMockClientWithUrl(string expectedUrl)
        {
            var mockedHttpClient = Substitute.For<IManagementHttpClient>();
            mockedHttpClient.SendAsync(Arg.Any<IMessageCreator>(), Arg.Any<string>(), Arg.Any<HttpMethod>(), Arg.Any<HttpContent>(), Arg.Any<Dictionary<string, string>>())
             .Returns(x =>
             {
                 var url = x.ArgAt<string>(1);
                 url.Should().BeEquivalentTo(expectedUrl, "because url does not match");

                 var result = new HttpResponseMessage();

                 return Task.FromResult(result);
             });
            return CreateMockClient(mockedHttpClient);
        }

        public IManagementClient CreateMockClientWithResponse(params string[] responseFileNames)
        {
            List<HttpResponseMessage> responses = new();
            foreach (var responseFileName in responseFileNames)
            {
                string dataPath = Path.Combine(Environment.CurrentDirectory, "Data", _folder);

                var responsePath = Path.Combine(dataPath, responseFileName);
                var result = new HttpResponseMessage();
                result.Content = new StringContent(File.ReadAllText(responsePath));

                responses.Add(result);
            }

            var mockedHttpClient = Substitute.For<IManagementHttpClient>();
            mockedHttpClient.SendAsync(Arg.Any<IMessageCreator>(), Arg.Any<string>(), Arg.Any<HttpMethod>(), Arg.Any<HttpContent>(), Arg.Any<Dictionary<string, string>>())
             .Returns(responses.First(), responses.Skip(1).ToArray());
            return CreateMockClient(mockedHttpClient);
        }

        public IManagementClient CreateMockClientWithoutResponse()
        {
            var mockedHttpClient = Substitute.For<IManagementHttpClient>();
            mockedHttpClient.SendAsync(Arg.Any<IMessageCreator>(), Arg.Any<string>(), Arg.Any<HttpMethod>(), Arg.Any<HttpContent>(), Arg.Any<Dictionary<string, string>>())
             .Returns(x =>
             {
                 var result = new HttpResponseMessage();

                 return Task.FromResult(result);
             });
            return CreateMockClient(mockedHttpClient);
        }

        public IList<T> GetItemsOfExpectedListingResponse<T>(params string[] responseFileNames)
        {
            List<T> result = new();

            foreach (var responseFileName in responseFileNames)
            {
                string filePath = Path.Combine(Environment.CurrentDirectory, "Data", _folder, responseFileName);

                var serverResponse = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(filePath));

                //listing endpoints always have two children. First are items and second is pagination
                var items = serverResponse.Values().First();

                result.AddRange(JsonConvert.DeserializeObject<List<T>>(items.ToString()));
            }

            return result;
        }

        public T GetExpectedResponse<T>(string responseFileName)
        {
            string filePath = Path.Combine(Environment.CurrentDirectory, "Data", _folder, responseFileName);

            return JsonConvert.DeserializeObject<T>(File.ReadAllText(filePath));
        }

        public void Dispose()
        {
        }
    }
}
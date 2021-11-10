using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Kentico.Kontent.Management.Modules.ActionInvoker;
using Kentico.Kontent.Management.Modules.HttpClient;
using Kentico.Kontent.Management.UrlBuilder;
using Microsoft.Extensions.Configuration;
using NSubstitute;

namespace Kentico.Kontent.Management.Tests.Unit.Base
{
    public class FileSystemFixture : IDisposable
    {

        /// <summary>
        /// ID of the test project.
        /// </summary>
        public const string PROJECT_ID = "a9931a80-9af4-010b-0590-ecb1273cf1b8";
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

        public ManagementClient CreateMockClient(IManagementHttpClient httpClient)
        {
            var actionInvoker = new ActionInvoker(httpClient, _messageCreator);
            return new ManagementClient(_urlBuilder, actionInvoker);
        }

        public ManagementClient CreateDefaultMockClientRespondingWithFilename(string responseFileName)
        {
            var mockedHttpClient = Substitute.For<IManagementHttpClient>();
            mockedHttpClient.SendAsync(Arg.Any<IMessageCreator>(), Arg.Any<string>(), Arg.Any<HttpMethod>(), Arg.Any<HttpContent>(), Arg.Any<Dictionary<string, string>>())
             .Returns(x =>
                {
                    string dataPath = Path.Combine(Environment.CurrentDirectory, "Unit", "Data");

                    var responsePath = Path.Combine(dataPath, responseFileName);
                    var result = new HttpResponseMessage();
                    result.Content = new StringContent(File.ReadAllText(responsePath));

                    return Task.FromResult<HttpResponseMessage>(result);
                });
            return CreateMockClient(mockedHttpClient);
        }

        public void Dispose()
        {
        }
    }
}
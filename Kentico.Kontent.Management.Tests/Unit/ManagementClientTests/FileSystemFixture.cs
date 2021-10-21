using System;
using Kentico.Kontent.Management.Modules.ActionInvoker;
using Kentico.Kontent.Management.Modules.HttpClient;
using Kentico.Kontent.Management.UrlBuilder;
using Microsoft.Extensions.Configuration;

namespace Kentico.Kontent.Management.Tests.Unit.ManagementClientTests
{
    public class FileSystemFixture : IDisposable
    {

        /// <summary>
        /// ID of the test project.
        /// </summary>
        public const string PROJECT_ID = "a9931a80-9af4-010b-0590-ecb1273cf1b8";
        
        protected ManagementClient _client;

        protected IManagementHttpClient HttpClient { get; private set; }

        public void setMockedHttpClient(IManagementHttpClient httpClient)
        {

            var configuration = new ConfigurationBuilder()
                .AddUserSecrets<FileSystemFixture>()
                .Build();
            var options = new ManagementOptions() { ApiKey = configuration.GetValue<string>("ApiKey") ?? "Dummy_API_key", ProjectId = configuration.GetValue<string>("ProjectId") ?? PROJECT_ID };
            var urlBuilder = new EndpointUrlBuilder(options);
            var actionInvoker = new ActionInvoker(httpClient, new MessageCreator(options.ApiKey));

            _client = new ManagementClient(urlBuilder, actionInvoker);
        }

        public void Dispose()
        {
            _client = null;
        }
    }
}
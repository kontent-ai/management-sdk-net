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

        public void Dispose()
        {
        }
    }
}
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Kentico.Kontent.Management;
using Kentico.Kontent.Management.Models.Languages;
using Kentico.Kontent.Management.Models.Shared;
using Kentico.Kontent.Management.Modules.ActionInvoker;
using Kentico.Kontent.Management.Modules.HttpClient;
using Kentico.Kontent.Management.Tests.Mocks;
using Kentico.Kontent.Management.Tests.Unit.ManagementClientTests;
using Kentico.Kontent.Management.UrlBuilder;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Kentico.Kontent.Management.Tests.Unit.ManagementClientTests
{

    // public interface IHttpResponseMessageToJsonMapper
    // {
    //     Task<HttpResponseMessage> GetHttpResponseMessageAsync(HttpMethod method, string endpointUrl);
    // }

    // public class FileSystemManagementBase : IManagementHttpClient
    // {
    //     private IHttpResponseMessageToJsonMapper _mapper;
    //     public FileSystemManagementBase(IHttpResponseMessageToJsonMapper mapper)
    //     {
    //         _mapper = mapper;
    //     }

    //     public Task<HttpResponseMessage> SendAsync(IMessageCreator messageCreator, string endpointUrl, HttpMethod method, HttpContent content = null, Dictionary<string, string> headers = null)
    //     {
    //         if (_mapper == null)
    //         {
    //             Assert.True(false, "Please provide an implementation of the IHttpResponseMessageToJsonMapper (mapper property) to the test fixture.");
    //             return null;
    //         }

    //         return _mapper.GetHttpResponseMessageAsync(method, endpointUrl);
    //     }

    // }
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
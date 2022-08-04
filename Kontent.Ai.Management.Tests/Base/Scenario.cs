using Kontent.Ai.Management.Configuration;
using Kontent.Ai.Management.Modules.ActionInvoker;
using Kontent.Ai.Management.Modules.HttpClient;
using Kontent.Ai.Management.Modules.UrlBuilder;
using Newtonsoft.Json;
using NSubstitute;
using NSubstitute.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;

namespace Kontent.Ai.Management.Tests.Base;

internal class Scenario
{
    public static string Endpoint => "https://manage.kontent.ai/v2";
    public static string PROJECT_ID => "a9931a80-9af4-010b-0590-ecb1273cf1b8";
    public static string SUBCRIPTION_ID => "9c7b9841-ea99-48a7-a46d-65b2549d6c0";

    private readonly EndpointUrlBuilder _urlBuilder;
    private readonly MessageCreator _messageCreator;
    private readonly string _folder;

    private HttpClientMockData _clientData;
    private List<HttpResponseMessage> _responsesMessages;
    private List<string> _filePaths;

    public Scenario(string folder)
    {
        var managementOptions = new ManagementOptions()
        {
            ApiKey = "Dummy_API_key",
            ProjectId = PROJECT_ID,
            SubscriptionId = SUBCRIPTION_ID
        };
        _urlBuilder = new EndpointUrlBuilder(managementOptions);
        _messageCreator = new MessageCreator(managementOptions.ApiKey);
        _responsesMessages = new();
        _folder = Path.Combine(Environment.CurrentDirectory, "Data", folder);
    }

    public IManagementClient CreateManagementClient() => CreateMockClient(_urlBuilder, _messageCreator);

    public Expectations CreateExpectations() => new(_clientData, _filePaths);

    public Scenario WithResponses(params string[] responseFileNames)
    {
        _responsesMessages = new();
        _filePaths = new();

        foreach (var responseFileName in responseFileNames)
        {
            var filePath = Path.Combine(_folder, responseFileName);
            var response = File.ReadAllText(filePath);

            var result = new HttpResponseMessage
            {
                Content = new StringContent(response)
            };

            _filePaths.Add(filePath);
            _responsesMessages.Add(result);
        }

        return this;
    }

    public T GetExpectedResponse<T>(string responseFileName)
    {
        var filePath = Path.Combine(Environment.CurrentDirectory, "Data", _folder, responseFileName);

        return JsonConvert.DeserializeObject<T>(File.ReadAllText(filePath));
    }

    public T GetExpectedResponse<T>()
    {
        var filePath = Path.Combine(Environment.CurrentDirectory, "Data", _folder, _filePaths.First());

        return JsonConvert.DeserializeObject<T>(File.ReadAllText(filePath));
    }

    private IManagementClient CreateMockClient(EndpointUrlBuilder urlBuilder, MessageCreator messageCreator)
    {
        var mockedHttpClient = Substitute.For<IManagementHttpClient>();

        mockedHttpClient.SendAsync(
        Arg.Any<IMessageCreator>(),
        Arg.Any<string>(),
        Arg.Any<HttpMethod>(),
        Arg.Any<HttpContent>(),
        Arg.Any<Dictionary<string, string>>())
        .Returns(callInfo =>
        {
            var payload = callInfo.ArgAt<HttpContent>(3);

            _clientData = new(
                Url: callInfo.ArgAt<string>(1),
                HttpMethod: callInfo.ArgAt<HttpMethod>(2),
                Payload: payload?.ReadAsStringAsync().Result,
                Headers: callInfo.ArgAt<Dictionary<string, string>>(4));

            if (!_responsesMessages.Any())
            {
                return new HttpResponseMessage();
            }

            return _responsesMessages.First();

        }, GetRestOfResponses());


        var actionInvoker = new ActionInvoker(mockedHttpClient, messageCreator);
        return new ManagementClient(urlBuilder, actionInvoker);
    }

    private Func<CallInfo, HttpResponseMessage>[] GetRestOfResponses()
    {
        if (_responsesMessages.Count > 1)
        {
            return _responsesMessages.Skip(1)
                .Select<HttpResponseMessage, Func<CallInfo, HttpResponseMessage>>(response => callInfo => response).ToArray();
        }

        return Array.Empty<Func<CallInfo, HttpResponseMessage>>();
    }
}
using Kontent.Ai.Management.Api;
using Kontent.Ai.Management.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;

namespace Kontent.Ai.Management.Tests.Base;

internal class Scenario
{
    public static string Endpoint => "https://manage.kontent.ai/v2";
    public static string ENVIRONMENT_ID => "a9931a80-9af4-010b-0590-ecb1273cf1b8";
    public static string SUBSCRIPTION_ID => "9c7b9841-ea99-48a7-a46d-65b2549d6c0";

    private readonly ManagementOptions _managementOptions;
    private readonly string _folder;

    private HttpClientMockData _clientData;
    private List<HttpResponseMessage> _responsesMessages;
    private List<string> _filePaths;

    public Scenario(string folder)
    {
        _managementOptions = new ManagementOptions
        {
            ApiKey = "Dummy_API_key",
            EnvironmentId = ENVIRONMENT_ID,
            SubscriptionId = SUBSCRIPTION_ID
        };
        _responsesMessages = new();
        _folder = Path.Combine(Environment.CurrentDirectory, "Data", folder);
    }

    public IManagementClient CreateManagementClient()
    {
        var managementApi = ManagementApiFactory.Create(_managementOptions, new RefitMockHandler(_responsesMessages, RecordRefitRequest));
        var subscriptionApi = ManagementApiFactory.CreateSubscription(_managementOptions, new RefitMockHandler(_responsesMessages, RecordRefitRequest));
        return new ManagementClient(managementApi, subscriptionApi);
    }

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

    private void RecordRefitRequest(HttpRequestMessage request, string? payload) => _clientData = new HttpClientMockData(
        Url: request.RequestUri!.AbsoluteUri,
        HttpMethod: request.Method,
        Payload: payload,
        Headers: null);
}

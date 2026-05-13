using Kontent.Ai.Management.Api;
using Kontent.Ai.Management.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;

namespace Kontent.Ai.Management.Tests.Base;

public sealed class FileSystemFixture : IDisposable
{
    public static string Endpoint => "https://manage.kontent.ai/v2";
    public static string ENVIRONMENT_ID => "a9931a80-9af4-010b-0590-ecb1273cf1b8";
    public static string SUBCRIPTION_ID => "9c7b9841-ea99-48a7-a46d-65b2549d6c0";

    private string _folder = "";
    private readonly ManagementOptions _managementOptions;

    public FileSystemFixture()
    {
        _managementOptions = new ManagementOptions
        {
            ApiKey = "Dummy_API_key",
            EnvironmentId = ENVIRONMENT_ID,
            SubscriptionId = SUBCRIPTION_ID,
        };
    }

    public void SetSubFolder(string folder) => _folder = folder;

    public IManagementClient CreateMockClient() => BuildClient(Array.Empty<HttpResponseMessage>());

    public IManagementClient CreateMockClientWithoutResponse() => BuildClient(Array.Empty<HttpResponseMessage>());

    public IManagementClient CreateMockClientWithResponse(params string[] responseFileNames)
    {
        var responses = new List<HttpResponseMessage>();
        foreach (var responseFileName in responseFileNames)
        {
            var responsePath = Path.Combine(Environment.CurrentDirectory, "Data", _folder, responseFileName);
            responses.Add(new HttpResponseMessage
            {
                Content = new StringContent(File.ReadAllText(responsePath)),
            });
        }

        return BuildClient(responses);
    }

    private IManagementClient BuildClient(IReadOnlyList<HttpResponseMessage> refitResponses)
    {
        var managementApi = ManagementApiFactory.Create(_managementOptions, new RefitMockHandler(refitResponses));
        var subscriptionApi = ManagementApiFactory.CreateSubscription(_managementOptions, new RefitMockHandler(refitResponses));
        return new ManagementClient(managementApi, subscriptionApi);
    }

    public IList<T> GetItemsOfExpectedListingResponse<T>(params string[] responseFileNames)
    {
        var result = new List<T>();

        foreach (var responseFileName in responseFileNames)
        {
            var filePath = Path.Combine(Environment.CurrentDirectory, "Data", _folder, responseFileName);

            var serverResponse = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(filePath));

            //listing endpoints always have two children. First are items and second is pagination
            var items = serverResponse.Values().First();

            result.AddRange(JsonConvert.DeserializeObject<List<T>>(items.ToString()));
        }

        return result;
    }

    public T GetExpectedResponse<T>(string responseFileName)
    {
        var filePath = Path.Combine(Environment.CurrentDirectory, "Data", _folder, responseFileName);

        return JsonConvert.DeserializeObject<T>(File.ReadAllText(filePath));
    }

    public void Dispose()
    {
    }
}

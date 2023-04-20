using FluentAssertions;
using FluentAssertions.Execution;
using Kontent.Ai.Management.Tests.Base.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;

namespace Kontent.Ai.Management.Tests.Base;

internal class Expectations
{
    private readonly HttpClientMockData _httpClientMockData;
    private readonly IReadOnlyCollection<string> _filePaths;
    private readonly JsonSerializerSettings _deserializeSettings;
    private string _expectedUrl;
    private object _expectedRequest;
    private Dictionary<string, string> _expectedHeaders;
    private HttpMethod _expectedHttpMethod;
    private List<object> _expectedResponse;
    private object _resposne;
    private object _request;

    public Expectations(HttpClientMockData httpClientMockData, IReadOnlyCollection<string> filePaths)
    {
        _httpClientMockData = httpClientMockData;
        _filePaths = filePaths;
        _expectedUrl = default;
        _expectedRequest = default;
        _expectedHeaders = default;
        _expectedHttpMethod = default;
        _expectedResponse = default;
        _deserializeSettings = new()
        {
            Converters = new List<JsonConverter>
            {
                new ContentTypeOperationBaseModelConverter(),
                new TaxonomyGroupOperationBaseModelConverter(),
                new ContentTypeSnippetOperationBaseModelConverter(),
                new AssetFolderOperationBaseModelConverter()
            }
        };
    }

    public Expectations Url(string url)
    {
        _expectedUrl = url;
        return this;
    }

    public Expectations RequestPayload<T>(T requestPayload, Func<string, T> deserializer) where T : class
    {
        _expectedRequest = deserializer(_httpClientMockData.Payload);
        _request = requestPayload;

        return this;
    }

    public Expectations RequestPayload<T>(T requestPayload) where T : class
    {
        try
        {
            _expectedRequest = JsonConvert.DeserializeObject<T>(_httpClientMockData.Payload, _deserializeSettings);
        }
        catch
        {
            _expectedRequest = _httpClientMockData.Payload;
        }

        _request = JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(requestPayload), _deserializeSettings);

        return this;
    }

    public Expectations Response<T>(T response)
    {
        if (_filePaths.Count > 0)
        {
            _expectedResponse = new List<object> { JsonConvert.DeserializeObject<T>(File.ReadAllText(_filePaths.First())) };
        }

        _resposne = new List<object> { response };

        return this;
    }

    public Expectations Response<T>(T response, T expected)
    {

        _expectedResponse = new List<object> { expected };

        _resposne = new List<object> { response };

        return this;
    }

    public Expectations ListingResponse<T>(IEnumerable<T> response)
    {
        List<T> result = new();

        foreach (var filePath in _filePaths)
        {
            var serverResponse = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(filePath));

            //listing endpoints always have two children. First are items and second is pagination
            var items = serverResponse.Values().First();

            result.AddRange(JsonConvert.DeserializeObject<List<T>>(items.ToString()));
        }

        _expectedResponse = result.Select(x => (object)x).ToList();

        _resposne = response;

        return this;
    }

    public Expectations Headers(Dictionary<string, string> headers)
    {
        _expectedHeaders = headers;
        return this;
    }

    public Expectations HttpMethod(HttpMethod httpMethod)
    {
        _expectedHttpMethod = httpMethod;
        return this;
    }

    public void Validate()
    {
        using var scope = new AssertionScope();

        _httpClientMockData.Url.Should().Be(_expectedUrl, "the URL should be the same as the expected one");
        _httpClientMockData.HttpMethod.Should().Be(_expectedHttpMethod, "the HTTP method should be the same as the expected one");
        _httpClientMockData.Headers.Should().BeEquivalentTo(_expectedHeaders, "The headers should be the same as the expected ones");
        _request.Should().BeEquivalentTo(_expectedRequest);
        _resposne.Should().BeEquivalentTo(_expectedResponse, "the response should be the same as the expected one");
    }
}

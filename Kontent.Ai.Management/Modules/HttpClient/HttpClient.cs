﻿using System.Net.Http;
using System.Threading.Tasks;

namespace Kontent.Ai.Management.Modules.HttpClient;

internal class HttpClient : IHttpClient
{
    public HttpClient()
    {

    }
    public HttpClient(System.Net.Http.HttpClient httpClient)
    {
        _baseClient = httpClient;
    }

    private readonly System.Net.Http.HttpClient _baseClient = new();

    public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request) => await _baseClient.SendAsync(request);
}

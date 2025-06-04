using System.Net.Http;
using System.Threading.Tasks;

namespace Kontent.Ai.Management.Modules.HttpClient;

internal class HttpClient(System.Net.Http.HttpClient httpClient) : IHttpClient
{
    private readonly System.Net.Http.HttpClient _baseClient = httpClient;

    public HttpClient() : this(new System.Net.Http.HttpClient())
    {
    }

    public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request) => await _baseClient.SendAsync(request);
}

using System.Net.Http;
using System.Threading.Tasks;

namespace Kontent.Ai.Management.Modules.HttpClient;

internal interface IHttpClient
{
    Task<HttpResponseMessage> SendAsync(HttpRequestMessage request);
}

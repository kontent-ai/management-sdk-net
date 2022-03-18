using System.Net.Http;
using System.Threading.Tasks;

namespace Kentico.Kontent.Management.Modules.HttpClient;

internal interface IHttpClient
{
    Task<HttpResponseMessage> SendAsync(HttpRequestMessage request);
}

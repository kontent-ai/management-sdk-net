using System.Net.Http;
using System.Threading.Tasks;

namespace Kentico.Kontent.Management.Modules.HttpClient
{
    internal class HttpClient : IHttpClient
    {
        private readonly System.Net.Http.HttpClient _baseClient = new System.Net.Http.HttpClient();
        
        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            return await _baseClient.SendAsync(request);
        }
    }
}

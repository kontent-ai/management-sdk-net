using System.Net.Http;
using System.Threading.Tasks;

namespace KenticoCloud.ContentManagement.Modules.HttpClient
{
    internal interface IHttpClient
    {
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request);
    }
}

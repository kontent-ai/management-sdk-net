using System.Net.Http;
using System.Threading.Tasks;

namespace KenticoCloud.ContentManagement.Modules.HttpClient
{
    internal interface IContentManagementHttpClient
    {
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage message);
    }
}

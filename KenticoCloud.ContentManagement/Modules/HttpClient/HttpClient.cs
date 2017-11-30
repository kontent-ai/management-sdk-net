using System.Net.Http;
using System.Threading.Tasks;

namespace KenticoCloud.ContentManagement.Modules.HttpClient
{
    internal class HttpClient : IHttpClient
    {
        private static System.Net.Http.HttpClient _baseClient;

        private static System.Net.Http.HttpClient BaseClient
        {
            get
            {
                if (_baseClient == null)
                {
                    _baseClient = new System.Net.Http.HttpClient();
                }
                return _baseClient;
            }
        }

        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            return BaseClient.SendAsync(request);
        }
    }
}

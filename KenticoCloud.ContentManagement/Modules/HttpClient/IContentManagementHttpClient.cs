using System.Net.Http;
using System.Threading.Tasks;
using KenticoCloud.ContentManagement.Modules.ActionInvoker;

namespace KenticoCloud.ContentManagement.Modules.HttpClient
{
    /// <summary>
    /// The interface for a content management http client.
    /// </summary>
    public interface IContentManagementHttpClient
    {
        /// <summary>
        /// Send an HTTP request to get an HTTP response.
        /// </summary>
        /// <param name="messageCreator">The message creator.</param>
        /// <param name="endpointUrl">The url to make request to.</param>
        /// <param name="method">The HTTP method.</param>
        /// <param name="content">The HTTP content.</param>
        /// <returns></returns>
        Task<HttpResponseMessage> SendAsync(
            IMessageCreator messageCreator,
            string endpointUrl,
            HttpMethod method,
            HttpContent content = null);
    }
}

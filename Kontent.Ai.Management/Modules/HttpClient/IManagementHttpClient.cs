using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Kontent.Ai.Management.Modules.ActionInvoker;

namespace Kontent.Ai.Management.Modules.HttpClient;

/// <summary>
/// The interface for a content management http client.
/// </summary>
public interface IManagementHttpClient
{
    /// <summary>
    /// Send an HTTP request to get an HTTP response.
    /// </summary>
    /// <param name="messageCreator">The message creator.</param>
    /// <param name="endpointUrl">The url to make request to.</param>
    /// <param name="method">The HTTP method.</param>
    /// <param name="content">The HTTP content.</param>
    /// <param name="headers">Additional HTTP headers if needed.</param>
    /// <returns></returns>
    Task<HttpResponseMessage> SendAsync(
        IMessageCreator messageCreator,
        string endpointUrl,
        HttpMethod method,
        HttpContent content = null,
        Dictionary<string, string> headers = null);
}

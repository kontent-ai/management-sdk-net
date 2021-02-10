using System.Collections.Generic;
using System.Net.Http;

namespace Kentico.Kontent.Management.Modules.ActionInvoker
{
    /// <summary>
    /// Interface for a creator of <see cref="HttpRequestMessage"/>.
    /// </summary>
    public interface IMessageCreator
    {
        /// <summary>
        /// Create an <see cref="HttpRequestMessage"/>.
        /// </summary>
        /// <param name="method">The HTTP method.</param>
        /// <param name="url">The url as a string.</param>
        /// <param name="content">The HTTP content.</param>
        /// <param name="headers">HTTP Headers (optional).</param>
        /// <returns></returns>
        HttpRequestMessage CreateMessage(HttpMethod method, string url, HttpContent content = null, Dictionary<string,string> headers = null);
    }
}
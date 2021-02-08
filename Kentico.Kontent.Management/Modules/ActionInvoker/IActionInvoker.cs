using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kentico.Kontent.Management.Modules.ActionInvoker
{
    internal interface IActionInvoker
    {
        Task<TResponse> InvokeMethodAsync<TPayload, TResponse>(string endpointUrl, HttpMethod method, TPayload body);

        Task<TResponse> InvokeReadOnlyMethodAsync<TResponse>(string endpointUrl, HttpMethod method, Dictionary<string, string> headers = null);

        Task InvokeMethodAsync(string endpointUrl, HttpMethod method, Dictionary<string, string> headers = null);

        Task<TResponse> UploadFileAsync<TResponse>(string endpointUrl, Stream stream, string contentType);
    }
}

using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace KenticoCloud.ContentManagement.Modules.ActionInvoker
{
    internal interface IActionInvoker
    {
        Task<TResponse> InvokeMethodAsync<TPayload, TResponse>(string endpointUrl, HttpMethod method, TPayload body);

        Task<TResponse> InvokeReadOnlyMethodAsync<TResponse>(string endpointUrl, HttpMethod method);

        Task InvokeMethodAsync(string endpointUrl, HttpMethod method);

        Task<TResponse> UploadFileAsync<TResponse>(string endpointUrl, Stream stream, string contentType);
    }
}

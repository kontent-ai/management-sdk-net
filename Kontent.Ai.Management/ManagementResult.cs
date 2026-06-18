using System.Net;
using System.Net.Http.Headers;

namespace Kontent.Ai.Management;

internal sealed class ManagementResult : IManagementResult
{
    public bool IsSuccess { get; }
    public IError? Error { get; }
    public HttpStatusCode? StatusCode { get; }
    public string? RequestUrl { get; }
    public HttpResponseHeaders? ResponseHeaders { get; }

    private ManagementResult(bool isSuccess, IError? error, HttpStatusCode? statusCode, string? requestUrl, HttpResponseHeaders? responseHeaders)
    {
        IsSuccess = isSuccess;
        Error = error;
        StatusCode = statusCode;
        RequestUrl = requestUrl;
        ResponseHeaders = responseHeaders;
    }

    public static ManagementResult Success(HttpStatusCode? statusCode = null, string? requestUrl = null, HttpResponseHeaders? responseHeaders = null) =>
        new(true, null, statusCode, requestUrl, responseHeaders);

    public static ManagementResult Failure(IError error, HttpStatusCode? statusCode = null, string? requestUrl = null, HttpResponseHeaders? responseHeaders = null) =>
        new(false, error, statusCode, requestUrl, responseHeaders);
}

internal sealed class ManagementResult<T> : IManagementResult<T>
{
    public bool IsSuccess { get; }
    public T Value { get; }
    public IError? Error { get; }
    public HttpStatusCode? StatusCode { get; }
    public string? RequestUrl { get; }
    public HttpResponseHeaders? ResponseHeaders { get; }

    private ManagementResult(bool isSuccess, T value, IError? error, HttpStatusCode? statusCode, string? requestUrl, HttpResponseHeaders? responseHeaders)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
        StatusCode = statusCode;
        RequestUrl = requestUrl;
        ResponseHeaders = responseHeaders;
    }

    public static ManagementResult<T> Success(T value, HttpStatusCode? statusCode = null, string? requestUrl = null, HttpResponseHeaders? responseHeaders = null) =>
        new(true, value, null, statusCode, requestUrl, responseHeaders);

    public static ManagementResult<T> Failure(IError error, HttpStatusCode? statusCode = null, string? requestUrl = null, HttpResponseHeaders? responseHeaders = null) =>
        new(false, default!, error, statusCode, requestUrl, responseHeaders);
}

using System.Net;

namespace Kontent.Ai.Management;

internal sealed class ManagementResult : IManagementResult
{
    public bool IsSuccess { get; }
    public IError? Error { get; }
    public HttpStatusCode? StatusCode { get; }
    public string? RequestUrl { get; }

    private ManagementResult(bool isSuccess, IError? error, HttpStatusCode? statusCode, string? requestUrl)
    {
        IsSuccess = isSuccess;
        Error = error;
        StatusCode = statusCode;
        RequestUrl = requestUrl;
    }

    public static ManagementResult Success(HttpStatusCode? statusCode = null, string? requestUrl = null) =>
        new(true, null, statusCode, requestUrl);

    public static ManagementResult Failure(IError error, HttpStatusCode? statusCode = null, string? requestUrl = null) =>
        new(false, error, statusCode, requestUrl);
}

internal sealed class ManagementResult<T> : IManagementResult<T>
{
    public bool IsSuccess { get; }
    public T Value { get; }
    public IError? Error { get; }
    public HttpStatusCode? StatusCode { get; }
    public string? RequestUrl { get; }

    private ManagementResult(bool isSuccess, T value, IError? error, HttpStatusCode? statusCode, string? requestUrl)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
        StatusCode = statusCode;
        RequestUrl = requestUrl;
    }

    public static ManagementResult<T> Success(T value, HttpStatusCode? statusCode = null, string? requestUrl = null) =>
        new(true, value, null, statusCode, requestUrl);

    public static ManagementResult<T> Failure(IError error, HttpStatusCode? statusCode = null, string? requestUrl = null) =>
        new(false, default!, error, statusCode, requestUrl);
}

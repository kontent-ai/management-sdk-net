using System.Net;

namespace Kontent.Ai.Management;

/// <summary>
/// The outcome of a Management SDK operation. Management API responses — including validation failures and other
/// 4xx/5xx errors — are surfaced here without throwing; inspect <see cref="IsSuccess"/> rather than catching.
/// Transport-level failures, where no HTTP response is received, still surface as exceptions.
/// </summary>
public interface IManagementResult
{
    /// <summary>
    /// Whether the operation succeeded. When <c>false</c>, <see cref="Error"/> describes the failure.
    /// </summary>
    bool IsSuccess { get; }

    /// <summary>
    /// The failure detail when <see cref="IsSuccess"/> is <c>false</c>; <c>null</c> on success.
    /// </summary>
    IError? Error { get; }

    /// <summary>
    /// The HTTP status code of the Management API response. <c>null</c> when the operation failed before a request
    /// was sent — local content-item validation performed by the SDK.
    /// </summary>
    HttpStatusCode? StatusCode { get; }

    /// <summary>
    /// The request URL, for diagnostics. <c>null</c> when no request was sent.
    /// </summary>
    string? RequestUrl { get; }
}

/// <summary>
/// The outcome of a Management SDK operation that yields a value on success.
/// </summary>
/// <typeparam name="T">The value type carried on success.</typeparam>
public interface IManagementResult<out T> : IManagementResult
{
    /// <summary>
    /// The result value when <see cref="IManagementResult.IsSuccess"/> is <c>true</c>; <c>default</c> on failure.
    /// </summary>
    T Value { get; }
}

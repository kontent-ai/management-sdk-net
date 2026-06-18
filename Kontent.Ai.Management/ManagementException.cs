using System.Net;

namespace Kontent.Ai.Management;

/// <summary>
/// Thrown by <see cref="ManagementResultExtensions.EnsureSuccess(IManagementResult)"/> when a result represents a
/// failure. The SDK never throws this on its own — operations return <see cref="IManagementResult"/> and report
/// failures via <see cref="IManagementResult.IsSuccess"/>; this is opt-in for callers who prefer exceptions.
/// </summary>
public sealed class ManagementException : Exception
{
    /// <summary>The failure detail carried by the originating result.</summary>
    public IError Error { get; }

    /// <summary>The HTTP status code, when the failure came from an HTTP response.</summary>
    public HttpStatusCode? StatusCode { get; }

    /// <summary>The request URL, when one was sent.</summary>
    public string? RequestUrl { get; }

    internal ManagementException(IManagementResult result)
        : base($"Management API request failed ({result.StatusCode}): {result.Error?.Message}", result.Error?.Exception)
    {
        Error = result.Error!;
        StatusCode = result.StatusCode;
        RequestUrl = result.RequestUrl;
    }
}

using System.Net;

namespace Kontent.Ai.Management;

/// <summary>
/// Thrown when a failed <see cref="IManagementResult"/> is surfaced through an API that has no result channel to
/// carry it — currently the <see cref="Extensions.ListingExtensions.Items{T}"/> listing extension, which flattens a
/// page stream and so cannot return a mid-enumeration failure as data. Inspect <see cref="Error"/> for the detail.
/// </summary>
public sealed class ManagementResultException : Exception
{
    /// <summary>
    /// The failure detail of the result that could not be surfaced as data.
    /// </summary>
    public IError Error { get; }

    /// <summary>
    /// The HTTP status code of the failed response; <c>null</c> when the failure had no HTTP response.
    /// </summary>
    public HttpStatusCode? StatusCode { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ManagementResultException"/> class.
    /// </summary>
    /// <param name="error">The failure detail.</param>
    /// <param name="statusCode">The HTTP status code of the failed response, when any.</param>
    public ManagementResultException(IError error, HttpStatusCode? statusCode = null)
        : base(error?.Message ?? "The Management API operation failed.")
    {
        ArgumentNullException.ThrowIfNull(error);
        Error = error;
        StatusCode = statusCode;
    }
}

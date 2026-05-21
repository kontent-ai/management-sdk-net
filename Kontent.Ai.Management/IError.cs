namespace Kontent.Ai.Management;

/// <summary>
/// Describes why a Management SDK operation failed. Carried by a failed <see cref="IManagementResult"/>.
/// </summary>
public interface IError
{
    /// <summary>
    /// A human-readable description of the failure. Always populated.
    /// </summary>
    string Message { get; }

    /// <summary>
    /// The Management API request identifier, useful when reporting an issue. <c>null</c> when the failure did not
    /// originate from a Management API response.
    /// </summary>
    string? RequestId { get; }

    /// <summary>
    /// The Management API's internal error code — a Kontent.ai diagnostic code, not the HTTP status code.
    /// <c>null</c> when the failure did not originate from a Management API response.
    /// </summary>
    int? ErrorCode { get; }

    /// <summary>
    /// Individual validation failures. Populated for requests rejected by Management API validation and for
    /// content-item validation performed by the SDK before a request is sent; empty otherwise.
    /// </summary>
    IReadOnlyList<ValidationError> ValidationErrors { get; }

    /// <summary>
    /// The underlying exception, when the failure originated from a transport error or an unparseable response.
    /// <c>null</c> when the failure was a well-formed Management API error response.
    /// </summary>
    Exception? Exception { get; }
}

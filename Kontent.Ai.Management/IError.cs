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
    /// Individual validation failures reported by Management API validation; empty otherwise.
    /// </summary>
    IReadOnlyList<ValidationError> ValidationErrors { get; }

    /// <summary>
    /// The underlying exception captured for the failure, when one is available — for a Management API error
    /// response this carries the raw HTTP response for diagnostics. <c>null</c> when no exception was captured.
    /// </summary>
    Exception? Exception { get; }
}

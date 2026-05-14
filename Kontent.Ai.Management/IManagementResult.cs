using System.Net;

namespace Kontent.Ai.Management;

/// <summary>
/// Result of a Management SDK operation that surfaces failures without throwing — currently produced by the
/// content-item validator, and (from phase 4 onward) by strongly-typed <c>IManagementClient</c> methods that
/// accept or return generated content-type records.
/// </summary>
/// <typeparam name="T">The value type carried on success.</typeparam>
public interface IManagementResult<out T>
{
    /// <summary>
    /// Whether the operation succeeded. When <c>true</c>, <see cref="Value"/> is populated and <see cref="Errors"/>
    /// is empty; when <c>false</c>, <see cref="Errors"/> describes the failures and <see cref="Value"/> is
    /// <c>default(T)</c>.
    /// </summary>
    bool IsSuccess { get; }

    /// <summary>
    /// The result value when <see cref="IsSuccess"/> is <c>true</c>. May be <c>default(T)</c> on failure.
    /// </summary>
    T Value { get; }

    /// <summary>
    /// Errors collected during the operation. Empty when <see cref="IsSuccess"/> is <c>true</c>.
    /// </summary>
    IReadOnlyList<ManagementError> Errors { get; }

    /// <summary>
    /// HTTP status code when the failure originated from a Management API response.
    /// <c>null</c> when the failure originated before an HTTP call (e.g., from local validation).
    /// </summary>
    HttpStatusCode? StatusCode { get; }
}

/// <summary>
/// A single error within an <see cref="IManagementResult{T}"/>. Carries a human-readable message and, when
/// applicable, the codename of the offending element and a machine-readable code.
/// </summary>
/// <param name="Message">Human-readable description of the failure.</param>
/// <param name="ElementCodename">
/// Codename of the element the error pertains to. <c>null</c> when the error is not element-scoped
/// (e.g., top-level authorization or rate-limit failures).
/// </param>
/// <param name="Code">Machine-readable error code, when the API or validator supplies one.</param>
public sealed record ManagementError(
    string Message,
    string? ElementCodename = null,
    string? Code = null);

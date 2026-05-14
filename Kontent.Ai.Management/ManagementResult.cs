using System.Net;

namespace Kontent.Ai.Management;

internal sealed class ManagementResult<T> : IManagementResult<T>
{
    public bool IsSuccess { get; }
    public T Value { get; }
    public IReadOnlyList<ManagementError> Errors { get; }
    public HttpStatusCode? StatusCode { get; }

    private ManagementResult(bool isSuccess, T value, IReadOnlyList<ManagementError> errors, HttpStatusCode? statusCode)
    {
        IsSuccess = isSuccess;
        Value = value;
        Errors = errors;
        StatusCode = statusCode;
    }

    public static ManagementResult<T> Success(T value, HttpStatusCode? statusCode = null) =>
        new(true, value, Array.Empty<ManagementError>(), statusCode);

    public static ManagementResult<T> Failure(IReadOnlyList<ManagementError> errors, HttpStatusCode? statusCode = null)
    {
        ArgumentNullException.ThrowIfNull(errors);
        if (errors.Count == 0)
        {
            throw new ArgumentException("A failed result must carry at least one error.", nameof(errors));
        }

        return new(false, default!, errors, statusCode);
    }
}

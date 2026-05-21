using System.Text.Json.Serialization;

namespace Kontent.Ai.Management;

/// <inheritdoc cref="IError" />
internal sealed record Error : IError
{
    /// <inheritdoc />
    [JsonPropertyName("message")]
    public string Message { get; init; } = "Unknown error.";

    /// <inheritdoc />
    [JsonPropertyName("request_id")]
    public string? RequestId { get; init; }

    /// <inheritdoc />
    [JsonPropertyName("error_code")]
    public int? ErrorCode { get; init; }

    /// <inheritdoc />
    [JsonPropertyName("validation_errors")]
    public IReadOnlyList<ValidationError> ValidationErrors { get; init; } = [];

    /// <inheritdoc />
    [JsonIgnore]
    public Exception? Exception { get; init; }
}

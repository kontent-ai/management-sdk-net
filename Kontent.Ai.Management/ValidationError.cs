
namespace Kontent.Ai.Management;

/// <summary>
/// A single validation failure within an <see cref="IError"/>.
/// </summary>
public sealed record ValidationError
{
    /// <summary>
    /// A human-readable description of the validation failure.
    /// </summary>
    [JsonPropertyName("message")]
    public string Message { get; init; } = "";

    /// <summary>
    /// Locates the failure — a path into the request body the Management API rejected.
    /// </summary>
    [JsonPropertyName("path")]
    public string? Path { get; init; }

    /// <summary>
    /// The line in the request body the failure refers to, when the Management API supplies one.
    /// </summary>
    [JsonPropertyName("line")]
    public int? Line { get; init; }

    /// <summary>
    /// The position on the line the failure refers to, when the Management API supplies one.
    /// </summary>
    [JsonPropertyName("position")]
    public int? Position { get; init; }
}

using System.Text.Json.Serialization;

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
    /// Locates the failure. For a Management API response this is a path into the request body; for content-item
    /// validation performed by the SDK it is the codename of the offending element.
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

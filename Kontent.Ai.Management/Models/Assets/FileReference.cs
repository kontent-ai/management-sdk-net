using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.Assets;

/// <summary>
/// Represents binary file reference which can be used in an Asset to point it to a specific binary file.
/// </summary>
public sealed record FileReference
{
    /// <summary>
    /// Gets the id of the binary file.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; init; }

    /// <summary>
    /// Gets file reference type.
    /// </summary>
    [JsonPropertyName("type")]
    public FileReferenceTypeEnum Type { get; init; }
}

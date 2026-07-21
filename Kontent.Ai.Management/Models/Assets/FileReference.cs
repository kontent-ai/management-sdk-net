
namespace Kontent.Ai.Management.Models.Assets;

/// <summary>
/// Reference to a previously uploaded binary file, used to attach it to an asset.
/// </summary>
public sealed record FileReference
{
    /// <summary>
    /// Binary file ID returned by the upload endpoint.
    /// </summary>
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    /// <summary>
    /// Reference type. Defaults to <see cref="FileReferenceType.Internal"/> — the only type the API uses — so callers never set it.
    /// </summary>
    [JsonPropertyName("type")]
    public FileReferenceType Type { get; init; } = FileReferenceType.Internal;
}

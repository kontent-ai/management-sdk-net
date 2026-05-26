
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
    /// Reference type. Always <see cref="FileReferenceTypeEnum.Internal"/>.
    /// </summary>
    [JsonPropertyName("type")]
    public required FileReferenceTypeEnum Type { get; init; }
}

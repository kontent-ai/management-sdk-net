using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.Assets;

/// <summary>
/// Represents the language specific description for the asset.
/// </summary>
public sealed record AssetDescription
{
    /// <summary>
    /// Gets the identifier of the language.
    /// </summary>
    [JsonProperty("language", Required = Required.Always)]
    public Reference Language { get; init; }

    /// <summary>
    /// Gets the description of the asset.
    /// </summary>
    [JsonProperty("description", Required = Required.AllowNull)]
    public string Description { get; init; }
}

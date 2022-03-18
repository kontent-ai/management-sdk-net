using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Assets;

/// <summary>
/// Represents binary file reference which can be used in an Asset to point it to a specific binary file.
/// </summary>
public sealed class FileReference
{
    /// <summary>
    /// Gets or sets the id of the binary file.
    /// </summary>
    [JsonProperty("id", Required = Required.Always)]
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets file reference type.
    /// </summary>
    [JsonProperty("type", Required = Required.Always)]
    public FileReferenceTypeEnum Type { get; set; }
}

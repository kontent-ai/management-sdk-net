namespace Kontent.Ai.Management.Models.Assets;

/// <summary>
/// Request payload for creating an asset (after the binary file has been uploaded).
/// </summary>
public sealed record AssetCreateModel
{
    /// <summary>
    /// Reference to the previously uploaded binary file.
    /// </summary>
    [JsonPropertyName("file_reference")]
    public required FileReference FileReference { get; init; }

    /// <summary>
    /// Per-language alt-text descriptions.
    /// </summary>
    [JsonPropertyName("descriptions")]
    public IEnumerable<AssetDescription>? Descriptions { get; init; }

    /// <summary>
    /// Display title.
    /// </summary>
    [JsonPropertyName("title")]
    public string? Title { get; init; }

    /// <summary>
    /// Folder to place the asset in. Use <c>00000000-0000-0000-0000-000000000000</c> as the ID to place at the top level.
    /// </summary>
    [JsonPropertyName("folder")]
    public Reference? Folder { get; init; }

    /// <summary>
    /// Asset collection to assign the asset to. Defaults server-side to the project's default collection when omitted.
    /// </summary>
    [JsonPropertyName("collection")]
    public AssetCollectionReference? Collection { get; init; }

    /// <summary>
    /// Caller-supplied external ID for the asset.
    /// </summary>
    [JsonPropertyName("external_id")]
    public string? ExternalId { get; init; }

    /// <summary>
    /// Taxonomy assignments from the environment's asset type.
    /// </summary>
    [JsonPropertyName("elements")]
    public IEnumerable<AssetElement>? Elements { get; init; }

    /// <summary>
    /// Caller-supplied codename. When omitted, the CMS generates one from the title (or file name).
    /// </summary>
    [JsonPropertyName("codename")]
    public string? Codename { get; init; }
}

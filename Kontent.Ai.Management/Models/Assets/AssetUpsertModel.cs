namespace Kontent.Ai.Management.Models.Assets;

/// <summary>
/// Request payload for upserting an asset by external ID.
/// </summary>
public sealed record AssetUpsertModel
{
    /// <summary>
    /// Per-language alt-text descriptions. Omit to leave existing descriptions unchanged; passing an empty array also retains existing descriptions server-side.
    /// </summary>
    [JsonPropertyName("descriptions")]
    public IReadOnlyList<AssetDescription>? Descriptions { get; init; }

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
    /// Asset collection to assign the asset to.
    /// </summary>
    [JsonPropertyName("collection")]
    public AssetCollectionReference? Collection { get; init; }

    /// <summary>
    /// Taxonomy assignments from the environment's asset type.
    /// </summary>
    [JsonPropertyName("elements")]
    public IReadOnlyList<AssetTaxonomyElement>? Elements { get; init; }

    /// <summary>
    /// Reference to the previously uploaded binary file. Omit when upserting metadata only — the existing binary stays attached.
    /// </summary>
    [JsonPropertyName("file_reference")]
    public FileReference? FileReference { get; init; }

    /// <summary>
    /// Caller-supplied codename. When omitted, the CMS generates one from the title (or file name).
    /// </summary>
    [JsonPropertyName("codename")]
    public string? Codename { get; init; }
}

using Kontent.Ai.Management.Configuration;
using Kontent.Ai.Management.Models.TaxonomyGroups;
using Kontent.Ai.Management.Models.Types;
using Kontent.Ai.Management.Models.TypeSnippets;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace Kontent.Ai.Management.Models.ContentModel;

/// <summary>
/// A complete, serializable snapshot of an environment's content model — its content types, snippets and taxonomy
/// groups. Produced by <see cref="Extensions.ContentModelExtensions.ExportContentModelAsync"/>, e.g. for committing
/// so content-model changes show up as reviewable diffs.
/// </summary>
/// <remarks>
/// <para>
/// <b>Experimental.</b> The snapshot feature and its JSON format are not yet a supported contract and may change or
/// be removed in a future release. Suppress the <c>KAIM001</c> diagnostic to opt in.
/// </para>
/// <para>
/// The JSON form produced by <see cref="ToJson"/> is versioned and byte-deterministic: entities are wire-faithful
/// (the shapes and identifiers the Management API returns, including environment-specific ids), collections are
/// ordered by codename, and serialization is pinned — the same environment state always yields the same string, so
/// committed snapshots diff meaningfully. Breaking changes to the format bump <see cref="CurrentVersion"/>.
/// </para>
/// <para>
/// Ids are environment-specific: a snapshot belongs to one environment lineage, and pulling the "same" model from an
/// API-managed clone produces a large diff because every id differs.
/// </para>
/// </remarks>
[Experimental("KAIM001")]
public sealed record ContentModelSnapshot
{
    /// <summary>The snapshot format version this SDK produces and the highest it can read.</summary>
    public const int CurrentVersion = 1;

    // WriteIndented is the only divergence from the SDK's wire options; both are pinned here so the snapshot format
    // stays byte-deterministic and self-contained (the element polymorphism and enum converters are SDK-internal).
    private static readonly JsonSerializerOptions SerializerOptions = CreateSerializerOptions();

    /// <summary>The snapshot format version. See <see cref="CurrentVersion"/>.</summary>
    [JsonPropertyName("version")]
    public int Version { get; init; } = CurrentVersion;

    /// <summary>All content types in the environment, ordered by codename.</summary>
    [JsonPropertyName("types")]
    public required IReadOnlyList<ContentTypeModel> Types { get; init; }

    /// <summary>All content type snippets in the environment, ordered by codename.</summary>
    [JsonPropertyName("snippets")]
    public required IReadOnlyList<ContentTypeSnippetModel> Snippets { get; init; }

    /// <summary>All taxonomy groups in the environment, ordered by codename.</summary>
    [JsonPropertyName("taxonomies")]
    public required IReadOnlyList<TaxonomyGroupModel> Taxonomies { get; init; }

    /// <summary>
    /// Serializes the snapshot to its canonical, byte-deterministic JSON form — indented, codename-ordered as
    /// exported, with <c>\n</c> line endings on every platform.
    /// </summary>
    public string ToJson() =>
        // Pin \n regardless of platform: raw CR/LF inside JSON strings is always escaped by the writer, so the
        // only \r\n sequences come from indentation and are safe to normalize.
        JsonSerializer.Serialize(this, SerializerOptions).Replace("\r\n", "\n");

    /// <summary>
    /// Deserializes a snapshot previously produced by <see cref="ToJson"/>.
    /// </summary>
    /// <param name="json">The snapshot JSON.</param>
    /// <exception cref="JsonException">The JSON is not a content model snapshot, or its format version is newer than
    /// this SDK supports.</exception>
    public static ContentModelSnapshot FromJson(string json)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(json);

        var snapshot = JsonSerializer.Deserialize<ContentModelSnapshot>(json, SerializerOptions)
            ?? throw new JsonException("The JSON does not contain a content model snapshot.");

        if (snapshot.Version is < 1 or > CurrentVersion)
        {
            throw new JsonException(
                $"Unsupported content model snapshot format version {snapshot.Version}; this SDK reads up to version {CurrentVersion}.");
        }

        return snapshot;
    }

    private static JsonSerializerOptions CreateSerializerOptions()
    {
        var options = RefitSettingsProvider.CreateDefaultJsonSerializerOptions();
        options.WriteIndented = true;
        return options;
    }
}

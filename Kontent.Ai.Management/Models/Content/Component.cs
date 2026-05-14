using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.Content;

/// <summary>
/// Inline component within a <see cref="RichTextElement"/>. Phase 3 ships this type with the component identifier
/// only; the payload shape — whether to carry the embedded <see cref="IContentItem"/> directly, or a content-type
/// codename plus an element-values dictionary, or both — is finalized in phase 4 alongside the STJ envelope
/// converter (see <c>phases-3-4-5.md</c>, open coordination item §1).
/// </summary>
public sealed record Component
{
    /// <summary>Component identifier within the rich-text payload (always GUID; not null).</summary>
    [JsonPropertyName("id")]
    public required Guid Id { get; init; }
}

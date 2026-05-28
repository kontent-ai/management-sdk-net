namespace Kontent.Ai.Management.Models.TypeSnippets.Patch;

/// <summary>
/// Base shape for a content type snippet PATCH operation. Concrete subtypes specialize the verb (<c>addInto</c>, <c>replace</c>, <c>move</c>, <c>remove</c>).
/// </summary>
public abstract record ContentTypeSnippetOperationBaseModel
{
    /// <summary>
    /// Operation verb. Pinned by each concrete subtype.
    /// </summary>
    [JsonPropertyName("op")]
    public abstract string Op { get; }

    /// <summary>
    /// JSON-Pointer-style path to the target, with identifier selectors. Examples: <c>/elements</c>, <c>/elements/codename:my_element</c>, <c>/elements/id:abc-...</c>.
    /// </summary>
    [JsonPropertyName("path")]
    public required string Path { get; init; }
}

namespace Kontent.Ai.Management.Models.ContentModel.Patch;

/// <summary>
/// Base shape for a content-type or content-type-snippet PATCH operation. Concrete subtypes specialize the verb
/// (<c>addInto</c>, <c>replace</c>, <c>move</c>, <c>remove</c>). Build instances via
/// <c>ContentTypePatch</c> / <c>ContentTypeSnippetPatch</c> rather than constructing them directly.
/// </summary>
public abstract record ContentModelOperationBaseModel
{
    /// <summary>
    /// Operation verb. Pinned by each concrete subtype.
    /// </summary>
    [JsonPropertyName("op")]
    public abstract string Op { get; }

    /// <summary>
    /// JSON-Pointer-style path to the target, with identifier selectors. Examples: <c>/elements</c>, <c>/elements/codename:my_element</c>, <c>/elements/id:abc-...</c>, <c>/elements/external_id:my-ext-id</c>.
    /// </summary>
    [JsonPropertyName("path")]
    public required string Path { get; init; }
}

namespace Kontent.Ai.Management.Models.Types.Patch;

/// <summary>
/// Base shape for a content type PATCH operation. Concrete subtypes specialize the verb (<c>addInto</c>, <c>replace</c>, <c>move</c>, <c>remove</c>).
/// </summary>
public abstract record ContentTypeOperationBaseModel
{
    /// <summary>
    /// Operation verb. Pinned by each concrete subtype.
    /// </summary>
    [JsonPropertyName("op")]
    public abstract string Op { get; }

    /// <summary>
    /// JSON-Pointer-style path to the target, with identifier selectors. Examples: <c>/elements</c>, <c>/elements/codename:my_text_element</c>, <c>/elements/id:abc-...</c>, <c>/elements/external_id:my-ext-id</c>.
    /// </summary>
    [JsonPropertyName("path")]
    public required string Path { get; init; }
}

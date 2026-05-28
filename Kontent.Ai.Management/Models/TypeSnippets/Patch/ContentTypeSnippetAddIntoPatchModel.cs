namespace Kontent.Ai.Management.Models.TypeSnippets.Patch;

/// <summary>
/// <c>addInto</c> operation. Inserts a new object into the collection at <see cref="ContentTypeSnippetOperationBaseModel.Path"/>.
/// </summary>
public sealed record ContentTypeSnippetAddIntoPatchModel : ContentTypeSnippetOperationBaseModel
{
    /// <inheritdoc/>
    public override string Op => "addInto";

    /// <summary>
    /// Object to insert. For <c>/elements</c>, this is an element metadata model (excluding <c>url_slug</c>, <c>subpages</c>, and <c>content_type_snippet</c>, which are not allowed in snippets).
    /// </summary>
    [JsonPropertyName("value")]
    public required object Value { get; init; }

    /// <summary>
    /// Position the new object before this existing object. Mutually exclusive with <see cref="After"/>. When both are null the new object is appended at the end.
    /// </summary>
    [JsonPropertyName("before")]
    public Reference? Before { get; init; }

    /// <summary>
    /// Position the new object after this existing object. Mutually exclusive with <see cref="Before"/>. When both are null the new object is appended at the end.
    /// </summary>
    [JsonPropertyName("after")]
    public Reference? After { get; init; }
}

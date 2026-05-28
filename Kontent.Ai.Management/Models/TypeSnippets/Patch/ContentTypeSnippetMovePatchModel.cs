namespace Kontent.Ai.Management.Models.TypeSnippets.Patch;

/// <summary>
/// <c>move</c> operation. Moves the object at <see cref="ContentTypeSnippetOperationBaseModel.Path"/> to a new position. The API requires exactly one of <see cref="Before"/> or <see cref="After"/>; sending neither (or both) returns 400.
/// </summary>
public sealed record ContentTypeSnippetMovePatchModel : ContentTypeSnippetOperationBaseModel
{
    /// <inheritdoc/>
    public override string Op => "move";

    /// <summary>
    /// Position the moved object before this existing object. Mutually exclusive with <see cref="After"/>.
    /// </summary>
    [JsonPropertyName("before")]
    public Reference? Before { get; init; }

    /// <summary>
    /// Position the moved object after this existing object. Mutually exclusive with <see cref="Before"/>.
    /// </summary>
    [JsonPropertyName("after")]
    public Reference? After { get; init; }
}

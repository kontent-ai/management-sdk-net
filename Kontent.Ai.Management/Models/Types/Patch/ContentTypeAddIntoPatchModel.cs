namespace Kontent.Ai.Management.Models.Types.Patch;

/// <summary>
/// <c>addInto</c> operation. Inserts a new object into the collection at <see cref="ContentTypeOperationBaseModel.Path"/>.
/// </summary>
public sealed record ContentTypeAddIntoPatchModel : ContentTypeOperationBaseModel
{
    /// <inheritdoc/>
    public override string Op => "addInto";

    /// <summary>
    /// Object to insert. Shape depends on the target collection (e.g., an element metadata model under <c>/elements</c>).
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

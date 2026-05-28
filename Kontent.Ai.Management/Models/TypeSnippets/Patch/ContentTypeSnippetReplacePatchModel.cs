namespace Kontent.Ai.Management.Models.TypeSnippets.Patch;

/// <summary>
/// <c>replace</c> operation. Replaces the value at <see cref="ContentTypeSnippetOperationBaseModel.Path"/>. A null <see cref="Value"/> clears the targeted property.
/// </summary>
public sealed record ContentTypeSnippetReplacePatchModel : ContentTypeSnippetOperationBaseModel
{
    /// <inheritdoc/>
    public override string Op => "replace";

    /// <summary>
    /// New value at the targeted path. Set to <c>null</c> to clear the property.
    /// </summary>
    [JsonPropertyName("value")]
    public required object? Value { get; init; }
}

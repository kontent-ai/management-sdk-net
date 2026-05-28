namespace Kontent.Ai.Management.Models.Types.Patch;

/// <summary>
/// <c>replace</c> operation. Replaces the value at <see cref="ContentTypeOperationBaseModel.Path"/>. A null <see cref="Value"/> clears the targeted property.
/// </summary>
public sealed record ContentTypeReplacePatchModel : ContentTypeOperationBaseModel
{
    /// <inheritdoc/>
    public override string Op => "replace";

    /// <summary>
    /// New value at the targeted path. Set to <c>null</c> to clear the property (e.g., remove a previously-configured default value or guidelines).
    /// </summary>
    [JsonPropertyName("value")]
    public required object? Value { get; init; }
}

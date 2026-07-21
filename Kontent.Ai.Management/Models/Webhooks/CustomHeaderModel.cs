namespace Kontent.Ai.Management.Models.Webhooks;

/// <summary>
/// A custom HTTP header sent alongside the webhook notification's standard headers.
/// </summary>
public sealed record CustomHeaderModel
{
    /// <summary>
    /// Header name.
    /// </summary>
    [JsonPropertyName("key")]
    public required string Key { get; init; }

    /// <summary>
    /// Header value.
    /// </summary>
    [JsonPropertyName("value")]
    public required string Value { get; init; }
}

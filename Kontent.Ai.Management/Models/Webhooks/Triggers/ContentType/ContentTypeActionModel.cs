namespace Kontent.Ai.Management.Models.Webhooks.Triggers.ContentType;

/// <summary>
/// A content type action that fires the webhook.
/// </summary>
public sealed record ContentTypeActionModel
{
    /// <summary>
    /// The action performed on the content type.
    /// </summary>
    [JsonPropertyName("action")]
    public required ContentTypeAction Action { get; init; }
}

namespace Kontent.Ai.Management.Models.Workflow;

/// <summary>
/// A scope binding for a workflow (response shape). Always emits both <see cref="Collections"/> and <see cref="ContentTypes"/> as arrays — the server normalizes any missing side to an empty array.
/// </summary>
public sealed record WorkflowScopeModel
{
    /// <summary>
    /// Collections this scope applies to. May be empty.
    /// </summary>
    [JsonPropertyName("collections")]
    public required IReadOnlyList<Reference> Collections { get; init; }

    /// <summary>
    /// Content types this scope applies to. May be empty.
    /// </summary>
    [JsonPropertyName("content_types")]
    public required IReadOnlyList<Reference> ContentTypes { get; init; }
}

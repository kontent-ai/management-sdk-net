namespace Kontent.Ai.Management.Models.Workflow;

/// <summary>
/// A scope binding for a workflow (upsert shape). Both arrays are field-optional — if omitted, the server fills the missing side with <c>[]</c> and echoes both on the response.
/// </summary>
public sealed record WorkflowScopeUpsertModel
{
    /// <summary>
    /// Collections this scope applies to. Optional — omit to default to none.
    /// </summary>
    [JsonPropertyName("collections")]
    public IReadOnlyList<Reference>? Collections { get; init; }

    /// <summary>
    /// Content types this scope applies to. Optional — omit to default to none.
    /// </summary>
    [JsonPropertyName("content_types")]
    public IReadOnlyList<Reference>? ContentTypes { get; init; }
}

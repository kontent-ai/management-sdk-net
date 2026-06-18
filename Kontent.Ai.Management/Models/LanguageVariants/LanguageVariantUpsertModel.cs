using Kontent.Ai.Management.Models.LanguageVariants.Elements;
using Kontent.Ai.Management.Models.Workflow;

namespace Kontent.Ai.Management.Models.LanguageVariants;

/// <summary>
/// Payload for creating or replacing a language variant.
/// </summary>
public sealed record LanguageVariantUpsertModel
{
    /// <summary>
    /// Element values to set. Use a typed <see cref="BaseElement"/> subtype per element kind, or
    /// <see cref="DynamicElement"/> for kinds the SDK does not model.
    /// </summary>
    [JsonPropertyName("elements")]
    public required IEnumerable<BaseElement> Elements { get; init; }

    /// <summary>
    /// Workflow and step to move the variant into. Optional — omit to leave the workflow unchanged.
    /// </summary>
    [JsonPropertyName("workflow")]
    public WorkflowStepIdentifier? Workflow { get; init; }

    /// <summary>
    /// Due date to set. Optional.
    /// </summary>
    [JsonPropertyName("due_date")]
    public DueDateModel? DueDate { get; init; }

    /// <summary>
    /// Free-form note to set. Optional.
    /// </summary>
    [JsonPropertyName("note")]
    public string? Note { get; init; }

    /// <summary>
    /// Contributors to assign. Optional.
    /// </summary>
    [JsonPropertyName("contributors")]
    public IEnumerable<UserIdentifier>? Contributors { get; init; }
}

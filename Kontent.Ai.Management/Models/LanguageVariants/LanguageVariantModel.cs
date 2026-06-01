using Kontent.Ai.Management.Models.Publishing;
using Kontent.Ai.Management.Models.Workflow;

namespace Kontent.Ai.Management.Models.LanguageVariants;

/// <summary>
/// A language variant of a content item (response shape).
/// </summary>
public sealed record LanguageVariantModel
{
    /// <summary>
    /// Reference to the content item this variant belongs to.
    /// </summary>
    [JsonPropertyName("item")]
    public required Reference Item { get; init; }

    /// <summary>
    /// Element values. Each entry is a polymorphic <c>{ element, value }</c> shape whose value type depends on the element kind.
    /// </summary>
    [JsonPropertyName("elements")]
    public required IEnumerable<object> Elements { get; init; }

    /// <summary>
    /// Reference to the language of this variant.
    /// </summary>
    [JsonPropertyName("language")]
    public required Reference Language { get; init; }

    /// <summary>
    /// ISO-8601 timestamp of the last change to the variant.
    /// </summary>
    [JsonPropertyName("last_modified")]
    public required DateTime LastModified { get; init; }

    /// <summary>
    /// Publishing and unpublishing schedule. The wrapper is always present; the individual timestamps may be null.
    /// </summary>
    [JsonPropertyName("schedule")]
    public required ScheduleResponseModel Schedule { get; init; }

    /// <summary>
    /// Workflow and step the variant currently sits in.
    /// </summary>
    [JsonPropertyName("workflow")]
    public required WorkflowStepIdentifier Workflow { get; init; }

    /// <summary>
    /// Due date. The wrapper is always present; its value may be null when no due date is set.
    /// </summary>
    [JsonPropertyName("due_date")]
    public required DueDateModel DueDate { get; init; }

    /// <summary>
    /// Free-form note attached to the variant.
    /// </summary>
    [JsonPropertyName("note")]
    public string? Note { get; init; }

    /// <summary>
    /// Users assigned as contributors. May be empty.
    /// </summary>
    [JsonPropertyName("contributors")]
    public required IEnumerable<UserIdentifier> Contributors { get; init; }
}

namespace Kontent.Ai.Management.Models.Webhooks.Triggers.ContentItem;

/// <summary>
/// A content item action that fires the webhook.
/// </summary>
public sealed record ContentItemActionModel
{
    /// <summary>
    /// The action performed on the content item.
    /// </summary>
    [JsonPropertyName("action")]
    public required ContentItemAction Action { get; init; }

    /// <summary>
    /// Workflow/step transitions that fire the webhook. Only relevant for the workflow-step-changed action.
    /// </summary>
    [JsonPropertyName("transition_to")]
    public IReadOnlyList<ContentItemWorkflowTransition>? TransitionTo { get; init; }
}

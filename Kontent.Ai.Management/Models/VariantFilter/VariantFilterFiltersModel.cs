namespace Kontent.Ai.Management.Models.VariantFilter;

/// <summary>
/// Represents the variant filter filters model.
/// </summary>
public sealed record VariantFilterFiltersModel
{
    /// <summary>
    /// Gets the search phrase.
    /// </summary>
    [JsonPropertyName("search_phrase")]
    public string SearchPhrase { get; init; }

    /// <summary>
    /// Gets the language.
    /// </summary>
    [JsonPropertyName("language")]
    public Reference Language { get; init; }

    /// <summary>
    /// Gets the content types.
    /// </summary>
    [JsonPropertyName("content_types")]
    public IEnumerable<Reference> ContentTypes { get; init; }

    /// <summary>
    /// Gets the contributors.
    /// </summary>
    [JsonPropertyName("contributors")]
    public IEnumerable<UserIdentifier> Contributors { get; init; }

    /// <summary>
    /// Gets whether to filter items with no contributors.
    /// </summary>
    [JsonPropertyName("has_no_contributors")]
    public bool? HasNoContributors { get; init; }

    /// <summary>
    /// Gets the completion statuses.
    /// </summary>
    [JsonPropertyName("completion_statuses")]
    public IEnumerable<VariantFilterCompletionStatus> CompletionStatuses { get; init; }

    /// <summary>
    /// Gets the workflow steps.
    /// </summary>
    [JsonPropertyName("workflow_steps")]
    public IEnumerable<VariantFilterWorkflowStepsModel> WorkflowSteps { get; init; }

    /// <summary>
    /// Gets the taxonomy groups.
    /// </summary>
    [JsonPropertyName("taxonomy_groups")]
    public IEnumerable<VariantFilterTaxonomyGroupModel> TaxonomyGroups { get; init; }

    /// <summary>
    /// Gets the spaces.
    /// </summary>
    [JsonPropertyName("spaces")]
    public IEnumerable<Reference> Spaces { get; init; }

    /// <summary>
    /// Gets the collections.
    /// </summary>
    [JsonPropertyName("collections")]
    public IEnumerable<Reference> Collections { get; init; }

    /// <summary>
    /// Gets the publishing states.
    /// </summary>
    [JsonPropertyName("publishing_states")]
    public IEnumerable<VariantFilterPublishingState> PublishingStates { get; init; }

    /// <summary>
    /// Gets the component types.
    /// </summary>
    [JsonPropertyName("component_types")]
    public IEnumerable<Reference> ComponentTypes { get; init; }
}
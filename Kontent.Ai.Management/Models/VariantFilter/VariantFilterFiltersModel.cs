namespace Kontent.Ai.Management.Models.VariantFilter;

/// <summary>
/// Filter criteria for the items-with-variants filter endpoint. Any combination of fields can be supplied; omit a field to skip that filter.
/// </summary>
public sealed record VariantFilterFiltersModel
{
    /// <summary>
    /// Free-text search phrase matched against item content.
    /// </summary>
    [JsonPropertyName("search_phrase")]
    public string? SearchPhrase { get; init; }

    /// <summary>
    /// Restrict results to a specific language.
    /// </summary>
    [JsonPropertyName("language")]
    public Reference? Language { get; init; }

    /// <summary>
    /// Restrict results to items of these content types.
    /// </summary>
    [JsonPropertyName("content_types")]
    public IEnumerable<Reference>? ContentTypes { get; init; }

    /// <summary>
    /// Restrict results to variants with these contributors.
    /// </summary>
    [JsonPropertyName("contributors")]
    public IEnumerable<UserIdentifier>? Contributors { get; init; }

    /// <summary>
    /// When true, restricts to variants without any assigned contributor.
    /// </summary>
    [JsonPropertyName("has_no_contributors")]
    public bool? HasNoContributors { get; init; }

    /// <summary>
    /// Restrict to variants with these completion statuses.
    /// </summary>
    [JsonPropertyName("completion_statuses")]
    public IEnumerable<VariantFilterCompletionStatus>? CompletionStatuses { get; init; }

    /// <summary>
    /// Restrict to variants currently in these workflow steps.
    /// </summary>
    [JsonPropertyName("workflow_steps")]
    public IEnumerable<VariantFilterWorkflowStepsModel>? WorkflowSteps { get; init; }

    /// <summary>
    /// Restrict to variants tagged with these taxonomy terms.
    /// </summary>
    [JsonPropertyName("taxonomy_groups")]
    public IEnumerable<VariantFilterTaxonomyGroupModel>? TaxonomyGroups { get; init; }

    /// <summary>
    /// Restrict to items assigned to these spaces.
    /// </summary>
    [JsonPropertyName("spaces")]
    public IEnumerable<Reference>? Spaces { get; init; }

    /// <summary>
    /// Restrict to items in these collections.
    /// </summary>
    [JsonPropertyName("collections")]
    public IEnumerable<Reference>? Collections { get; init; }

    /// <summary>
    /// Restrict to variants in these publishing states.
    /// </summary>
    [JsonPropertyName("publishing_states")]
    public IEnumerable<VariantFilterPublishingState>? PublishingStates { get; init; }

    /// <summary>
    /// Restrict to variants containing components of these content types.
    /// </summary>
    [JsonPropertyName("component_types")]
    public IEnumerable<Reference>? ComponentTypes { get; init; }
}
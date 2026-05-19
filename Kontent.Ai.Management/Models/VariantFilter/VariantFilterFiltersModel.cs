using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.VariantFilter;

/// <summary>
/// Represents the variant filter filters model.
/// </summary>
public sealed record VariantFilterFiltersModel
{
    /// <summary>
    /// Gets the search phrase.
    /// </summary>
    [JsonProperty("search_phrase")]
    public string SearchPhrase { get; init; }

    /// <summary>
    /// Gets the language.
    /// </summary>
    [JsonProperty("language")]
    public Reference Language { get; init; }

    /// <summary>
    /// Gets the content types.
    /// </summary>
    [JsonProperty("content_types")]
    public IEnumerable<Reference> ContentTypes { get; init; }

    /// <summary>
    /// Gets the contributors.
    /// </summary>
    [JsonProperty("contributors")]
    public IEnumerable<UserIdentifier> Contributors { get; init; }

    /// <summary>
    /// Gets whether to filter items with no contributors.
    /// </summary>
    [JsonProperty("has_no_contributors")]
    public bool? HasNoContributors { get; init; }

    /// <summary>
    /// Gets the completion statuses.
    /// </summary>
    [JsonProperty("completion_statuses")]
    public IEnumerable<VariantFilterCompletionStatus> CompletionStatuses { get; init; }

    /// <summary>
    /// Gets the workflow steps.
    /// </summary>
    [JsonProperty("workflow_steps")]
    public IEnumerable<VariantFilterWorkflowStepsModel> WorkflowSteps { get; init; }

    /// <summary>
    /// Gets the taxonomy groups.
    /// </summary>
    [JsonProperty("taxonomy_groups")]
    public IEnumerable<VariantFilterTaxonomyGroupModel> TaxonomyGroups { get; init; }

    /// <summary>
    /// Gets the spaces.
    /// </summary>
    [JsonProperty("spaces")]
    public IEnumerable<Reference> Spaces { get; init; }

    /// <summary>
    /// Gets the collections.
    /// </summary>
    [JsonProperty("collections")]
    public IEnumerable<Reference> Collections { get; init; }

    /// <summary>
    /// Gets the publishing states.
    /// </summary>
    [JsonProperty("publishing_states")]
    public IEnumerable<VariantFilterPublishingState> PublishingStates { get; init; }

    /// <summary>
    /// Gets the component types.
    /// </summary>
    [JsonProperty("component_types")]
    public IEnumerable<Reference> ComponentTypes { get; init; }
}
using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.VariantFilter;

/// <summary>
/// Represents the variant filter filters model.
/// </summary>
public class VariantFilterFiltersModel
{
    /// <summary>
    /// Gets or sets the search phrase.
    /// </summary>
    [JsonProperty("search_phrase")]
    public string SearchPhrase { get; set; }

    /// <summary>
    /// Gets or sets the language.
    /// </summary>
    [JsonProperty("language")]
    public Reference Language { get; set; }

    /// <summary>
    /// Gets or sets the content types.
    /// </summary>
    [JsonProperty("content_types")]
    public IEnumerable<Reference> ContentTypes { get; set; }

    /// <summary>
    /// Gets or sets the contributors.
    /// </summary>
    [JsonProperty("contributors")]
    public IEnumerable<UserIdentifier> Contributors { get; set; }

    /// <summary>
    /// Gets or sets whether to filter items with no contributors.
    /// </summary>
    [JsonProperty("has_no_contributors")]
    public bool? HasNoContributors { get; set; }

    /// <summary>
    /// Gets or sets the completion statuses.
    /// </summary>
    [JsonProperty("completion_statuses")]
    public IEnumerable<VariantFilterCompletionStatus> CompletionStatuses { get; set; }

    /// <summary>
    /// Gets or sets the workflow steps.
    /// </summary>
    [JsonProperty("workflow_steps")]
    public IEnumerable<VariantFilterWorkflowStepsModel> WorkflowSteps { get; set; }

    /// <summary>
    /// Gets or sets the taxonomy groups.
    /// </summary>
    [JsonProperty("taxonomy_groups")]
    public IEnumerable<VariantFilterTaxonomyGroupModel> TaxonomyGroups { get; set; }

    /// <summary>
    /// Gets or sets the spaces.
    /// </summary>
    [JsonProperty("spaces")]
    public IEnumerable<Reference> Spaces { get; set; }

    /// <summary>
    /// Gets or sets the collections.
    /// </summary>
    [JsonProperty("collections")]
    public IEnumerable<Reference> Collections { get; set; }

    /// <summary>
    /// Gets or sets the publishing states.
    /// </summary>
    [JsonProperty("publishing_states")]
    public IEnumerable<VariantFilterPublishingState> PublishingStates { get; set; }
}
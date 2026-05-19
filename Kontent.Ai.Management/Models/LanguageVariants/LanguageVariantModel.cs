using Kontent.Ai.Management.Models.Publishing;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.Workflow;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.LanguageVariants;

/// <summary>
/// Represents language variant model.
/// </summary>
public sealed record LanguageVariantModel
{
    /// <summary>
    /// Gets item of the variant.
    /// </summary>
    [JsonPropertyName("item")]
    public Reference Item { get; init; }

    /// <summary>
    /// Gets elements of the variant.
    /// </summary>
    [JsonPropertyName("elements")]
    public IEnumerable<object> Elements { get; init; }

    /// <summary>
    /// Gets the language of the variant.
    /// </summary>
    [JsonPropertyName("language")]
    public Reference Language { get; init; }

    /// <summary>
    /// Gets the last modified timestamp of the language variant.
    /// </summary>
    [JsonPropertyName("last_modified")]
    public DateTime? LastModified { get; init; }

    /// <summary>
    /// Gets the publishing and unpublishing schedule of the language variant.
    /// </summary>
    [JsonPropertyName("schedule")]
    public ScheduleResponseModel Schedule { get; init; }

    /// <summary>
    /// Gets workflow step identifier.
    /// </summary>
    [JsonPropertyName("workflow")]
    public WorkflowStepIdentifier Workflow { get; init; }

    /// <summary>
    /// Gets due date.
    /// </summary>
    [JsonPropertyName("due_date")]
    public DueDateModel DueDate { get; init; }

    /// <summary>
    /// Gets a note.
    /// </summary>
    [JsonPropertyName("note")]
    public string Note { get; init; }

    /// <summary>
    /// Gets the contributors.
    /// </summary>
    [JsonPropertyName("contributors")]
    public IEnumerable<UserIdentifier> Contributors { get; init; }
}

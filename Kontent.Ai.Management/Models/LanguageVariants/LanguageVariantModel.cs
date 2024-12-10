using Kontent.Ai.Management.Models.Publishing;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.Workflow;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.LanguageVariants;

/// <summary>
/// Represents language variant model.
/// </summary>
public sealed class LanguageVariantModel
{
    /// <summary>
    /// Gets or sets item of the variant.
    /// </summary>
    [JsonProperty("item")]
    public Reference Item { get; set; }

    /// <summary>
    /// Gets or sets elements of the variant.
    /// </summary>
    [JsonProperty("elements")]
    public IEnumerable<dynamic> Elements { get; set; }

    /// <summary>
    /// Gets or sets the language of the variant.
    /// </summary>
    [JsonProperty("language")]
    public Reference Language { get; set; }

    /// <summary>
    /// Gets or sets the last modified timestamp of the language variant.
    /// </summary>
    [JsonProperty("last_modified")]
    public DateTime? LastModified { get; set; }

    /// <summary>
    /// Gets or sets the publishing and unpublishing schedule of the language variant.
    /// </summary>
    [JsonProperty("schedule")]
    public ScheduleResponseModel Schedule { get; set; }

    /// <summary>
    /// Gets or sets workflow step identifier.
    /// </summary>
    [JsonProperty("workflow")]
    public WorkflowStepIdentifier Workflow { get; set; }

    /// <summary>
    /// Gets or sets due date.
    /// </summary>
    [JsonProperty("due_date")]
    public DueDateModel DueDate { get; set; }

    /// <summary>
    /// Gets or sets a note.
    /// </summary>
    [JsonProperty("note")]
    public string Note { get; set; }

    /// <summary>
    /// Gets or sets the contributors.
    /// </summary>
    [JsonProperty("contributors")]
    public IEnumerable<UserIdentifier> Contributors { get; set; }
}

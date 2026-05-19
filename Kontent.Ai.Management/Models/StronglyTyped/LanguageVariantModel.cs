using Kontent.Ai.Management.Models.Publishing;
﻿using Kontent.Ai.Management.Models.LanguageVariants;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.Workflow;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.StronglyTyped;

/// <summary>
/// Represents strongly typed language variant model.
/// </summary>
public sealed record LanguageVariantModel<T> where T : new()
{
    /// <summary>
    /// Gets item of the variant.
    /// </summary>
    [JsonProperty("item")]
    public Reference Item { get; init; }

    /// <summary>
    /// Gets elements of the language variant.
    /// </summary>
    [JsonProperty("elements")]
    public T Elements { get; init; }

    /// <summary>
    /// Gets the language of the variant.
    /// </summary>
    [JsonProperty("language")]
    public Reference Language { get; init; }

    /// <summary>
    /// Gets the last modified timestamp of the language variants.
    /// </summary>
    [JsonProperty("last_modified")]
    public DateTime? LastModified { get; init; }

    /// <summary>
    /// Gets the publishing and unpublishing schedule of the language variant.
    /// </summary>
    [JsonProperty("schedule")]
    public ScheduleResponseModel Schedule { get; init; }

    /// <summary>
    /// Gets workflow step identifier.
    /// </summary>
    [JsonProperty("workflow")]
    public WorkflowStepIdentifier Workflow { get; init; }

    /// <summary>
    /// Gets due date.
    /// </summary>
    [JsonProperty("due_date")]
    public DueDateModel DueDate { get; init; }

    /// <summary>
    /// Gets a note.
    /// </summary>
    [JsonProperty("note")]
    public string Note { get; init; }

    /// <summary>
    /// Gets the contributors.
    /// </summary>
    [JsonProperty("contributors")]
    public IEnumerable<UserIdentifier> Contributors { get; init; }
}

using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.Workflow;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.LanguageVariants;

/// <summary>
/// Represents language variant upsert model.
/// </summary>
public sealed record LanguageVariantUpsertModel
{
    /// <summary>
    /// Gets elements of the variant.
    /// </summary>
    [JsonProperty("elements", Required = Required.Always)]
    public IEnumerable<dynamic> Elements { get; init; }

    /// <summary>
    /// Gets workflow step identifier to update.
    /// </summary>
    [JsonProperty("workflow")]
    public WorkflowStepIdentifier Workflow { get; init; }

    /// <summary>
    /// Gets due date to update.
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

    /// <summary>
    /// Creates an instance of the language variant upsert model.
    /// </summary>
    public LanguageVariantUpsertModel()
    {
    }

    internal LanguageVariantUpsertModel(LanguageVariantModel languageVariant)
    {
        Elements = languageVariant.Elements;
        Workflow = languageVariant.Workflow;
        DueDate = languageVariant.DueDate;
        Note = languageVariant.Note;
        Contributors = languageVariant.Contributors;
    }
}

using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.Workflow;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.LanguageVariants;

/// <summary>
/// Represents language variant upsert model.
/// </summary>
public sealed class LanguageVariantUpsertModel
{
    /// <summary>
    /// Gets or sets elements of the variant.
    /// </summary>
    [JsonProperty("elements", Required = Required.Always)]
    public IEnumerable<dynamic> Elements { get; set; }

    /// <summary>
    /// Gets or sets workflow step identifier to update.
    /// </summary>
    [JsonProperty("workflow")]
    public WorkflowStepIdentifier Workflow { get; set; }

    /// <summary>
    /// Gets or sets due date to update.
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

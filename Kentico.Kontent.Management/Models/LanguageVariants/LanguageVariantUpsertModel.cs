using System.Collections.Generic;
using Kentico.Kontent.Management.Models.Workflow;
using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.LanguageVariants;

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
    public WorkflowStepIdentifier Workflow { get; set; }

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
    }
}

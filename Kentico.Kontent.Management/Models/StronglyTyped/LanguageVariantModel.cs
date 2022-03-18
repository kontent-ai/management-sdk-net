using System;
using Kentico.Kontent.Management.Models.Shared;
using Kentico.Kontent.Management.Models.Workflow;
using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.StronglyTyped;

/// <summary>
/// Represents strongly typed language variant model.
/// </summary>
public sealed class LanguageVariantModel<T> where T : new()
{
    /// <summary>
    /// Gets or sets item of the variant.
    /// </summary>
    [JsonProperty("item")]
    public Reference Item { get; set; }

    /// <summary>
    /// Gets or sets elements of the language variant.
    /// </summary>
    [JsonProperty("elements")]
    public T Elements { get; set; }

    /// <summary>
    /// Gets or sets the language of the variant.
    /// </summary>
    [JsonProperty("language")]
    public Reference Language { get; set; }

    /// <summary>
    /// Gets or sets the last modified timestamp of the language variants.
    /// </summary>
    [JsonProperty("last_modified")]
    public DateTime? LastModified { get; set; }
        
    /// <summary>
    /// Gets or sets workflow step identifier.
    /// </summary>
    [JsonProperty("workflow")]
    public WorkflowStepIdentifier Workflow { get; set; }
}

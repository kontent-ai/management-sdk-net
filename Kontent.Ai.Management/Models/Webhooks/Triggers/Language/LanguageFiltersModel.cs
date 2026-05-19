using Kontent.Ai.Management.Models.Shared;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.Webhooks.Triggers.Language;

/// <summary>
/// Represents the language filters model.
/// </summary>
public class LanguageFiltersModel
{
    /// <summary>
    /// References to languages
    /// </summary>
    [JsonPropertyName("languages")]
    public IEnumerable<Reference> Languages { get; set; }
}
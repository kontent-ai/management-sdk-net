using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.Webhooks.Triggers.Language;

/// <summary>
/// Represents the language filters model.
/// </summary>
public class LanguageFiltersModel
{
    /// <summary>
    /// References to languages
    /// </summary>
    [JsonProperty("languages")]
    public IEnumerable<Reference> Languages { get; set; }
}
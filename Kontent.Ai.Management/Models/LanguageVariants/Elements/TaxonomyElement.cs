using System.Collections.Generic;
using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.LanguageVariants.Elements;

/// <summary>
/// Represents the strongly typed assets element.
/// </summary>
public class TaxonomyElement : BaseElement
{
    /// <summary>
    /// Gets or sets the value of the asset element.
    /// </summary>
    [JsonProperty("value")]
    public IEnumerable<Reference> Value { get; set; }

    /// <summary>
    /// Coverts the taxonomy element to the dynamic object.
    /// </summary>
    public override dynamic ToDynamic() => new {
        element = Element.ToDynamic(),
        value = Value,
    };
}

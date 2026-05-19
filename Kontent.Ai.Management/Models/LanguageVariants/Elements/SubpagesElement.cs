using Kontent.Ai.Management.Models.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.LanguageVariants.Elements;

/// <summary>
/// Represents the subpages element.
/// </summary>
public class SubpagesElement : BaseElement
{
    /// <summary>
    /// Gets or sets the value of the subpages element.
    /// </summary>
    [JsonPropertyName("value")]
    public IEnumerable<Reference> Value { get; set; }

    /// <summary>
    /// Coverts the subpages element to the dynamic object.
    /// </summary>
    public override dynamic ToDynamic() => new {
        element = Element.ToDynamic(),
        value = Value?.Select(v => v.ToDynamic()),
    };
}

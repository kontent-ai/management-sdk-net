using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.LanguageVariants.Elements;

/// <summary>
/// Represents the strongly typed linked items element.
/// </summary>
public class LinkedItemsElement : BaseElement
{
    /// <summary>
    /// Gets or sets the value of linked items element.
    /// </summary>
    [JsonProperty("value")]
    public IEnumerable<Reference> Value { get; set; }

    /// <summary>
    /// Coverts the linked items element to the dynamic object.
    /// </summary>
    public override dynamic ToDynamic() => new {
        element = Element.ToDynamic(),
        value = Value,
    };
}

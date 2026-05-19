using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.LanguageVariants.Elements;

/// <summary>
/// Represents the strongly typed rich text element.
/// </summary>
public class RichTextElement : BaseElement
{
    /// <summary>
    /// Gets or sets the value of rich text element components.
    /// </summary>
    [JsonPropertyName("components")]
    public IEnumerable<ComponentModel> Components { get; set; }

    /// <summary>
    /// Gets or sets the value of the rich text element.
    /// </summary>
    [JsonPropertyName("value")]
    public string Value { get; set; }

    /// <summary>
    /// Coverts the rich text element to the dynamic object.
    /// </summary>
    public override dynamic ToDynamic() => new {
        element = Element.ToDynamic(),
        value = Value,
        components = Components
    };
}

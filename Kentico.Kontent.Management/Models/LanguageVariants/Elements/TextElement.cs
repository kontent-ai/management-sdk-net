using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.LanguageVariants.Elements;

/// <summary>
/// Represents the strongly typed text element.
/// </summary>
public class TextElement : BaseElement
{
    /// <summary>
    /// Gets or sets the value of the text element.
    /// </summary>
    [JsonProperty("value")]
    public string Value { get; set; }

    /// <summary>
    /// Coverts the text element to the dynamic object.
    /// </summary>
    public override dynamic ToDynamic() => new {
        element = Element.ToDynamic(),
        value = Value,
    };
}

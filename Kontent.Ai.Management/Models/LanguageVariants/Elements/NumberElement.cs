using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.LanguageVariants.Elements;

/// <summary>
/// Represents the strongly typed number element.
/// </summary>
public class NumberElement : BaseElement
{
    /// <summary>
    /// Gets or sets the value of the number element.
    /// </summary>
    [JsonProperty("value")]
    public decimal? Value { get; set; }

    /// <summary>
    /// Coverts the number element to the dynamic object.
    /// </summary>
    public override dynamic ToDynamic() => new {
        element = Element.ToDynamic(),
        value = Value,
    };
}

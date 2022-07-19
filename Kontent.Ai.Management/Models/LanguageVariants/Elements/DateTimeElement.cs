using System;
using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.LanguageVariants.Elements;

/// <summary>
/// Represents the strongly typed date and time element.
/// </summary>
public class DateTimeElement : BaseElement
{
    /// <summary>
    /// Gets or sets the value of the datetime element.
    /// </summary>
    [JsonProperty("value")]
    public DateTime Value { get; set; }

    /// <summary>
    /// Coverts the datetime element to the dynamic object.
    /// </summary>
    public override dynamic ToDynamic() => new {
        element = Element.ToDynamic(),
        value = Value,
    };
}

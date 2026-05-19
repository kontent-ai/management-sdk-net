using System;
using System.Text.Json.Serialization;

namespace Kontent.Ai.Management.Models.LanguageVariants.Elements;

/// <summary>
/// Represents the strongly typed date and time element.
/// </summary>
public class DateTimeElement : BaseElement
{
    /// <summary>
    /// Gets or sets the value of the datetime element.
    /// </summary>
    [JsonPropertyName("value")]
    public DateTime Value { get; set; }

    /// <summary>
    /// IANA time zone name used to display time offset of datetime element in the UI.
    /// </summary>
    [JsonPropertyName("display_timezone")]
    public string DisplayTimeZone { get; set; }

    /// <summary>
    /// Coverts the datetime element to the dynamic object.
    /// </summary>
    public override dynamic ToDynamic() => new {
        element = Element.ToDynamic(),
        value = Value,
        display_timezone = DisplayTimeZone
    };
}

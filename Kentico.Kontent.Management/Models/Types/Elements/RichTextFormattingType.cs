using System.Runtime.Serialization;

namespace Kentico.Kontent.Management.Models.Types.Elements;

/// <summary>
/// Specifies which text formatting is allowed inside tables in your rich text element. To allow all formatting, leave the array empty.
/// </summary>
public enum RichTextFormattingType
{
    /// <summary>
    /// Blod
    /// </summary>
    [EnumMember(Value = "bold")]
    Bold,

    /// <summary>
    /// Code
    /// </summary>
    [EnumMember(Value = "code")]
    Code,

    /// <summary>
    /// Italic
    /// </summary>
    [EnumMember(Value = "italic")]
    Italic,

    /// <summary>
    /// Link
    /// </summary>
    [EnumMember(Value = "link")]
    Link,

    /// <summary>
    /// Subscript
    /// </summary>
    [EnumMember(Value = "subscript")]
    Subscript,

    /// <summary>
    /// Superscript
    /// </summary>
    [EnumMember(Value = "superscript")]
    Superscript,

    /// <summary>
    /// Unstyled formatting allows only plain text
    /// </summary>
    [EnumMember(Value = "unstyled")]
    Unstyled,
}

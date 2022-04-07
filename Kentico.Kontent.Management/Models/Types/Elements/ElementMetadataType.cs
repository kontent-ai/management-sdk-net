using System.Runtime.Serialization;

namespace Kentico.Kontent.Management.Models.Types.Elements;

/// <summary>
/// Enum of all possible element types in content types.
/// </summary>
public enum ElementMetadataType
{
    /// <summary>
    /// Represents the text element.
    /// </summary>
    [EnumMember(Value = "text")]
    Text = 1,
    /// <summary>
    /// Represents the rich text element.
    /// </summary>
    [EnumMember(Value = "rich_text")]
    RichText = 2,

    /// <summary>
    /// Represents the number element.
    /// </summary>
    [EnumMember(Value = "number")]
    Number = 3,

    /// <summary>
    /// Represents the multiple-choice element.
    /// </summary>
    [EnumMember(Value = "multiple_choice")]
    MultipleChoice = 4,

    /// <summary>
    /// Represents the date and time element.
    /// </summary>
    [EnumMember(Value = "date_time")]
    DateTime = 5,

    /// <summary>
    /// Represents the asset element.
    /// </summary>
    [EnumMember(Value = "asset")]
    Asset = 6,

    /// <summary>
    /// Represents the linked items element.
    /// </summary>
    [EnumMember(Value = "modular_content")]
    LinkedItems = 7,

    /// <summary>
    /// Represents the guidelines element.
    /// </summary>
    [EnumMember(Value = "guidelines")]
    Guidelines = 8,

    /// <summary>
    /// Represents the taxonomy element.
    /// </summary>
    [EnumMember(Value = "taxonomy")]
    Taxonomy = 9,

    /// <summary>
    /// Represents the url slug element.
    /// </summary>
    [EnumMember(Value = "url_slug")]
    UrlSlug = 10,

    /// <summary>
    /// Represents the content type snippet element.
    /// </summary>
    [EnumMember(Value = "snippet")]
    ContentTypeSnippet = 11,

    /// <summary>
    /// Represents the custom element.
    /// </summary>
    [EnumMember(Value = "custom")]
    Custom = 12,

    /// <summary>
    /// Represents the subpages element.
    /// </summary>
    [EnumMember(Value = "subpages")]
    Subpages = 13
}

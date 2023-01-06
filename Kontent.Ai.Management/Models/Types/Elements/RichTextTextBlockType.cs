using System.Runtime.Serialization;

namespace Kontent.Ai.Management.Models.Types.Elements;

/// <summary>
/// Represents block types that can be used inside your rich text element.
/// </summary>
public enum RichTextTextBlockType
{
    /// <summary>
    /// OrderList
    /// </summary>
    [EnumMember(Value = "ordered-list")]
    OrderedList,

    /// <summary>
    /// UnorderedList
    /// </summary>
    [EnumMember(Value = "unordered-list")]
    UnorderedList,

    /// <summary>
    /// Paragraph
    /// </summary>
    [EnumMember(Value = "paragraph")]
    Paragraph,

    /// <summary>
    /// HeadingOne
    /// </summary>
    [EnumMember(Value = "heading-one")]
    HeadingOne,

    /// <summary>
    /// HeadingTwo
    /// </summary>
    [EnumMember(Value = "heading-two")]
    HeadingTwo,

    /// <summary>
    /// HeadingThree
    /// </summary>
    [EnumMember(Value = "heading-three")]
    HeadingThree,

    /// <summary>
    /// HeadingFour
    /// </summary>
    [EnumMember(Value = "heading-four")]
    HeadingFour,

    /// <summary>
    /// HeadingFive
    /// </summary>
    [EnumMember(Value = "heading-five")]
    HeadingFive,

    /// <summary>
    /// HeadingSix
    /// </summary>
    [EnumMember(Value = "heading-six")]
    HeadingSix
}

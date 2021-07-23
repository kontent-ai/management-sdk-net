using System.Runtime.Serialization;

namespace Kentico.Kontent.Management.Models.Types.Elements
{
    public enum RichTextTextBlockType
    {
        [EnumMember(Value = "ordered-list")]
        OrderedList,

        [EnumMember(Value = "unordered-list")]
        UnorderedList,

        [EnumMember(Value = "paragraph")]
        Paragraph,

        [EnumMember(Value = "heading-one")]
        HeadingOne,

        [EnumMember(Value = "heading-two")]
        HeadingTwo,

        [EnumMember(Value = "heading-three")]
        HeadingThree,

        [EnumMember(Value = "heading-four")]
        HeadingFour
    }
}
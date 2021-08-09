using System.Runtime.Serialization;

namespace Kentico.Kontent.Management.Models.Types.Elements
{
    public enum RichTextFormattingType
    {
        [EnumMember(Value = "bold")]
        Bold,

        [EnumMember(Value = "code")]
        Code,

        [EnumMember(Value = "italic")]
        Italic,

        [EnumMember(Value = "link")]
        Link,

        [EnumMember(Value = "subscript")]
        Subscript,

        [EnumMember(Value = "superscript")]
        Superscript,

        [EnumMember(Value = "unstyled")]
        Unstyled,
    }
}
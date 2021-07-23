using System.Runtime.Serialization;

namespace Kentico.Kontent.Management.Models.Types.Elements
{
    public enum RichTextTableBlockType
    {
        [EnumMember(Value = "text")]
        Text,
        [EnumMember(Value = "images")]
        Images,
    }
}
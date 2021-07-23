using System.Runtime.Serialization;

namespace Kentico.Kontent.Management.Models.Types.Elements
{
    public enum RichTextBlockType
    {
        [EnumMember(Value = "text")]
        Text,
        [EnumMember(Value = "tables")]
        Tables,
        [EnumMember(Value = "images")]
        Images,
        [EnumMember(Value = "components-and-items")]
        ComponentsAndItems
    }
}
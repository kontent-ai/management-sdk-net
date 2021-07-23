using System.Runtime.Serialization;

namespace Kentico.Kontent.Management.Models.Types.Elements
{
    public enum TextLengthLimitType
    {
        Undefined,
        [EnumMember(Value = "words")]
        Words,
        [EnumMember(Value = "characters")]
        Characters
    }
}
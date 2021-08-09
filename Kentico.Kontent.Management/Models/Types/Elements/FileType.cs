using System.Runtime.Serialization;

namespace Kentico.Kontent.Management.Models.Types.Elements
{
    public enum FileType
    {
        Undefined,
        [EnumMember(Value = "any")]
        Any,
        [EnumMember(Value = "adjustable")]
        Adjustable
    }
}

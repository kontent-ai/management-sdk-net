using System.Runtime.Serialization;

namespace Kentico.Kontent.Management.Models.Types
{
    public enum LimitType
    {
        Undefined,
        [EnumMember(Value = "at_least")]
        AtLeast,
        [EnumMember(Value = "exactly")]
        Exactly,
        [EnumMember(Value = "at_most")]
        AtMost
    }
}

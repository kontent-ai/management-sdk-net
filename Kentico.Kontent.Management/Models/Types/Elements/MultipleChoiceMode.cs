using System.Runtime.Serialization;

namespace Kentico.Kontent.Management.Models.Types.Elements
{
    public enum  MultipleChoiceMode
    {
        Undefined,
        [EnumMember(Value = "multiple")]
        Multiple,
        [EnumMember(Value = "single")]
        Single
    }
}

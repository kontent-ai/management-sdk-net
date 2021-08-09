using System.Runtime.Serialization;

namespace Kentico.Kontent.Management.Models.Languages
{
    public enum LanguangePropertyName
    {
        [EnumMember(Value = "codename")]
        Codename,
        [EnumMember(Value = "name")]
        Name,
        [EnumMember(Value = "fallback_language")]
        FallbackLanguage,
        [EnumMember(Value = "is_active")]
        IsActive,
    }
}

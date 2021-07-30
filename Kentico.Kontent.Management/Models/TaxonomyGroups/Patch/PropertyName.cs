using System.Runtime.Serialization;

namespace Kentico.Kontent.Management.Models.TaxonomyGroups.Patch
{
    public enum PropertyName
    {
        [EnumMember(Value = "codename")]
        Codename,
        [EnumMember(Value = "name")]
        Name,
        [EnumMember(Value = "terms")]
        Terms,
    }
}
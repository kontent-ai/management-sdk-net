using System.Runtime.Serialization;

namespace Kentico.Kontent.Management.Models.TaxonomyGroups.Patch
{
    /// <summary>
    /// Represents enum of properties that can be replaced in the taxonomy group.
    /// </summary>
    public enum PropertyName
    {
        /// <summary>
        /// The taxonomy group's codename.
        /// </summary>
        [EnumMember(Value = "codename")]
        Codename,

        /// <summary>
        /// The taxonomy group's display name.
        /// </summary>
        [EnumMember(Value = "name")]
        Name,

        /// <summary>
        /// All terms in the taxonomy group.
        /// </summary>
        [EnumMember(Value = "terms")]
        Terms,
    }
}
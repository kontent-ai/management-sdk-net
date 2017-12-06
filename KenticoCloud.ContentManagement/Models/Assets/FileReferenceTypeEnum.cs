using System.Runtime.Serialization;

namespace KenticoCloud.ContentManagement.Models.Assets
{
    /// <summary>
    /// Represents file reference type.
    /// </summary>
    public enum FileReferenceTypeEnum
    {
        /// <summary>
        /// Internal reference type.
        /// </summary>
        [EnumMember(Value = "internal")]
        Internal
    }
}

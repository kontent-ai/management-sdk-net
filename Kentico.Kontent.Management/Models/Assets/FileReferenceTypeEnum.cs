using System.Runtime.Serialization;

namespace Kentico.Kontent.Management.Models.Assets
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

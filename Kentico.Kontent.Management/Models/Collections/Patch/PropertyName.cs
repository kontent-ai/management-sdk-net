using System.Runtime.Serialization;

namespace Kentico.Kontent.Management.Models.Collections.Patch
{
    /// <summary>
    /// Represents properties of the collection.
    /// </summary>
    public enum PropertyName
    {
        /// <summary>
        /// Represents the name of the collection.
        /// </summary>
        [EnumMember(Value = "name")]
        Name
    }
}

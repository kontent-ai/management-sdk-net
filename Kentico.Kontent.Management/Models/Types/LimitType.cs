using System.Runtime.Serialization;

namespace Kentico.Kontent.Management.Models.Types
{
    /// <summary>
    /// Defines how to apply the limitation.
    /// </summary>
    public enum LimitType
    {
        /// <summary>
        /// At least.
        /// </summary>
        [EnumMember(Value = "at_least")]
        AtLeast,

        /// <summary>
        /// Exactly.
        /// </summary>
        [EnumMember(Value = "exactly")]
        Exactly,

        /// <summary>
        /// At most.
        /// </summary>
        [EnumMember(Value = "at_most")]
        AtMost
    }
}

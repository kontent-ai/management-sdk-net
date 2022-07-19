using System.Runtime.Serialization;

namespace Kontent.Ai.Management.Models.Collections.Patch;

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

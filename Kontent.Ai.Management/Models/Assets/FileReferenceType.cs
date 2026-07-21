
namespace Kontent.Ai.Management.Models.Assets;

/// <summary>
/// Type of a file reference. Currently the API only uses <see cref="Internal"/>.
/// </summary>
public enum FileReferenceType
{
    /// <summary>
    /// Internal reference type.
    /// </summary>
    [EnumMember(Value = "internal")]
    Internal
}

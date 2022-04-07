using System.Runtime.Serialization;

namespace Kentico.Kontent.Management.Models.Types.Elements;

/// <summary>
/// Determines whether the maximum_text_length applies to characters or words.
/// </summary>
public enum TextLengthLimitType
{
    /// <summary>
    /// Words.
    /// </summary>
    [EnumMember(Value = "words")]
    Words,

    /// <summary>
    /// Characters.
    /// </summary>
    [EnumMember(Value = "characters")]
    Characters
}

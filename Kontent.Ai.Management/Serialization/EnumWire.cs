using System.Reflection;

namespace Kontent.Ai.Management.Serialization;

/// <summary>
/// Resolves an enum member's Management API wire token — its <see cref="EnumMemberAttribute"/> value,
/// falling back to the member name. Used where a token is needed outside JSON serialization, e.g. as a
/// PATCH path segment.
/// </summary>
internal static class EnumWire
{
    public static string ToValue<TEnum>(TEnum value) where TEnum : struct, Enum =>
        typeof(TEnum).GetField(value.ToString())!.GetCustomAttribute<EnumMemberAttribute>()?.Value
        ?? value.ToString();
}

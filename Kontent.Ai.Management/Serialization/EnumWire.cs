using Kontent.Ai.Management.Serialization.Converters;

namespace Kontent.Ai.Management.Serialization;

/// <summary>
/// Resolves an enum member's Management API wire token — its <see cref="EnumMemberAttribute"/> value,
/// falling back to the member name. Used where a token is needed outside JSON serialization, e.g. as a
/// PATCH path segment. Shares the cached map with <see cref="EnumMemberJsonConverter{TEnum}"/> so the two never drift.
/// </summary>
internal static class EnumWire
{
    public static string ToValue<TEnum>(TEnum value) where TEnum : struct, Enum
        => EnumMemberJsonConverter<TEnum>.ToWireValue(value);
}

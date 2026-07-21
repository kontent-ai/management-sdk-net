using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace Kontent.Ai.Management.Serialization;

internal static class JsonElementExtensions
{
    public static bool TryGetStringProperty(this JsonElement element, string name, [NotNullWhen(true)] out string? value)
    {
        if (element.TryGetProperty(name, out var property) && property.ValueKind == JsonValueKind.String)
        {
            value = property.GetString()!;
            return true;
        }

        value = null;
        return false;
    }
}

using System.Text.Json;
using Kontent.Ai.Management.Configuration;

namespace Kontent.Ai.Management.Tests.Base;

/// <summary>
/// Cached System.Text.Json options matching the production Refit serializer — use for any test-side
/// deserialization/serialization of SDK models so attribute-level naming/converters apply identically.
/// </summary>
internal static class SharedTestJsonOptions
{
    public static readonly JsonSerializerOptions Default = RefitSettingsProvider.CreateDefaultJsonSerializerOptions();
}

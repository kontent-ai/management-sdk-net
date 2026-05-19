using System.Text.Json;
using System.Text.Json.Serialization;
using Kontent.Ai.Management.Serialization.Converters;

namespace Kontent.Ai.Management.Configuration;

/// <summary>
/// Builds the Management API Refit settings and the shared System.Text.Json options.
/// </summary>
internal static class RefitSettingsProvider
{
    public static RefitSettings CreateRefitSettings(Action<RefitSettings>? configureRefit = null)
    {
        var settings = new RefitSettings
        {
            ContentSerializer = new SystemTextJsonContentSerializer(CreateDefaultJsonSerializerOptions()),
        };

        configureRefit?.Invoke(settings);
        return settings;
    }

    public static JsonSerializerOptions CreateDefaultJsonSerializerOptions()
    {
        // No PropertyNamingPolicy on purpose: unlike delivery's camelCase policy, MAPI names are not
        // expressible by a single policy (e.g. metadata__description), so every model property carries
        // an explicit [JsonPropertyName]. Case-insensitive matching mirrors Newtonsoft's lenient
        // fallback; MaxDepth matches the Kontent.ai platform nesting limit (same as delivery).
        var options = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNameCaseInsensitive = true,
            MaxDepth = 124,
        };

        options.Converters.Add(new ElementMetadataJsonConverter());
        options.Converters.Add(new ImageTransformationJsonConverter());
        options.Converters.Add(new AsyncValidationTaskIssueJsonConverter());
        options.Converters.Add(new AssetWithRenditionsReferenceJsonConverter());
        options.Converters.Add(new PatchOperationJsonConverterFactory());
        options.Converters.Add(new DecimalJsonConverter());
        options.Converters.Add(new EnumMemberJsonConverterFactory());

        return options;
    }
}

using Kontent.Ai.Management.Serialization.Converters;
using System.Text.Json;

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
        // No PropertyNamingPolicy: every model property carries an explicit [JsonPropertyName], so a
        // policy would have nothing to do. Case-insensitive matching is a lenient read-side fallback;
        // MaxDepth matches the Kontent.ai platform nesting limit.
        var options = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNameCaseInsensitive = true,
            MaxDepth = 124,
            // Newtonsoft accepted string-encoded numbers on read (e.g. element default values `"value": "10"`
            // bound to decimal). System.Text.Json is strict by default; this restores the lenient parity.
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
            // Newtonsoft also tolerated trailing commas in JSON (some hand-authored fixtures rely on it).
            AllowTrailingCommas = true,
        };

        options.Converters.Add(new BaseElementJsonConverter());
        options.Converters.Add(new ElementMetadataJsonConverter());
        options.Converters.Add(new ImageTransformationJsonConverter());
        options.Converters.Add(new AsyncValidationTaskIssueJsonConverter());
        options.Converters.Add(new ReferenceJsonConverter());
        options.Converters.Add(new UserIdentifierJsonConverter());
        options.Converters.Add(new WorkflowStepIdentifierJsonConverter());
        options.Converters.Add(new AssetWithRenditionsReferenceJsonConverter());
        options.Converters.Add(new PatchOperationJsonConverterFactory());
        options.Converters.Add(new DecimalJsonConverter());
        options.Converters.Add(new EnumMemberJsonConverterFactory());
        options.Converters.Add(new ListingResponseJsonConverterFactory());

        return options;
    }
}

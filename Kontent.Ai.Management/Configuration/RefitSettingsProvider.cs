using Kontent.Ai.Management.Serialization.Converters;
using System.Text.Json;

namespace Kontent.Ai.Management.Configuration;

/// <summary>
/// Builds the Management API Refit settings and the shared System.Text.Json options.
/// </summary>
internal static class RefitSettingsProvider
{
    public static RefitSettings CreateDefaultSettings()
        => new()
        {
            ContentSerializer = new SystemTextJsonContentSerializer(CreateDefaultJsonSerializerOptions()),
            CollectionFormat = CollectionFormat.Multi,
            UrlParameterKeyFormatter = new CamelCaseUrlParameterKeyFormatter(),
        };

    public static JsonSerializerOptions CreateDefaultJsonSerializerOptions()
    {
        // No PropertyNamingPolicy: every model property carries an explicit JsonPropertyName attribute, so a
        // policy would have nothing to do. Case-insensitive matching is a lenient read-side fallback;
        // MaxDepth matches the Kontent.ai platform nesting limit.
        var options = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNameCaseInsensitive = true,
            MaxDepth = 124,
            // The Management API sends some numeric values as strings (e.g. a number element's default
            // `"value": "10"` bound to decimal), so tolerate string-encoded numbers on read.
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
            // Tolerate trailing commas — some hand-authored JSON fixtures include them.
            AllowTrailingCommas = true,
        };

        options.Converters.Add(new BaseElementJsonConverter());
        options.Converters.Add(new ElementMetadataJsonConverter());
        options.Converters.Add(new ImageTransformationJsonConverter());
        options.Converters.Add(new AsyncValidationTaskIssueJsonConverter());
        options.Converters.Add(new ReferenceJsonConverter());
        options.Converters.Add(new UserIdentifierJsonConverter());
        options.Converters.Add(new PatchOperationJsonConverterFactory());
        options.Converters.Add(new DecimalJsonConverter());
        options.Converters.Add(new EnumMemberJsonConverterFactory());

        return options;
    }
}

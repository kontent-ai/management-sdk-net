using System.Text.Json.Serialization;
using AwesomeAssertions;
using Kontent.Ai.Management.Configuration;
using Kontent.Ai.Management.Serialization.Converters;
using Refit;
using Xunit;

namespace Kontent.Ai.Management.Tests.Serialization;

public class RefitSettingsProviderTests
{
    [Fact]
    public void DefaultJsonSerializerOptions_HasManagementShape()
    {
        var options = RefitSettingsProvider.CreateDefaultJsonSerializerOptions();

        options.DefaultIgnoreCondition.Should().Be(JsonIgnoreCondition.WhenWritingNull);
        options.PropertyNameCaseInsensitive.Should().BeTrue();
        options.MaxDepth.Should().Be(124);
        options.PropertyNamingPolicy.Should().BeNull();
    }

    [Fact]
    public void DefaultJsonSerializerOptions_RegistersAllWaveOneConverters()
    {
        var converters = RefitSettingsProvider.CreateDefaultJsonSerializerOptions().Converters;

        converters.Should().ContainSingle(c => c is ElementMetadataJsonConverter);
        converters.Should().ContainSingle(c => c is ImageTransformationJsonConverter);
        converters.Should().ContainSingle(c => c is AsyncValidationTaskIssueJsonConverter);
        converters.Should().ContainSingle(c => c is AssetWithRenditionsReferenceJsonConverter);
        converters.Should().ContainSingle(c => c is PatchOperationJsonConverterFactory);
        converters.Should().ContainSingle(c => c is DecimalJsonConverter);
    }

    [Fact]
    public void CreateRefitSettings_UsesSystemTextJsonContentSerializer()
    {
        var settings = RefitSettingsProvider.CreateRefitSettings();

        settings.ContentSerializer.Should().BeOfType<SystemTextJsonContentSerializer>();
    }

    [Fact]
    public void CreateRefitSettings_AppliesConfigureHook()
    {
        var settings = RefitSettingsProvider.CreateRefitSettings(s => s.Buffered = true);

        settings.Buffered.Should().BeTrue();
    }
}

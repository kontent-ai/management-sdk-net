using AwesomeAssertions;
using Kontent.Ai.Management.Models.AssetRenditions;
using Kontent.Ai.Management.Models.EnvironmentValidation;
using Kontent.Ai.Management.Models.Types.Elements;
using Kontent.Ai.Management.Serialization.Converters;
using System.Text.Json;

namespace Kontent.Ai.Management.Tests.Serialization;

// These tests cover the converters' dispatch and recursion guard in isolation, not full model fidelity —
// the models carry explicit [JsonPropertyName] on every property, so no naming policy is involved.
public class PolymorphicConvertersTests
{
    private static JsonSerializerOptions Options(System.Text.Json.Serialization.JsonConverter converter) => new()
    {
        Converters = { converter },
    };

    [Fact]
    public void ElementMetadata_Read_DispatchesToConcreteSubtype()
    {
        var options = Options(new ElementMetadataJsonConverter());

        var text = JsonSerializer.Deserialize<ElementMetadataBase>(
            """{"name":"Title","is_required":false,"type":"text","external_id":"ext","id":"ba7c8840-bcbc-5e3b-b292-24d0a60f3977","codename":"title","is_non_localizable":true}""",
            options);

        var guidelines = JsonSerializer.Deserialize<ElementMetadataBase>(
            """{"type":"guidelines","external_id":"g","id":"f9f65756-5ed8-55ed-9d2a-43f9fe7974cd","codename":"gl","guidelines":"<p>x</p>"}""",
            options);

        text.Should().BeOfType<TextElementMetadataModel>();
        ((TextElementMetadataModel)text!).Name.Should().Be("Title");
        ((TextElementMetadataModel)text!).IsRequired.Should().BeFalse();
        text!.Codename.Should().Be("title");
        text.ExternalId.Should().Be("ext");

        guidelines.Should().BeOfType<GuidelinesElementMetadataModel>();
        guidelines!.Codename.Should().Be("gl");
    }

    [Fact]
    public void ElementMetadata_Read_UnknownType_Throws()
    {
        var act = () => JsonSerializer.Deserialize<ElementMetadataBase>(
            """{"type":"bogus"}""", Options(new ElementMetadataJsonConverter()));

        act.Should().Throw<JsonException>().WithMessage("*bogus*");
    }

    [Fact]
    public void ElementMetadata_Read_MissingDiscriminator_Throws()
    {
        var act = () => JsonSerializer.Deserialize<ElementMetadataBase>(
            """{"codename":"x"}""", Options(new ElementMetadataJsonConverter()));

        act.Should().Throw<JsonException>().WithMessage("*type*");
    }

    [Fact]
    public void ElementMetadata_Write_EmitsRuntimeSubtype_WithoutRecursion()
    {
        var elements = new List<ElementMetadataBase>
        {
            new TextElementMetadataModel { Name = "T", IsRequired = true, Codename = "t" },
        };

        var json = JsonSerializer.Serialize(elements, Options(new ElementMetadataJsonConverter()));

        json.Should().Contain("\"name\":\"T\"").And.Contain("\"is_required\":true");
    }

    [Fact]
    public void ImageTransformation_Read_DispatchesToRectangle()
    {
        var result = JsonSerializer.Deserialize<ImageTransformation>(
            """{"mode":"rect","custom_width":1,"custom_height":1,"x":0,"y":0,"width":1,"height":1}""",
            Options(new ImageTransformationJsonConverter()));

        result.Should().BeOfType<RectangleResizeTransformation>();
    }

    [Fact]
    public void ImageTransformation_Read_UnknownMode_Throws()
    {
        var act = () => JsonSerializer.Deserialize<ImageTransformation>(
            """{"mode":"spin"}""", Options(new ImageTransformationJsonConverter()));

        act.Should().Throw<JsonException>().WithMessage("*spin*");
    }

    [Fact]
    public void AsyncValidationTaskIssue_Read_UnknownIssueType_Throws()
    {
        // Positive dispatch for issue_type-discriminated models needs the production string-enum
        // configuration (the discriminator is also an enum-typed model property). The shared
        // dispatch + recursion logic is exercised by the element-metadata cases above.
        var act = () => JsonSerializer.Deserialize<AsyncValidationTaskIssueModel>(
            """{"issue_type":"bogus"}""", Options(new AsyncValidationTaskIssueJsonConverter()));

        act.Should().Throw<JsonException>().WithMessage("*bogus*");
    }

    [Fact]
    public void Converters_CanConvert_MatchesAbstractBaseOnly()
    {
        new ElementMetadataJsonConverter().CanConvert(typeof(ElementMetadataBase)).Should().BeTrue();
        new ElementMetadataJsonConverter().CanConvert(typeof(TextElementMetadataModel)).Should().BeFalse();
        new ImageTransformationJsonConverter().CanConvert(typeof(RectangleResizeTransformation)).Should().BeFalse();
        new AsyncValidationTaskIssueJsonConverter().CanConvert(typeof(AsyncValidationTaskIssueModel)).Should().BeTrue();
    }
}

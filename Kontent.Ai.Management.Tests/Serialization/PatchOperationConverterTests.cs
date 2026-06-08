using AwesomeAssertions;
using Kontent.Ai.Management.Models.AssetFolders;
using Kontent.Ai.Management.Models.AssetFolders.Patch;
using Kontent.Ai.Management.Serialization.Converters;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Kontent.Ai.Management.Tests.Serialization;

public class PatchOperationConverterTests
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        Converters = { new PatchOperationJsonConverterFactory() },
    };

    [Fact]
    public void Write_HeterogeneousOps_EmitsRuntimeTypeInOrder()
    {
        var operations = new List<AssetFolderOperationBaseModel>
        {
            new AssetFolderAddIntoModel { Value = new AssetFolderHierarchy { Name = "new-folder" } },
            new AssetFolderRemoveModel(),
            new AssetFolderRenameModel { Value = "renamed" },
        };

        var array = JsonNode.Parse(JsonSerializer.Serialize(operations, Options))!.AsArray();

        array.Should().HaveCount(3);
        array[0]!["op"]!.GetValue<string>().Should().Be("addInto");
        array[1]!["op"]!.GetValue<string>().Should().Be("remove");
        array[2]!["op"]!.GetValue<string>().Should().Be("rename");
        array[2]!["value"]!.GetValue<string>().Should().Be("renamed");
    }

    [Fact]
    public void Read_IsNotSupported()
    {
        var act = () => JsonSerializer.Deserialize<AssetFolderOperationBaseModel>("{}", Options);

        act.Should().Throw<NotSupportedException>();
    }

    [Fact]
    public void Factory_CanConvert_AbstractBaseOnly()
    {
        var factory = new PatchOperationJsonConverterFactory();

        factory.CanConvert(typeof(AssetFolderOperationBaseModel)).Should().BeTrue();
        factory.CanConvert(typeof(AssetFolderAddIntoModel)).Should().BeFalse();
    }
}

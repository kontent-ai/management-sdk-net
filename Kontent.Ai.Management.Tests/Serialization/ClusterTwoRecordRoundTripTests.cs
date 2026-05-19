using System.IO;
using AwesomeAssertions;
using Kontent.Ai.Management.Models.AssetFolders.Patch;
using Kontent.Ai.Management.Models.AssetRenditions;
using Kontent.Ai.Management.Modules.ActionInvoker;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Kontent.Ai.Management.Tests.Serialization;

// Wave-3 cluster-2 gate. Strongest required+Newtonsoft case so far: RectangleResizeTransformation
// (9 `required` members) read polymorphically through the retained Newtonsoft [JsonConverter] on the
// now-abstract-record ImageTransformation. Full suite is the primary differential vs removed classes.
public class ClusterTwoRecordRoundTripTests
{
    private static readonly JsonSerializerSettings RequestSettings = new()
    {
        NullValueHandling = NullValueHandling.Ignore,
        Converters = { new DecimalObjectConverter(), new StringEnumConverter() },
    };

    private static readonly JsonSerializerSettings ResponseSettings = new()
    {
        Converters = { new DynamicObjectJsonConverter() },
    };

    private static string Fixture(string sub, string name)
        => File.ReadAllText(Path.Combine(System.Environment.CurrentDirectory, "Data", sub, name));

    [Fact]
    public void ImageTransformation_PolymorphicConverterReadIntoRequiredRecord_RoundTrips()
    {
        var transformation = JObject.Parse(Fixture("AssetRendition", "AssetRendition.json"))["transformation"]!.ToString();

        var model = JsonConvert.DeserializeObject<ImageTransformation>(transformation, ResponseSettings);

        model.Should().BeOfType<RectangleResizeTransformation>();
        JToken.DeepEquals(
            JToken.Parse(JsonConvert.SerializeObject(model, RequestSettings)),
            JToken.Parse(transformation)).Should().BeTrue();
    }

    [Fact]
    public void AssetRenditionModel_WithNestedPolymorphicTransformation_RoundTrips()
    {
        var json = Fixture("AssetRendition", "AssetRendition.json");

        var reserialized = JsonConvert.SerializeObject(
            JsonConvert.DeserializeObject<AssetRenditionModel>(json, ResponseSettings), RequestSettings);

        JToken.DeepEquals(JToken.Parse(reserialized), JToken.Parse(json)).Should().BeTrue();
    }

    [Fact]
    public void AssetRenditionCreateModel_RequiredNestedAbstractRecord_RoundTrips()
    {
        var model = new AssetRenditionCreateModel
        {
            ExternalId = "rendition-1",
            Transformation = new RectangleResizeTransformation
            {
                Fit = ImageTransformationFit.Clip,
                CustomWidth = 120,
                CustomHeight = 240,
                X = 300,
                Y = 200,
                Width = 360,
                Height = 720,
            },
        };

        var first = JsonConvert.SerializeObject(model, RequestSettings);
        var second = JsonConvert.SerializeObject(
            JsonConvert.DeserializeObject<AssetRenditionCreateModel>(first, ResponseSettings), RequestSettings);

        JToken.DeepEquals(JToken.Parse(first), JToken.Parse(second)).Should().BeTrue();
        JObject.Parse(first)["transformation"]!["mode"]!.Value<string>().Should().Be("rect");
    }

    [Fact]
    public void AssetFolderPatch_AbstractRecordWithSealedDerived_RoundTrips()
    {
        AssetFolderOperationBaseModel rename = new AssetFolderRenameModel { Value = "Renamed folder" };
        AssetFolderOperationBaseModel remove = new AssetFolderRemoveModel();

        foreach (var op in new[] { rename, remove })
        {
            var concrete = op.GetType();
            var first = JsonConvert.SerializeObject(op, RequestSettings);
            var second = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(first, concrete, RequestSettings), RequestSettings);
            JToken.DeepEquals(JToken.Parse(first), JToken.Parse(second)).Should().BeTrue();
        }

        JObject.Parse(JsonConvert.SerializeObject(rename, RequestSettings))["op"]!.Value<string>().Should().Be("rename");
        JObject.Parse(JsonConvert.SerializeObject(remove, RequestSettings))["op"]!.Value<string>().Should().Be("remove");
    }
}

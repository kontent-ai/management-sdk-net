using System.Collections.Generic;
using System.IO;
using AwesomeAssertions;
using Kontent.Ai.Management.Models.Collections;
using Kontent.Ai.Management.Models.Collections.Patch;
using Kontent.Ai.Management.Models.EnvironmentValidation;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.Spaces.Patch;
using Kontent.Ai.Management.Modules.ActionInvoker;
using SpacePropertyName = Kontent.Ai.Management.Models.Spaces.Patch.PropertyName;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Kontent.Ai.Management.Tests.Serialization;

// Wave-3 cluster-1 gate: extends the pilot's Newtonsoft byte-identical round-trip proof to the
// archetypes the pilot did NOT exercise — `required`-bearing records, abstract-record + sealed
// derived PATCH hierarchies, and the static-Op/required-dynamic shape. The full suite is the
// primary differential vs the removed classes; this isolates the new Newtonsoft mechanics.
public class ClusterOneRecordRoundTripTests
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

    private static void RoundTripsIdentically<T>(string json, JsonSerializerSettings settings)
    {
        var reserialized = JsonConvert.SerializeObject(JsonConvert.DeserializeObject<T>(json, settings), RequestSettings);
        JToken.DeepEquals(JToken.Parse(reserialized), JToken.Parse(json))
            .Should().BeTrue($"record round-trip must equal the original payload but was:\n{reserialized}");
    }

    [Fact]
    public void CollectionModel_RequiredRecord_RoundTripsEveryEntry()
    {
        var json = File.ReadAllText(Path.Combine(System.Environment.CurrentDirectory, "Data", "Collection", "Collections.json"));
        foreach (var entry in JObject.Parse(json)["collections"]!.Children())
        {
            RoundTripsIdentically<CollectionModel>(entry.ToString(), RequestSettings);
        }
    }

    [Fact]
    public void CollectionCreateModel_RequiredRecord_RoundTrips()
    {
        var model = new CollectionCreateModel { Name = "Marketing", Codename = "marketing", ExternalId = "ext-1" };

        var first = JsonConvert.SerializeObject(model, RequestSettings);
        var second = JsonConvert.SerializeObject(JsonConvert.DeserializeObject<CollectionCreateModel>(first), RequestSettings);

        JToken.DeepEquals(JToken.Parse(first), JToken.Parse(second)).Should().BeTrue();
        JObject.Parse(first).Should().BeEquivalentTo(JObject.Parse(
            """{"name":"Marketing","codename":"marketing","external_id":"ext-1"}"""));
    }

    [Fact]
    public void CollectionPatch_AbstractRecordWithSealedDerived_RoundTrips()
    {
        CollectionOperationBaseModel addInto = new CollectionAddIntoPatchModel
        {
            Value = new CollectionCreateModel { Name = "New" },
            Before = Reference.ByCodename("existing"),
        };
        CollectionOperationBaseModel move = new CollectionMovePatchModel
        {
            Reference = Reference.ByCodename("to_move"),
            After = Reference.ByCodename("anchor"),
        };

        foreach (var op in new[] { addInto, move })
        {
            // PATCH bodies serialize by runtime type (Newtonsoft default); deserialize the concrete back.
            var concrete = op.GetType();
            var first = JsonConvert.SerializeObject(op, RequestSettings);
            var second = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(first, concrete, RequestSettings), RequestSettings);
            JToken.DeepEquals(JToken.Parse(first), JToken.Parse(second)).Should().BeTrue();
        }

        JObject.Parse(JsonConvert.SerializeObject(addInto, RequestSettings))["op"]!.Value<string>().Should().Be("addInto");
        JObject.Parse(JsonConvert.SerializeObject(move, RequestSettings))["op"]!.Value<string>().Should().Be("move");
    }

    [Fact]
    public void SpaceOperationReplaceModel_RequiredStaticDynamic_RoundTrips()
    {
        var model = new SpaceOperationReplaceModel { PropertyName = SpacePropertyName.Name, Value = "Marketing space" };

        var first = JsonConvert.SerializeObject(model, RequestSettings);
        var second = JsonConvert.SerializeObject(JsonConvert.DeserializeObject<SpaceOperationReplaceModel>(first, RequestSettings), RequestSettings);

        JToken.DeepEquals(JToken.Parse(first), JToken.Parse(second)).Should().BeTrue();
        var obj = JObject.Parse(first);
        obj["property_name"]!.Value<string>().Should().Be("name");
        obj["value"]!.Value<string>().Should().Be("Marketing space");
    }

    [Fact]
    public void AsyncValidationTaskIssue_AbstractRecordPolymorphicRead_RoundTrips()
    {
        var json = File.ReadAllText(Path.Combine(System.Environment.CurrentDirectory, "Data", "ProjectValidation", "ExpectedAsyncValidationTaskIssues.json"));

        var issues = JsonConvert.DeserializeObject<List<AsyncValidationTaskIssueModel>>(json, ResponseSettings);

        issues.Should().ContainSingle().Which.Should().BeOfType<AsyncValidationTaskVariantIssueModel>();
        JToken.DeepEquals(
            JToken.Parse(JsonConvert.SerializeObject(issues, RequestSettings)),
            JToken.Parse(json)).Should().BeTrue();
    }
}

using System.IO;
using AwesomeAssertions;
using Kontent.Ai.Management.Models.Items;
using Kontent.Ai.Management.Models.LanguageVariants;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.Workflow;
using Kontent.Ai.Management.Modules.ActionInvoker;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Xunit;
using StronglyTypedVariant = Kontent.Ai.Management.Models.StronglyTyped.LanguageVariantModel<object>;

namespace Kontent.Ai.Management.Tests.Serialization;

// Wave-3 cluster-4 gate (final conversion cluster). Novel shapes: records carrying
// IEnumerable<dynamic> Elements (legacy variant payload), nested WorkflowStepIdentifier record,
// and a generic strongly-typed record. Dynamic/fixture-heavy cases use idempotent round-trip
// stability; the broad LanguageVariant/ContentItem suites are the wire differential.
public class ClusterFourRecordRoundTripTests
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

    private static void RoundTripIsStable<T>(string json)
    {
        var first = JsonConvert.SerializeObject(JsonConvert.DeserializeObject<T>(json, ResponseSettings), RequestSettings);
        var second = JsonConvert.SerializeObject(JsonConvert.DeserializeObject<T>(first, ResponseSettings), RequestSettings);
        JToken.DeepEquals(JToken.Parse(first), JToken.Parse(second))
            .Should().BeTrue($"record round-trip must be idempotent but diverged:\n{first}\n---\n{second}");
    }

    [Fact]
    public void LanguageVariantModel_DynamicElementsRecord_RoundTripIsStable()
    {
        var json = Fixture("LanguageVariant", "LanguageVariant.json");

        var model = JsonConvert.DeserializeObject<LanguageVariantModel>(json, ResponseSettings);

        model.Should().BeOfType<LanguageVariantModel>();
        model!.Item.Should().NotBeNull();
        RoundTripIsStable<LanguageVariantModel>(json);
    }

    [Fact]
    public void LanguageVariantUpsertModel_DynamicElements_NestedRecordWorkflow_RoundTrips()
    {
        var model = new LanguageVariantUpsertModel
        {
            Elements = new object[] { new { element = new { id = "11111111-1111-1111-1111-111111111111" }, value = "v" } },
            Workflow = new WorkflowStepIdentifier(Reference.ByCodename("default"), Reference.ByCodename("draft")),
            Note = "note",
        };

        var first = JsonConvert.SerializeObject(model, RequestSettings);
        var second = JsonConvert.SerializeObject(JsonConvert.DeserializeObject<LanguageVariantUpsertModel>(first, ResponseSettings), RequestSettings);

        JToken.DeepEquals(JToken.Parse(first), JToken.Parse(second)).Should().BeTrue();
        var obj = JObject.Parse(first);
        obj["note"]!.Value<string>().Should().Be("note");
        obj["workflow"]!["workflow_identifier"]!["codename"]!.Value<string>().Should().Be("default");
    }

    [Fact]
    public void ContentItemModel_Record_RoundTripIsStable()
        => RoundTripIsStable<ContentItemModel>(Fixture("CodeSamples", "ContentItem.json"));

    [Fact]
    public void StronglyTypedLanguageVariantModel_GenericRecord_RoundTrips()
    {
        var model = new StronglyTypedVariant
        {
            Item = Reference.ById(System.Guid.NewGuid()),
            Language = Reference.ByCodename("default"),
            Elements = new object(),
        };

        var first = JsonConvert.SerializeObject(model, RequestSettings);
        var second = JsonConvert.SerializeObject(JsonConvert.DeserializeObject<StronglyTypedVariant>(first, ResponseSettings), RequestSettings);

        JToken.DeepEquals(JToken.Parse(first), JToken.Parse(second)).Should().BeTrue();
        JObject.Parse(first)["language"]!["codename"]!.Value<string>().Should().Be("default");
    }
}

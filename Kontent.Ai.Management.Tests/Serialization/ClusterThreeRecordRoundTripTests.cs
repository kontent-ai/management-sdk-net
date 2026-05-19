using System.Collections.Generic;
using System.IO;
using System.Linq;
using AwesomeAssertions;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.TaxonomyGroups;
using Kontent.Ai.Management.Models.Types;
using Kontent.Ai.Management.Models.Types.Elements;
using Kontent.Ai.Management.Models.Types.Patch;
using Kontent.Ai.Management.Models.Workflow;
using Kontent.Ai.Management.Modules.ActionInvoker;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Kontent.Ai.Management.Tests.Serialization;

// Wave-3 cluster-3 gate. Centerpiece: ContentTypeModel's heterogeneous `elements` (all 13
// ElementMetadataBase subtypes) read through the retained Newtonsoft [JsonConverter] into record
// subtypes. ContentType.json carries explicit nulls, so the polymorphic case is gated by
// idempotent round-trip stability + subtype-dispatch assertion (not brittle raw-fixture equality);
// the broad ContentTypeTests/Taxonomy/Workflow/etc. suites are the wire differential vs the classes.
public class ClusterThreeRecordRoundTripTests
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

    [Fact]
    public void ContentTypeModel_PolymorphicElements_DispatchIntoRecordSubtypes_AndRoundTripIsStable()
    {
        var json = File.ReadAllText(Path.Combine(System.Environment.CurrentDirectory, "Data", "ContentType", "ContentType.json"));

        var model = JsonConvert.DeserializeObject<ContentTypeModel>(json, ResponseSettings);

        // The Newtonsoft [JsonConverter] on the now-abstract-record ElementMetadataBase must still
        // dispatch every element to its concrete record subtype.
        var subtypes = model!.Elements.Select(e => e.GetType()).ToHashSet();
        subtypes.IsSupersetOf(new[]
        {
            typeof(TextElementMetadataModel), typeof(RichTextElementMetadataModel), typeof(NumberElementMetadataModel),
            typeof(MultipleChoiceElementMetadataModel), typeof(DateTimeElementMetadataModel), typeof(AssetElementMetadataModel),
            typeof(LinkedItemsElementMetadataModel), typeof(GuidelinesElementMetadataModel), typeof(TaxonomyElementMetadataModel),
            typeof(UrlSlugElementMetadataModel), typeof(ContentTypeSnippetElementMetadataModel), typeof(CustomElementMetadataModel),
            typeof(SubpagesElementMetadataModel),
        }).Should().BeTrue("every element type must dispatch to its concrete record subtype");

        var first = JsonConvert.SerializeObject(model, RequestSettings);
        var second = JsonConvert.SerializeObject(
            JsonConvert.DeserializeObject<ContentTypeModel>(first, ResponseSettings), RequestSettings);

        JToken.DeepEquals(JToken.Parse(first), JToken.Parse(second))
            .Should().BeTrue($"record round-trip must be idempotent but diverged:\n{first}\n---\n{second}");
    }

    [Fact]
    public void ContentTypePatch_AbstractRecordWithSealedDerived_RoundTrips()
    {
        ContentTypeOperationBaseModel addInto = new ContentTypeAddIntoPatchModel { Path = "/elements", Value = "x" };
        ContentTypeOperationBaseModel remove = new ContentTypeRemovePatchModel { Path = "/elements/id:abc" };

        foreach (var op in new[] { addInto, remove })
        {
            var concrete = op.GetType();
            var a = JsonConvert.SerializeObject(op, RequestSettings);
            var b = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(a, concrete, RequestSettings), RequestSettings);
            JToken.DeepEquals(JToken.Parse(a), JToken.Parse(b)).Should().BeTrue();
        }

        JObject.Parse(JsonConvert.SerializeObject(addInto, RequestSettings))["op"]!.Value<string>().Should().Be("addInto");
        JObject.Parse(JsonConvert.SerializeObject(remove, RequestSettings))["op"]!.Value<string>().Should().Be("remove");
    }

    [Fact]
    public void TaxonomyTermModel_RecursiveWithConcreteRecordBase_RoundTrips()
    {
        var model = new TaxonomyTermModel
        {
            Name = "Root",
            Codename = "root",
            Terms = new[]
            {
                new TaxonomyTermModel { Name = "Child", Codename = "child", Terms = System.Array.Empty<TaxonomyTermModel>() },
            },
        };

        var first = JsonConvert.SerializeObject(model, RequestSettings);
        var second = JsonConvert.SerializeObject(JsonConvert.DeserializeObject<TaxonomyTermModel>(first, ResponseSettings), RequestSettings);

        JToken.DeepEquals(JToken.Parse(first), JToken.Parse(second)).Should().BeTrue();
        var obj = JObject.Parse(first);
        obj["name"]!.Value<string>().Should().Be("Root");
        obj["terms"]![0]!["codename"]!.Value<string>().Should().Be("child");
    }

    [Fact]
    public void WorkflowStepIdentifier_ValueObjectRecord_RoundTrips()
    {
        var model = new WorkflowStepIdentifier(Reference.ByCodename("default"), Reference.ByCodename("draft"));

        var first = JsonConvert.SerializeObject(model, RequestSettings);
        var second = JsonConvert.SerializeObject(JsonConvert.DeserializeObject<WorkflowStepIdentifier>(first, ResponseSettings), RequestSettings);

        JToken.DeepEquals(JToken.Parse(first), JToken.Parse(second)).Should().BeTrue();
        JObject.Parse(first)["workflow_identifier"]!["codename"]!.Value<string>().Should().Be("default");
    }
}

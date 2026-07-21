using AwesomeAssertions;
using Kontent.Ai.Management.Models.Collections.Patch;
using Kontent.Ai.Management.Models.Types.Elements.DefaultValues;
using Kontent.Ai.Management.Models.Workflow;
using Kontent.Ai.Management.Tests.Base;
using System.Text.Json;

namespace Kontent.Ai.Management.Tests.Serialization;

// The ergonomic defaults and convenience constructors must keep the wire contract byte-identical to the explicit
// forms they replace: role arrays always serialize (empty = no restriction), the collection patch always names its
// property, and the default-value constructors produce the { "global": { "value": ... } } envelope.
public class RequestDefaultsSerializationTests
{
    private static string Serialize<T>(T model) => JsonSerializer.Serialize(model, SharedTestJsonOptions.Default);

    [Fact]
    public void WorkflowStep_OmittedRoleIds_SerializesEmptyArray()
    {
        var step = new WorkflowStepUpsertModel
        {
            Name = "Draft",
            Color = WorkflowStepColor.Red,
            TransitionsTo = [],
        };

        Serialize(step).Should().Contain("\"role_ids\":[]");
    }

    [Fact]
    public void WorkflowPublishedAndArchivedSteps_OmittedRoleLists_SerializeEmptyArrays()
    {
        Serialize(new WorkflowPublishedStepUpsertModel())
            .Should().Contain("\"create_new_version_role_ids\":[]").And.Contain("\"unpublish_role_ids\":[]");
        Serialize(new WorkflowArchivedStepUpsertModel()).Should().Contain("\"role_ids\":[]");
    }

    [Fact]
    public void CollectionReplacePatch_OmittedPropertyName_SerializesName()
    {
        var patch = new CollectionReplacePatchModel
        {
            Reference = Reference.ByCodename("marketing"),
            Value = "Marketing site",
        };

        Serialize(patch).Should().Contain("\"property_name\":\"name\"");
    }

    [Fact]
    public void TextDefaultValue_Constructor_ProducesGlobalValueEnvelope()
    {
        Serialize(new TextElementDefaultValueModel("Hello"))
            .Should().Be("""{"global":{"value":"Hello"}}""");
    }

    [Fact]
    public void NumberDefaultValue_Constructor_ProducesGlobalValueEnvelope()
    {
        Serialize(new NumberElementDefaultValueModel(42.5m))
            .Should().Be("""{"global":{"value":42.5}}""");
    }

    [Fact]
    public void LinkedItemsDefaultValue_Constructor_ProducesGlobalValueEnvelope()
    {
        var id = Guid.NewGuid();

        Serialize(new LinkedItemsElementDefaultValueModel(Reference.ById(id)))
            .Should().Be($$$"""{"global":{"value":[{"id":"{{{id}}}"}]}}""");
    }

    [Fact]
    public void DefaultValue_Constructor_EqualsInitializerForm()
    {
        new TextElementDefaultValueModel("Hello")
            .Should().Be(new TextElementDefaultValueModel { Global = new() { Value = "Hello" } });
    }
}

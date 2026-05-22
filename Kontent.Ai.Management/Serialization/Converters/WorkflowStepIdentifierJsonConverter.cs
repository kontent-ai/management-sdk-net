using System.Text.Json;
using Kontent.Ai.Management.Models.Workflow;

namespace Kontent.Ai.Management.Serialization.Converters;

/// <summary>
/// Constructs <see cref="WorkflowStepIdentifier"/> via its only public constructor — System.Text.Json
/// cannot bind otherwise because the constructor parameter names (<c>workflowIdentifier</c>,
/// <c>stepIdentifier</c>) do not match the property names (<c>Workflow</c>, <c>Step</c>) carrying the
/// <c>[JsonPropertyName]</c> attributes that describe the wire shape.
/// </summary>
internal sealed class WorkflowStepIdentifierJsonConverter : JsonConverter<WorkflowStepIdentifier>
{
    public override WorkflowStepIdentifier Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var document = JsonDocument.ParseValue(ref reader);
        var root = document.RootElement;

        var workflow = ReadReference(root, "workflow_identifier", options)
            ?? throw new JsonException("WorkflowStepIdentifier requires 'workflow_identifier'.");
        var step = ReadReference(root, "step_identifier", options)
            ?? throw new JsonException("WorkflowStepIdentifier requires 'step_identifier'.");

        return new WorkflowStepIdentifier(workflow, step);
    }

    public override void Write(Utf8JsonWriter writer, WorkflowStepIdentifier value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("workflow_identifier");
        JsonSerializer.Serialize(writer, value.Workflow, options);
        writer.WritePropertyName("step_identifier");
        JsonSerializer.Serialize(writer, value.Step, options);
        writer.WriteEndObject();
    }

    private static Reference? ReadReference(JsonElement root, string name, JsonSerializerOptions options)
        => root.TryGetProperty(name, out var element) && element.ValueKind == JsonValueKind.Object
            ? element.Deserialize<Reference>(options)
            : null;
}

using Kontent.Ai.Management.Models.Workflow;
using System.Text.Json;

namespace Kontent.Ai.Management.Serialization.Converters;

/// <summary>
/// Constructs <see cref="WorkflowStepIdentifier"/> via its only public constructor and maps the
/// <c>workflow_identifier</c> / <c>step_identifier</c> wire properties to its <c>Workflow</c> / <c>Step</c>
/// members. The constructor parameter names do not match the wire names, so System.Text.Json cannot bind the
/// single parameterized constructor on its own.
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

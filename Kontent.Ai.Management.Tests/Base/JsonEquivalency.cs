using AwesomeAssertions;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Kontent.Ai.Management.Tests.Base;

/// <summary>
/// Compares two model instances by serializing both with the production STJ options and asserting
/// <see cref="JsonNode.DeepEquals"/>. Use in MockHttp tests where the model carries
/// <c>JsonElement</c> payloads (e.g. <c>Elements</c>) and the hand-built <c>expected</c> has been
/// replaced with <c>JsonSerializer.Deserialize&lt;T&gt;(fixture, SharedTestJsonOptions.Default)</c>
/// — both sides now come from the same deser pipeline so their canonical JSON is identical.
/// </summary>
internal static class JsonEquivalency
{
    public static void ShouldEqualAsJson<T>(this T subject, T expected)
    {
        var subjectJson = JsonSerializer.Serialize(subject, SharedTestJsonOptions.Default);
        var expectedJson = JsonSerializer.Serialize(expected, SharedTestJsonOptions.Default);
        JsonNode.DeepEquals(JsonNode.Parse(subjectJson), JsonNode.Parse(expectedJson))
            .Should().BeTrue($"models must serialize identically.\nsubject:  {subjectJson}\nexpected: {expectedJson}");
    }

    public static void ShouldEqualAsJson<T>(this IEnumerable<T> subject, IEnumerable<T> expected)
    {
        var subjectJson = JsonSerializer.Serialize(subject, SharedTestJsonOptions.Default);
        var expectedJson = JsonSerializer.Serialize(expected, SharedTestJsonOptions.Default);
        JsonNode.DeepEquals(JsonNode.Parse(subjectJson), JsonNode.Parse(expectedJson))
            .Should().BeTrue($"collections must serialize identically.\nsubject:  {subjectJson}\nexpected: {expectedJson}");
    }
}

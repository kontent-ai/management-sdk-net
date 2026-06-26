using AwesomeAssertions;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Kontent.Ai.Management.Tests.Base;

/// <summary>
/// Compares two model instances by serializing both with the production STJ options and asserting
/// <see cref="JsonNode.DeepEquals"/>. Use in MockHttp tests where the model carries <c>JsonElement</c>
/// payloads (e.g. <c>Elements</c>) and both sides come from the same deserialization pipeline, so their
/// canonical JSON is directly comparable.
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

    /// <summary>
    /// Asserts a captured request body, deserialized to <typeparamref name="TModel"/>, equals <paramref name="model"/>
    /// after a serialize→deserialize round-trip through the production options (so the comparison ignores JSON
    /// formatting / property order unless <paramref name="strictOrdering"/> is set). Also asserts the body was captured.
    /// </summary>
    public static void ShouldMatchSerialized<TModel>(this CapturedBody captured, TModel model, bool strictOrdering = false)
    {
        captured.Value.Should().NotBeNull();
        var actual = JsonSerializer.Deserialize<TModel>(captured.Value!, SharedTestJsonOptions.Default);
        var expected = JsonSerializer.Deserialize<TModel>(JsonSerializer.Serialize(model, SharedTestJsonOptions.Default), SharedTestJsonOptions.Default);
        actual.Should().BeEquivalentTo(expected, opt => strictOrdering ? opt.WithStrictOrdering() : opt);
    }
}

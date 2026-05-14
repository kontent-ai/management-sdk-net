using System.Text.Json;
using FluentAssertions;
using Kontent.Ai.Management.Models.Content;
using Xunit;

namespace Kontent.Ai.Management.Tests.Content;

public class ReferenceTests
{
    private static readonly Guid SampleId = new("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");

    [Fact]
    public void ById_SetsIdOnly()
    {
        var reference = Reference.ById(SampleId);

        reference.Id.Should().Be(SampleId);
        reference.Codename.Should().BeNull();
        reference.ExternalId.Should().BeNull();
    }

    [Fact]
    public void ByCodename_SetsCodenameOnly()
    {
        var reference = Reference.ByCodename("article");

        reference.Id.Should().BeNull();
        reference.Codename.Should().Be("article");
        reference.ExternalId.Should().BeNull();
    }

    [Fact]
    public void ByExternalId_SetsExternalIdOnly()
    {
        var reference = Reference.ByExternalId("my-id");

        reference.Id.Should().BeNull();
        reference.Codename.Should().BeNull();
        reference.ExternalId.Should().Be("my-id");
    }

    [Fact]
    public void Deserialize_AllFields()
    {
        var json = $$"""
            {
              "id": "{{SampleId}}",
              "codename": "article",
              "external_id": "my-id"
            }
            """;

        var reference = JsonSerializer.Deserialize<Reference>(json);

        reference!.Id.Should().Be(SampleId);
        reference.Codename.Should().Be("article");
        reference.ExternalId.Should().Be("my-id");
    }

    [Theory]
    [InlineData("""{ "id": "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa" }""", true, false, false)]
    [InlineData("""{ "codename": "article" }""", false, true, false)]
    [InlineData("""{ "external_id": "my-id" }""", false, false, true)]
    public void Deserialize_EachIdentifierShape(string json, bool hasId, bool hasCodename, bool hasExternalId)
    {
        var reference = JsonSerializer.Deserialize<Reference>(json)!;

        reference.Id.HasValue.Should().Be(hasId);
        (reference.Codename != null).Should().Be(hasCodename);
        (reference.ExternalId != null).Should().Be(hasExternalId);
    }

    [Fact]
    public void Serialize_UsesSnakeCaseExternalId()
    {
        var reference = Reference.ByExternalId("my-id");

        var json = JsonSerializer.Serialize(reference);

        json.Should().Contain("\"external_id\":\"my-id\"");
    }

    [Fact]
    public void RoundTrip_PreservesValue()
    {
        var original = Reference.ByCodename("article");

        var json = JsonSerializer.Serialize(original);
        var roundtripped = JsonSerializer.Deserialize<Reference>(json);

        roundtripped.Should().Be(original);
    }

    [Fact]
    public void RecordEqualityHoldsByValue()
    {
        Reference.ById(SampleId).Should().Be(Reference.ById(SampleId));
        Reference.ByCodename("x").Should().NotBe(Reference.ByCodename("y"));
    }
}

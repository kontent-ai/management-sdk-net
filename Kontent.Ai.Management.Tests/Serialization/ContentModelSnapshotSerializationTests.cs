using AwesomeAssertions;
using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.ContentModel;
using Kontent.Ai.Management.Tests.Base;
using RichardSzalay.MockHttp;
using System.Text.Json;

namespace Kontent.Ai.Management.Tests.Serialization;

// ToJson/FromJson form the snapshot's stable, versioned contract: byte-deterministic output (the committed file must
// diff meaningfully) and self-contained deserialization (no SDK serializer setup required of the consumer).
public class ContentModelSnapshotSerializationTests
{
    private static string Fixture(string folder, string name)
        => File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Data", folder, name));

    private static async Task<ContentModelSnapshot> ExportFromFixturesAsync()
    {
        var (client, mock) = MockClientFactory.Create();
        mock.Expect(HttpMethod.Get, $"{MockClientFactory.BaseUrl}/types").Respond("application/json", Fixture("ContentType", "ContentTypesPage3.json"));
        mock.Expect(HttpMethod.Get, $"{MockClientFactory.BaseUrl}/snippets").Respond("application/json", Fixture("ContentTypeSnippet", "SnippetsPage3.json"));
        mock.Expect(HttpMethod.Get, $"{MockClientFactory.BaseUrl}/taxonomies").Respond("application/json", Fixture("TaxonomyGroup", "TaxonomyGroupsPage2.json"));

        return (await client.ExportContentModelAsync()).EnsureSuccess();
    }

    [Fact]
    public async Task ToJson_FromJson_RoundTripsTheSnapshot()
    {
        var snapshot = await ExportFromFixturesAsync();

        var roundTripped = ContentModelSnapshot.FromJson(snapshot.ToJson());

        roundTripped.Version.Should().Be(ContentModelSnapshot.CurrentVersion);
        roundTripped.Should().BeEquivalentTo(snapshot);
    }

    [Fact]
    public async Task ToJson_IsByteDeterministic()
    {
        var snapshot = await ExportFromFixturesAsync();

        var first = snapshot.ToJson();
        var second = snapshot.ToJson();
        var restabilized = ContentModelSnapshot.FromJson(first).ToJson();

        second.Should().Be(first);
        restabilized.Should().Be(first);
        first.Should().NotContain("\r", "the canonical form pins \\n line endings on every platform");
    }

    [Fact]
    public void FromJson_CommittedSampleFile_DeserializesWithoutAnySerializerSetup()
    {
        // The committed sample locks the on-disk format; it is also the fixture the model generator codes against.
        var json = Fixture("ContentModel", "Snapshot.json").Replace("\r\n", "\n");

        var snapshot = ContentModelSnapshot.FromJson(json);

        snapshot.Version.Should().Be(ContentModelSnapshot.CurrentVersion);
        snapshot.Types.Should().Contain(t => t.Codename == "with_deleted_taxonomy");
        snapshot.ToJson().Should().Be(json, "re-serializing the committed sample must reproduce it byte for byte");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(999)]
    public void FromJson_UnsupportedVersion_Throws(int version)
    {
        var json = $$"""{ "version": {{version}}, "types": [], "snippets": [], "taxonomies": [] }""";

        var act = () => ContentModelSnapshot.FromJson(json);

        act.Should().Throw<JsonException>().WithMessage("*version*");
    }

    [Fact]
    public void FromJson_NullJsonToken_Throws()
    {
        var act = () => ContentModelSnapshot.FromJson("null");

        act.Should().Throw<JsonException>();
    }
}

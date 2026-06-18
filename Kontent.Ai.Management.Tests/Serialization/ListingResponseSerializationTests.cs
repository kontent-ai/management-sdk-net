using AwesomeAssertions;
using Kontent.Ai.Management.Configuration;
using Kontent.Ai.Management.Models.Languages;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Kontent.Ai.Management.Tests.Serialization;

// IListingResponse<T> server models are plain records with explicit [JsonPropertyName]; System.Text.Json
// (de)serializes them natively with no custom converter. These tests guard that the production options keep
// round-tripping the object shape { "<items>": [...], "pagination": {...} }.
public class ListingResponseSerializationTests
{
    private static readonly JsonSerializerOptions Options = RefitSettingsProvider.CreateDefaultJsonSerializerOptions();

    [Fact]
    public void Read_ObjectShape_BindsItemsAndPagination()
    {
        const string json = """
            {
              "languages": [
                {"id":"00000000-0000-0000-0000-000000000000","name":"Default","codename":"default","is_active":true,"is_default":true,"fallback_language":{"id":"00000000-0000-0000-0000-000000000000"}},
                {"id":"0080e2ba-5c66-4067-a80a-5a81658cbe64","name":"German","codename":"de-DE","is_active":true,"is_default":false,"fallback_language":{"id":"00000000-0000-0000-0000-000000000000"}}
              ],
              "pagination": {"continuation_token":"t","next_page":"u"}
            }
            """;

        var listing = JsonSerializer.Deserialize<LanguagesListingResponseServerModel>(json, Options);

        listing!.Languages.Should().HaveCount(2);
        listing.Languages.First().Codename.Should().Be("default");
        listing.Pagination.Token.Should().Be("t");
    }

    [Fact]
    public void Write_EmitsObjectShape_NotBareArray()
    {
        var listing = new LanguagesListingResponseServerModel
        {
            Languages = new[] { new LanguageModel { Id = System.Guid.Empty, Name = "X", Codename = "x", IsActive = true, IsDefault = false, FallbackLanguage = Models.Shared.Reference.ById(System.Guid.Empty) } },
            Pagination = new(),
        };

        var written = JsonSerializer.Serialize(listing, Options);
        var node = JsonNode.Parse(written)!;

        written.Should().StartWith("{"); // object, not array
        node["languages"]!.AsArray().Should().HaveCount(1);
        node["languages"]![0]!["codename"]!.GetValue<string>().Should().Be("x");
    }
}

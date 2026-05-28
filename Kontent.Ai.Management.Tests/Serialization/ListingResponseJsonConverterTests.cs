using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using AwesomeAssertions;
using Kontent.Ai.Management.Configuration;
using Kontent.Ai.Management.Models.Languages;
using Kontent.Ai.Management.Serialization.Converters;

namespace Kontent.Ai.Management.Tests.Serialization;

// Tests the converter mechanism (object-shape read/write of IListingResponse<T> types) using a real
// listing model + snake_case naming policy as the wave-4 [JsonPropertyName] stand-in. Inner
// PaginationResponseModel field binding needs the wave-4 attribute swap, so these tests assert
// dispatch/shape, not deep-field equivalence of pagination internals.
public class ListingResponseJsonConverterTests
{
    private static JsonSerializerOptions SnakeCaseOptions(JsonIgnoreCondition nullHandling = JsonIgnoreCondition.Never) => new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        DefaultIgnoreCondition = nullHandling,
        Converters = { new ListingResponseJsonConverterFactory(), new ReferenceJsonConverter() },
    };

    [Fact]
    public void Read_ObjectShape_DispatchesItemsAndPagination()
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

        var listing = JsonSerializer.Deserialize<LanguagesListingResponseServerModel>(json, SnakeCaseOptions());

        listing!.Languages.Should().HaveCount(2);
        listing.Languages.First().Codename.Should().Be("default");
        listing.Pagination.Should().NotBeNull();
    }

    [Fact]
    public void Write_EmitsObjectShape_NotBareArray()
    {
        var listing = new LanguagesListingResponseServerModel
        {
            Languages = new[] { new LanguageModel { Id = System.Guid.Empty, Name = "X", Codename = "x", IsActive = true, IsDefault = false, FallbackLanguage = Models.Shared.Reference.ById(System.Guid.Empty) } },
            Pagination = new(),
        };

        var written = JsonSerializer.Serialize(listing, SnakeCaseOptions(JsonIgnoreCondition.WhenWritingNull));
        var node = JsonNode.Parse(written)!;

        written.Should().StartWith("{"); // object, not array
        node["languages"]!.AsArray().Should().HaveCount(1);
        node["languages"]![0]!["codename"]!.GetValue<string>().Should().Be("x");
    }

    [Fact]
    public void Factory_CanConvert_OnlyIListingResponseImplementors()
    {
        var factory = new ListingResponseJsonConverterFactory();

        factory.CanConvert(typeof(LanguagesListingResponseServerModel)).Should().BeTrue();
        factory.CanConvert(typeof(LanguageModel)).Should().BeFalse();
        factory.CanConvert(typeof(string)).Should().BeFalse();
    }

    [Fact]
    public void Registered_InRefitSettingsProviderOptions_RoundTripsObjectShape()
    {
        var options = RefitSettingsProvider.CreateDefaultJsonSerializerOptions();

        var listing = new LanguagesListingResponseServerModel { Languages = System.Array.Empty<LanguageModel>(), Pagination = new() };
        var written = JsonSerializer.Serialize(listing, options);

        written.Should().StartWith("{").And.Contain("languages"); // via the model's [JsonPropertyName]
    }
}

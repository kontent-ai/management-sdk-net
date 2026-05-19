using System.Collections.Generic;
using System.IO;
using System.Linq;
using AwesomeAssertions;
using Kontent.Ai.Management.Models.Languages;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Modules.ActionInvoker;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Kontent.Ai.Management.Tests.Serialization;

// Wave-3 pilot go/no-go gate: proves Newtonsoft 13.0.4 round-trips the Languages record archetypes
// byte-identically against real server payloads (request serializer settings, JToken value-equality).
// A non-empty diff here means the incremental route does not hold and the model layer falls back to
// the sibling-style rebuild.
public class LanguageRecordRoundTripTests
{
    private static readonly JsonSerializerSettings RequestSettings = new()
    {
        NullValueHandling = NullValueHandling.Ignore,
        Converters = { new DecimalObjectConverter(), new StringEnumConverter() },
    };

    private static string Fixture(string name)
        => File.ReadAllText(Path.Combine(System.Environment.CurrentDirectory, "Data", "Language", name));

    private static void AssertRoundTripsIdentically<T>(string json)
    {
        var roundTripped = JsonConvert.SerializeObject(JsonConvert.DeserializeObject<T>(json), RequestSettings);

        JToken.DeepEquals(JToken.Parse(roundTripped), JToken.Parse(json))
            .Should().BeTrue($"record round-trip must equal the original payload but was:\n{roundTripped}");
    }

    [Theory]
    [InlineData("SingleLanguageResponse.json")]
    [InlineData("CreateLanguage_CreatesLanguage.json")]
    [InlineData("ModifyLanguages_Replace_ModifiesLanguages.json")]
    public void LanguageModel_RoundTrips(string fixture)
        => AssertRoundTripsIdentically<LanguageModel>(Fixture(fixture));

    [Fact]
    public void LanguageModel_RoundTrips_EveryListedEntry()
    {
        foreach (var entry in JObject.Parse(Fixture("LanguagesPage1.json"))["languages"]!.Children())
        {
            AssertRoundTripsIdentically<LanguageModel>(entry.ToString());
        }
    }

    [Fact]
    public void LanguagesListingResponseServerModel_RoundTrips()
        => AssertRoundTripsIdentically<LanguagesListingResponseServerModel>(Fixture("LanguagesPage1.json"));

    [Fact]
    public void LanguageCreateModel_RoundTrips()
    {
        var model = new LanguageCreateModel
        {
            Name = "German (Germany)",
            Codename = "de-DE",
            ExternalId = "standard-german",
            IsActive = true,
            FallbackLanguage = Reference.ById(System.Guid.Empty),
        };

        var first = JsonConvert.SerializeObject(model, RequestSettings);
        var second = JsonConvert.SerializeObject(JsonConvert.DeserializeObject<LanguageCreateModel>(first), RequestSettings);

        JToken.DeepEquals(JToken.Parse(first), JToken.Parse(second)).Should().BeTrue();
        JObject.Parse(first).Should().BeEquivalentTo(JObject.Parse(
            """{"name":"German (Germany)","codename":"de-DE","external_id":"standard-german","is_active":true,"fallback_language":{"id":"00000000-0000-0000-0000-000000000000"}}"""));
    }

    [Fact]
    public void LanguagePatchModel_RoundTrips()
    {
        var model = new LanguagePatchModel { PropertyName = LanguagePropertyName.Name, Value = "Deutsch" };

        var first = JsonConvert.SerializeObject(model, RequestSettings);
        var second = JsonConvert.SerializeObject(JsonConvert.DeserializeObject<LanguagePatchModel>(first), RequestSettings);

        JToken.DeepEquals(JToken.Parse(first), JToken.Parse(second)).Should().BeTrue();
        JObject.Parse(first)["property_name"]!.Value<string>().Should().Be("name");
        JObject.Parse(first)["value"]!.Value<string>().Should().Be("Deutsch");
    }
}

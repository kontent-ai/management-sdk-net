using AwesomeAssertions;
using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.Items;
using Kontent.Ai.Management.Models.LanguageVariants;
using Kontent.Ai.Management.Tests.Base;
using RichardSzalay.MockHttp;
using System.Net;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

// Composite convenience helpers in ManagementClientExtensions: their value is the orchestration across two API calls
// and the result-projection at each seam, so the tests pin the call sequence (MockHttp throws on any unexpected
// request) and the failure short-circuits rather than re-covering the per-endpoint matrix.
public class ManagementClientExtensionsTests
{
    private const string CreatedItemId = "6b0a7992-e4a3-490f-a159-599846e918e6";

    private static string Fixture(string folder, string name)
        => File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Data", folder, name));

    private static string ItemsUrl => $"{MockClientFactory.BaseUrl}/items";
    private static string CreatedVariantUrl => $"{MockClientFactory.BaseUrl}/items/{CreatedItemId}/variants/codename/en-US";

    private static ContentItemCreateModel ItemToCreate => new() { Name = "On Roasts", Type = Reference.ByCodename("article") };
    private static LanguageVariantUpsertModel VariantToUpsert => new() { Elements = [] };

    [Fact]
    public async Task CreateContentItemWithVariantAsync_Success_CreatesItemThenUpsertsVariant()
    {
        var (client, mock) = MockClientFactory.Create();
        mock.Expect(HttpMethod.Post, ItemsUrl)
            .Respond("application/json", Fixture("ContentItem", "ContentItem.json"));
        mock.Expect(HttpMethod.Put, CreatedVariantUrl)
            .Respond("application/json", Fixture("LanguageVariant", "LanguageVariant.json"));

        var result = await client.CreateContentItemWithVariantAsync(ItemToCreate, Reference.ByCodename("en-US"), VariantToUpsert);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task CreateContentItemWithVariantAsync_ItemCreationFails_ShortCircuitsWithoutVariantUpsert()
    {
        // No PUT expectation is registered: MockHttp throws on any unmatched request, so a variant upsert would fail
        // this test. The failure must carry the item-create call's detail.
        var (client, mock) = MockClientFactory.Create();
        mock.Expect(HttpMethod.Post, ItemsUrl)
            .Respond(HttpStatusCode.BadRequest, "application/json",
                """{ "message": "Invalid type.", "request_id": "r0", "error_code": 50 }""");

        var result = await client.CreateContentItemWithVariantAsync(ItemToCreate, Reference.ByCodename("en-US"), VariantToUpsert);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        result.Error!.ErrorCode.Should().Be(50);
    }

    [Theory]
    [InlineData(true, false, false)]
    [InlineData(false, true, false)]
    [InlineData(false, false, true)]
    public async Task CreateContentItemWithVariantAsync_NullArg_Throws(bool nullItem, bool nullLanguage, bool nullVariant)
    {
        var (client, _) = MockClientFactory.Create();
        var item = nullItem ? null! : ItemToCreate;
        var language = nullLanguage ? null! : Reference.ByCodename("en-US");
        var variant = nullVariant ? null! : VariantToUpsert;

        await client.Invoking(c => c.CreateContentItemWithVariantAsync(item, language, variant))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }
}

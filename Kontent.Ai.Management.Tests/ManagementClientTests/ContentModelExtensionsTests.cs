using AwesomeAssertions;
using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Tests.Base;
using RichardSzalay.MockHttp;
using System.Net;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

// ExportContentModelAsync composes the three listing endpoints, drains each one's pagination, and is all-or-nothing:
// any failed page fails the whole export rather than returning a silently truncated model. MockHttp throws on any
// unexpected request, so the short-circuit tests prove that later listings are not called once one fails.
public class ContentModelExtensionsTests
{
    private static string Fixture(string folder, string name)
        => File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Data", folder, name));

    private static string TypesUrl => $"{MockClientFactory.BaseUrl}/types";
    private static string SnippetsUrl => $"{MockClientFactory.BaseUrl}/snippets";
    private static string TaxonomiesUrl => $"{MockClientFactory.BaseUrl}/taxonomies";

    private const string ErrorBody = """
        { "message": "Insufficient permissions.", "request_id": "req-1", "error_code": 16 }
        """;

    [Fact]
    public async Task ExportContentModelAsync_Success_AggregatesAllThreeSortedByCodename()
    {
        var (client, mock) = MockClientFactory.Create();
        mock.Expect(HttpMethod.Get, TypesUrl).Respond("application/json", Fixture("ContentType", "ContentTypesPage3.json"));
        mock.Expect(HttpMethod.Get, SnippetsUrl).Respond("application/json", Fixture("ContentTypeSnippet", "SnippetsPage3.json"));
        mock.Expect(HttpMethod.Get, TaxonomiesUrl).Respond("application/json", Fixture("TaxonomyGroup", "TaxonomyGroupsPage2.json"));

        var result = await client.ExportContentModelAsync();

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        var model = result.Value;
        model.Types.Should().NotBeEmpty();
        model.Snippets.Should().NotBeEmpty();
        model.Taxonomies.Should().NotBeEmpty();
        model.Types.Select(t => t.Codename).Should().BeInAscendingOrder(StringComparer.Ordinal);
        model.Snippets.Select(s => s.Codename).Should().BeInAscendingOrder(StringComparer.Ordinal);
        model.Taxonomies.Select(t => t.Codename).Should().BeInAscendingOrder(StringComparer.Ordinal);
    }

    [Fact]
    public async Task ExportContentModelAsync_DrainsEveryPageOfAListing()
    {
        // Three pages for /types; the last-page entry 'with_deleted_taxonomy' only appears on page 3, so its presence
        // proves the export paged past page 1 rather than stopping at the first response.
        var (client, mock) = MockClientFactory.Create();
        mock.Expect(HttpMethod.Get, TypesUrl).Respond("application/json", Fixture("ContentType", "ContentTypesPage1.json"));
        mock.Expect(HttpMethod.Get, TypesUrl).Respond("application/json", Fixture("ContentType", "ContentTypesPage2.json"));
        mock.Expect(HttpMethod.Get, TypesUrl).Respond("application/json", Fixture("ContentType", "ContentTypesPage3.json"));
        mock.Expect(HttpMethod.Get, SnippetsUrl).Respond("application/json", Fixture("ContentTypeSnippet", "SnippetsPage3.json"));
        mock.Expect(HttpMethod.Get, TaxonomiesUrl).Respond("application/json", Fixture("TaxonomyGroup", "TaxonomyGroupsPage2.json"));

        var result = await client.ExportContentModelAsync();

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Types.Should().Contain(t => t.Codename == "with_deleted_taxonomy");
    }

    [Fact]
    public async Task ExportContentModelAsync_FirstListingFails_ReturnsFailureWithoutRemainingListings()
    {
        // Only /types is registered: if the export went on to call /snippets or /taxonomies, MockHttp would throw.
        var (client, mock) = MockClientFactory.Create();
        mock.Expect(HttpMethod.Get, TypesUrl).Respond(HttpStatusCode.Forbidden, "application/json", ErrorBody);

        var result = await client.ExportContentModelAsync();

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        result.Error!.ErrorCode.Should().Be(16);
    }

    [Fact]
    public async Task ExportContentModelAsync_PageFailsMidListing_FailsWholeExport()
    {
        // Page 1 of /types succeeds (and carries a continuation token), page 2 fails. A partial model must not be
        // returned — the whole export fails.
        var (client, mock) = MockClientFactory.Create();
        mock.Expect(HttpMethod.Get, TypesUrl).Respond("application/json", Fixture("ContentType", "ContentTypesPage1.json"));
        mock.Expect(HttpMethod.Get, TypesUrl).Respond(HttpStatusCode.InternalServerError, "application/json", ErrorBody);

        var result = await client.ExportContentModelAsync();

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }

    [Fact]
    public async Task ExportContentModelAsync_ClientIsNull_Throws()
    {
        var act = () => ((IManagementClient)null!).ExportContentModelAsync();

        await act.Should().ThrowExactlyAsync<ArgumentNullException>();
    }
}

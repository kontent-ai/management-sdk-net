using System.Collections;
using AwesomeAssertions;
using Kontent.Ai.Management.Models.LanguageVariants;
using Kontent.Ai.Management.Tests.Base;
using RichardSzalay.MockHttp;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class LanguageVariantTests
{
    private static string Fixture(string name)
        => File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Data", "LanguageVariant", name));

    private static List<T> ConcatPages<T>(params string[] pages)
        => pages
            .SelectMany(p => JsonSerializer.Deserialize<List<T>>(JsonNode.Parse(p)!.AsObject().First().Value!.ToString(), SharedTestJsonOptions.Default)!)
            .ToList();

    [Fact]
    public async Task ListLanguageVariantsByItemAsync_ListsVariants()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ById(Guid.Parse("4b628214-e4fe-4fe0-b1ff-955df33e1515"));
        var fixture = Fixture("LanguageVariants.json");
        mock.Expect(HttpMethod.Get, $"{MockClientFactory.BaseUrl}/items/{identifier.Id}/variants")
            .Respond("application/json", fixture);

        var expected = JsonSerializer.Deserialize<List<LanguageVariantModel>>(fixture, SharedTestJsonOptions.Default)!;

        var result = await client.ListLanguageVariantsByItemAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.ShouldEqualAsJson(expected);
    }

    [Fact]
    public async Task ListLanguageVariantsByItemAsync_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.ListLanguageVariantsByItemAsync(null!))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task ListLanguageVariantsByTypeAsync_PagesThroughAllVariants()
    {
        var (client, mock) = MockClientFactory.Create();
        var page1 = Fixture("LanguageVariantsPage1.json");
        var page2 = Fixture("LanguageVariantsPage2.json");
        var page3 = Fixture("LanguageVariantsPage3.json");

        var identifier = Reference.ById(Guid.Parse("17ff8a28-ebe6-5c9d-95ea-18fe1ff86d2d"));
        var url = $"{MockClientFactory.BaseUrl}/types/{identifier.Id}/variants";
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page1);
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page2);
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page3);

        var listResult = await client.ListLanguageVariantsByTypeAsync(identifier);
        listResult.IsSuccess.Should().BeTrue();
        IReadOnlyList<LanguageVariantModel> variants = listResult.Value;

        mock.VerifyNoOutstandingExpectation();
        variants.ShouldEqualAsJson(ConcatPages<LanguageVariantModel>(page1, page2, page3));
    }

    [Fact]
    public async Task ListLanguageVariantsByTypeAsync_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.ListLanguageVariantsByTypeAsync(null!))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task ListLanguageVariantsOfContentTypeWithComponentsAsync_PagesThroughAllVariants()
    {
        var (client, mock) = MockClientFactory.Create();
        var page1 = Fixture("LanguageVariantsPage1.json");
        var page2 = Fixture("LanguageVariantsPage2.json");
        var page3 = Fixture("LanguageVariantsPage3.json");

        var identifier = Reference.ById(Guid.Parse("17ff8a28-ebe6-5c9d-95ea-18fe1ff86d2d"));
        var url = $"{MockClientFactory.BaseUrl}/types/{identifier.Id}/components";
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page1);
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page2);
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page3);

        var listResult = await client.ListLanguageVariantsOfContentTypeWithComponentsAsync(identifier);
        listResult.IsSuccess.Should().BeTrue();
        IReadOnlyList<LanguageVariantModel> variants = listResult.Value;

        mock.VerifyNoOutstandingExpectation();
        variants.ShouldEqualAsJson(ConcatPages<LanguageVariantModel>(page1, page2, page3));
    }

    [Fact]
    public async Task ListLanguageVariantsOfContentTypeWithComponentsAsync_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.ListLanguageVariantsOfContentTypeWithComponentsAsync(null!))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task ListLanguageVariantsByCollectionAsync_PagesThroughAllVariants()
    {
        var (client, mock) = MockClientFactory.Create();
        var page1 = Fixture("LanguageVariantsPage1.json");
        var page2 = Fixture("LanguageVariantsPage2.json");
        var page3 = Fixture("LanguageVariantsPage3.json");

        var identifier = Reference.ById(Guid.Parse("17ff8a28-ebe6-5c9d-95ea-18fe1ff86d2d"));
        var url = $"{MockClientFactory.BaseUrl}/collections/{identifier.Id}/variants";
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page1);
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page2);
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page3);

        var listResult = await client.ListLanguageVariantsByCollectionAsync(identifier);
        listResult.IsSuccess.Should().BeTrue();
        IReadOnlyList<LanguageVariantModel> variants = listResult.Value;

        mock.VerifyNoOutstandingExpectation();
        variants.ShouldEqualAsJson(ConcatPages<LanguageVariantModel>(page1, page2, page3));
    }

    [Fact]
    public async Task ListLanguageVariantsByCollectionAsync_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.ListLanguageVariantsByCollectionAsync(null!))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task ListLanguageVariantsBySpaceAsync_PagesThroughAllVariants()
    {
        var (client, mock) = MockClientFactory.Create();
        var page1 = Fixture("LanguageVariantsPage1.json");
        var page2 = Fixture("LanguageVariantsPage2.json");
        var page3 = Fixture("LanguageVariantsPage3.json");

        var identifier = Reference.ById(Guid.Parse("f81647c8-778a-4b20-a47e-d09dc8541151"));
        var url = $"{MockClientFactory.BaseUrl}/spaces/{identifier.Id}/variants";
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page1);
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page2);
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page3);

        var listResult = await client.ListLanguageVariantsBySpaceAsync(identifier);
        listResult.IsSuccess.Should().BeTrue();
        IReadOnlyList<LanguageVariantModel> variants = listResult.Value;

        mock.VerifyNoOutstandingExpectation();
        variants.ShouldEqualAsJson(ConcatPages<LanguageVariantModel>(page1, page2, page3));
    }

    [Fact]
    public async Task ListLanguageVariantsBySpaceAsync_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.ListLanguageVariantsBySpaceAsync(null!))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Theory]
    [ClassData(typeof(CombinationOfIdentifiersAndUrl))]
    public async Task GetLanguageVariantAsync_GetsVariant(LanguageVariantIdentifier identifier, string expectedUrl)
    {
        var (client, mock) = MockClientFactory.Create();
        var fixture = Fixture("LanguageVariant.json");
        mock.Expect(HttpMethod.Get, expectedUrl)
            .Respond("application/json", fixture);

        var expected = JsonSerializer.Deserialize<LanguageVariantModel>(fixture, SharedTestJsonOptions.Default)!;

        var result = await client.GetLanguageVariantAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.ShouldEqualAsJson(expected);
    }

    [Fact]
    public async Task GetLanguageVariantAsync_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.GetLanguageVariantAsync(null!))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Theory]
    [ClassData(typeof(CombinationOfIdentifiersAndUrl))]
    public async Task GetPublishedLanguageVariantAsync_GetsVariant(LanguageVariantIdentifier identifier, string expectedUrl)
    {
        var (client, mock) = MockClientFactory.Create();
        var fixture = Fixture("LanguageVariant.json");
        mock.Expect(HttpMethod.Get, expectedUrl + "/published")
            .Respond("application/json", fixture);

        var expected = JsonSerializer.Deserialize<LanguageVariantModel>(fixture, SharedTestJsonOptions.Default)!;

        var result = await client.GetPublishedLanguageVariantAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.ShouldEqualAsJson(expected);
    }

    [Fact]
    public async Task GetPublishedLanguageVariantAsync_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.GetPublishedLanguageVariantAsync(null!))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Theory]
    [ClassData(typeof(CombinationOfIdentifiersAndUrl))]
    public async Task UpsertLanguageVariantAsync_ByLanguageVariantUpsertModel_UpsertsVariant(LanguageVariantIdentifier identifier, string expectedUrl)
    {
        var (client, mock) = MockClientFactory.Create();
        var fixture = Fixture("LanguageVariant.json");
        var expected = JsonSerializer.Deserialize<LanguageVariantModel>(fixture, SharedTestJsonOptions.Default)!;
        var upsertModel = new LanguageVariantUpsertModel { Elements = expected.Elements };

        string? capturedBody = null;
        mock.Expect(HttpMethod.Put, expectedUrl)
            .With(r =>
            {
                capturedBody = r.Content!.ReadAsStringAsync().GetAwaiter().GetResult();
                return true;
            })
            .Respond("application/json", fixture);

        var result = await client.UpsertLanguageVariantAsync(identifier, upsertModel);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.ShouldEqualAsJson(expected);
        capturedBody.Should().NotBeNull();
        JsonSerializer.Deserialize<LanguageVariantUpsertModel>(capturedBody!, SharedTestJsonOptions.Default)
            .ShouldEqualAsJson(JsonSerializer.Deserialize<LanguageVariantUpsertModel>(JsonSerializer.Serialize(upsertModel, SharedTestJsonOptions.Default), SharedTestJsonOptions.Default)!);
    }

    [Fact]
    public async Task UpsertLanguageVariantAsync_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.UpsertLanguageVariantAsync(null!, new LanguageVariantUpsertModel { Elements = [] }))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Theory]
    [ClassData(typeof(CombinationOfIdentifiers))]
    public async Task UpsertLanguageVariantAsync_LanguageVariantUpsertModelIsNull_Throws(LanguageVariantIdentifier identifier)
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.UpsertLanguageVariantAsync(identifier, (LanguageVariantUpsertModel)null!))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Theory]
    [ClassData(typeof(CombinationOfIdentifiersAndUrl))]
    public async Task UpsertLanguageVariantAsync_ByLanguageVariantModel_UpsertsVariant(LanguageVariantIdentifier identifier, string expectedUrl)
    {
        var (client, mock) = MockClientFactory.Create();
        var fixture = Fixture("LanguageVariant.json");
        var expected = JsonSerializer.Deserialize<LanguageVariantModel>(fixture, SharedTestJsonOptions.Default)!;

        string? capturedBody = null;
        mock.Expect(HttpMethod.Put, expectedUrl)
            .With(r =>
            {
                capturedBody = r.Content!.ReadAsStringAsync().GetAwaiter().GetResult();
                return true;
            })
            .Respond("application/json", fixture);

        var result = await client.UpsertLanguageVariantAsync(identifier, expected);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.ShouldEqualAsJson(expected);
        capturedBody.Should().NotBeNull();
        var sentModel = new LanguageVariantUpsertModel(expected);
        JsonSerializer.Deserialize<LanguageVariantUpsertModel>(capturedBody!, SharedTestJsonOptions.Default)
            .ShouldEqualAsJson(JsonSerializer.Deserialize<LanguageVariantUpsertModel>(JsonSerializer.Serialize(sentModel, SharedTestJsonOptions.Default), SharedTestJsonOptions.Default)!);
    }

    [Fact]
    public async Task UpsertLanguageVariantAsync_ByLanguageVariantModel_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();
        var model = JsonSerializer.Deserialize<LanguageVariantModel>(Fixture("LanguageVariant.json"), SharedTestJsonOptions.Default)!;

        await client.Invoking(x => x.UpsertLanguageVariantAsync(null!, model))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Theory]
    [ClassData(typeof(CombinationOfIdentifiers))]
    public async Task UpsertLanguageVariantAsync_ByLanguageVariantModel_LanguageVariantModelIsNull_Throws(LanguageVariantIdentifier identifier)
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.UpsertLanguageVariantAsync(identifier, (LanguageVariantModel)null!))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Theory]
    [ClassData(typeof(CombinationOfIdentifiersAndUrl))]
    public async Task DeleteLanguageVariantAsync_DeletesVariant(LanguageVariantIdentifier identifier, string expectedUrl)
    {
        var (client, mock) = MockClientFactory.Create();
        mock.Expect(HttpMethod.Delete, expectedUrl)
            .Respond(System.Net.HttpStatusCode.OK);

        var result = await client.DeleteLanguageVariantAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
    }

    private class CombinationOfIdentifiersAndUrl : IEnumerable<object[]>
    {
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<object[]> GetEnumerator()
        {
            foreach (var (Identifier, Url) in GetPermutation())
            {
                yield return new object[] { Identifier, Url };
            }
        }

        public IEnumerable<(LanguageVariantIdentifier Identifier, string Url)> GetPermutation()
        {
            var itemsIdentifiers = new[] { ById, ByCodename, ByExternalId };
            var languageIdentifiers = new[] { ById, ByCodename };

            foreach (var item in itemsIdentifiers)
            {
                foreach (var language in languageIdentifiers)
                {
                    var identifier = new LanguageVariantIdentifier(item.Identifier, language.Identifier);
                    var url = $"{MockClientFactory.BaseUrl}/items/{item.UrlSegment}/variants/{language.UrlSegment}";
                    yield return (identifier, url);
                }
            }
        }

        static protected (Reference Identifier, string UrlSegment) ById => (Reference.ById(Guid.Parse("4b628214-e4fe-4fe0-b1ff-955df33e1515")), "4b628214-e4fe-4fe0-b1ff-955df33e1515");
        static protected (Reference Identifier, string UrlSegment) ByCodename => (Reference.ByCodename("codename"), "codename/codename");
        static protected (Reference Identifier, string UrlSegment) ByExternalId => (Reference.ByExternalId("external-id"), "external-id/external-id");
    }

    private class CombinationOfIdentifiers : CombinationOfIdentifiersAndUrl, IEnumerable<object[]>
    {
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public new IEnumerator<object[]> GetEnumerator()
        {
            foreach (var (Identifier, Url) in GetPermutation())
            {
                yield return new object[] { Identifier };
            }
        }
    }
}

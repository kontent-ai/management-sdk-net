using System.Collections;
using AwesomeAssertions;
using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.LanguageVariants;
using Kontent.Ai.Management.Models.Publishing;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.StronglyTyped;
using Kontent.Ai.Management.Models.Workflow;
using Kontent.Ai.Management.Modules.ModelBuilders;
using Kontent.Ai.Management.Tests.Base;
using Kontent.Ai.Management.Tests.Data;
using RichardSzalay.MockHttp;
using Xunit;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class LanguageVariantTests
{
    private static readonly IElementModelProvider _elementModelProvider = new ElementModelProvider();

    private static string Fixture(string name)
        => File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Data", "LanguageVariant", name));

    private static List<T> ConcatPages<T>(params string[] pages)
        => pages
            .SelectMany(p => JsonSerializer.Deserialize<List<T>>(JsonNode.Parse(p)!.AsObject().First().Value!.ToString(), SharedTestJsonOptions.Default)!)
            .ToList();

    private static LanguageVariantModel<T> MapToStronglyTyped<T>(LanguageVariantModel variant) where T : new()
        => new()
        {
            Item = variant.Item,
            Language = variant.Language,
            LastModified = variant.LastModified,
            Schedule = variant.Schedule,
            Workflow = variant.Workflow,
            DueDate = variant.DueDate,
            Note = variant.Note,
            Contributors = variant.Contributors,
            Elements = _elementModelProvider.GetStronglyTypedElements<T>(variant.Elements),
        };

    private static LanguageVariantModel<T> DeserializeStronglyTyped<T>(string json) where T : new()
        => MapToStronglyTyped<T>(JsonSerializer.Deserialize<LanguageVariantModel>(json, SharedTestJsonOptions.Default)!);

    private static List<LanguageVariantModel<T>> DeserializeStronglyTypedList<T>(string json) where T : new()
        => JsonSerializer.Deserialize<List<LanguageVariantModel>>(json, SharedTestJsonOptions.Default)!
            .Select(MapToStronglyTyped<T>)
            .ToList();

    private static List<LanguageVariantModel<T>> ConcatStronglyTypedPages<T>(params string[] pages) where T : new()
        => pages
            .SelectMany(p => JsonSerializer.Deserialize<List<LanguageVariantModel>>(JsonNode.Parse(p)!.AsObject().First().Value!.ToString(), SharedTestJsonOptions.Default)!)
            .Select(MapToStronglyTyped<T>)
            .ToList();

    [Fact]
    public async Task ListLanguageVariantsByItemAsync_StronglyTyped_ListsVariants()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ById(Guid.Parse("4b628214-e4fe-4fe0-b1ff-955df33e1515"));
        var fixture = Fixture("LanguageVariants.json");
        mock.Expect(HttpMethod.Get, $"{MockClientFactory.BaseUrl}/items/{identifier.Id}/variants")
            .Respond("application/json", fixture);

        var expected = DeserializeStronglyTypedList<ComplexTestModel>(fixture);

        var response = await client.ListLanguageVariantsByItemAsync<ComplexTestModel>(identifier);

        mock.VerifyNoOutstandingExpectation();
        response.ShouldEqualAsJson(expected);
    }

    [Fact]
    public async Task ListLanguageVariantsByItemAsync_StronglyTyped_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.ListLanguageVariantsByItemAsync<ComplexTestModel>(null!))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task ListLanguageVariantsByItemAsync_DynamicallyTyped_ListsVariants()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ById(Guid.Parse("4b628214-e4fe-4fe0-b1ff-955df33e1515"));
        var fixture = Fixture("LanguageVariants.json");
        mock.Expect(HttpMethod.Get, $"{MockClientFactory.BaseUrl}/items/{identifier.Id}/variants")
            .Respond("application/json", fixture);

        var expected = JsonSerializer.Deserialize<List<LanguageVariantModel>>(fixture, SharedTestJsonOptions.Default)!;

        var response = await client.ListLanguageVariantsByItemAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
        response.ShouldEqualAsJson(expected);
    }

    [Fact]
    public async Task ListLanguageVariantsByItemAsync_DynamicallyTyped_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.ListLanguageVariantsByItemAsync(null!))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task ListLanguageVariantsByTypeAsync_StronglyTyped_ListsVariants()
    {
        var (client, mock) = MockClientFactory.Create();
        var page1 = Fixture("LanguageVariantsPage1.json");
        var page2 = Fixture("LanguageVariantsPage2.json");
        var page3 = Fixture("LanguageVariantsPage3.json");

        var expected = ConcatStronglyTypedPages<ComplexTestModel>(page1, page2, page3);

        var identifier = Reference.ById(Guid.Parse("17ff8a28-ebe6-5c9d-95ea-18fe1ff86d2d"));
        var url = $"{MockClientFactory.BaseUrl}/types/{identifier.Id}/variants";
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page1);
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page2);
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page3);

        var response = await client.ListLanguageVariantsByTypeAsync<ComplexTestModel>(identifier).GetAllAsync();

        mock.VerifyNoOutstandingExpectation();
        response.ShouldEqualAsJson(expected);
    }

    [Fact]
    public async Task ListLanguageVariantsByTypeAsync_StronglyTyped_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.ListLanguageVariantsByTypeAsync<ComplexTestModel>(null!))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task ListLanguageVariantsByTypeAsync_DynamicallyTyped_ListsVariants()
    {
        var (client, mock) = MockClientFactory.Create();
        var page1 = Fixture("LanguageVariantsPage1.json");
        var page2 = Fixture("LanguageVariantsPage2.json");
        var page3 = Fixture("LanguageVariantsPage3.json");

        var expected = ConcatPages<LanguageVariantModel>(page1, page2, page3);

        var identifier = Reference.ById(Guid.Parse("17ff8a28-ebe6-5c9d-95ea-18fe1ff86d2d"));
        var url = $"{MockClientFactory.BaseUrl}/types/{identifier.Id}/variants";
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page1);
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page2);
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page3);

        var response = await client.ListLanguageVariantsByTypeAsync(identifier).GetAllAsync();

        mock.VerifyNoOutstandingExpectation();
        response.ShouldEqualAsJson(expected);
    }

    [Fact]
    public async Task ListLanguageVariantsByTypeAsync_DynamicallyTyped_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.ListLanguageVariantsByTypeAsync(null!))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task ListLanguageVariantsOfContentTypeWithComponentsAsync_DynamicallyTyped_ListsVariants()
    {
        var (client, mock) = MockClientFactory.Create();
        var page1 = Fixture("LanguageVariantsPage1.json");
        var page2 = Fixture("LanguageVariantsPage2.json");
        var page3 = Fixture("LanguageVariantsPage3.json");

        var expected = ConcatPages<LanguageVariantModel>(page1, page2, page3);

        var identifier = Reference.ById(Guid.Parse("17ff8a28-ebe6-5c9d-95ea-18fe1ff86d2d"));
        var url = $"{MockClientFactory.BaseUrl}/types/{identifier.Id}/components";
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page1);
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page2);
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page3);

        var response = await client.ListLanguageVariantsOfContentTypeWithComponentsAsync(identifier).GetAllAsync();

        mock.VerifyNoOutstandingExpectation();
        response.ShouldEqualAsJson(expected);
    }

    [Fact]
    public async Task ListLanguageVariantsOfContentTypeWithComponentsAsync_DynamicallyTyped_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.ListLanguageVariantsOfContentTypeWithComponentsAsync(null!))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task ListLanguageVariantsByCollectionAsync_DynamicallyTyped_ListsVariants()
    {
        var (client, mock) = MockClientFactory.Create();
        var page1 = Fixture("LanguageVariantsPage1.json");
        var page2 = Fixture("LanguageVariantsPage2.json");
        var page3 = Fixture("LanguageVariantsPage3.json");

        var expected = ConcatPages<LanguageVariantModel>(page1, page2, page3);

        var identifier = Reference.ById(Guid.Parse("17ff8a28-ebe6-5c9d-95ea-18fe1ff86d2d"));
        var url = $"{MockClientFactory.BaseUrl}/collections/{identifier.Id}/variants";
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page1);
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page2);
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page3);

        var response = await client.ListLanguageVariantsByCollectionAsync(identifier).GetAllAsync();

        mock.VerifyNoOutstandingExpectation();
        response.ShouldEqualAsJson(expected);
    }

    [Fact]
    public async Task ListLanguageVariantsByCollectionAsync_DynamicallyTyped_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.ListLanguageVariantsByCollectionAsync(null!))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task ListLanguageVariantsBySpaceAsync_DynamicallyTyped_ListsVariants()
    {
        var (client, mock) = MockClientFactory.Create();
        var page1 = Fixture("LanguageVariantsPage1.json");
        var page2 = Fixture("LanguageVariantsPage2.json");
        var page3 = Fixture("LanguageVariantsPage3.json");

        var expected = ConcatPages<LanguageVariantModel>(page1, page2, page3);

        var identifier = Reference.ById(Guid.Parse("f81647c8-778a-4b20-a47e-d09dc8541151"));
        var url = $"{MockClientFactory.BaseUrl}/spaces/{identifier.Id}/variants";
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page1);
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page2);
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page3);

        var response = await client.ListLanguageVariantsBySpaceAsync(identifier).GetAllAsync();

        mock.VerifyNoOutstandingExpectation();
        response.ShouldEqualAsJson(expected);
    }

    [Fact]
    public async Task ListLanguageVariantsBySpaceAsync_DynamicallyTyped_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.ListLanguageVariantsBySpaceAsync(null!))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Theory]
    [ClassData(typeof(CombinationOfIdentifiersAndUrl))]
    public async Task GetLanguageVariantAsync_DynamicallyTyped_GetsVariant(LanguageVariantIdentifier identifier, string expectedUrl)
    {
        var (client, mock) = MockClientFactory.Create();
        var fixture = Fixture("LanguageVariant.json");
        mock.Expect(HttpMethod.Get, expectedUrl)
            .Respond("application/json", fixture);

        var expected = JsonSerializer.Deserialize<LanguageVariantModel>(fixture, SharedTestJsonOptions.Default)!;

        var response = await client.GetLanguageVariantAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
        response.ShouldEqualAsJson(expected);
    }

    [Fact]
    public async Task GetLanguageVariantAsync_DynamicallyTyped_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.GetLanguageVariantAsync(null!))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Theory]
    [ClassData(typeof(CombinationOfIdentifiersAndUrl))]
    public async Task GetPublishedLanguageVariantAsync_StronglyTyped_GetsVariant(LanguageVariantIdentifier identifier, string expectedUrl)
    {
        var (client, mock) = MockClientFactory.Create();
        var fixture = Fixture("LanguageVariant.json");
        mock.Expect(HttpMethod.Get, expectedUrl + "/published")
            .Respond("application/json", fixture);

        var expected = DeserializeStronglyTyped<ComplexTestModel>(fixture);

        var response = await client.GetPublishedLanguageVariantAsync<ComplexTestModel>(identifier);

        mock.VerifyNoOutstandingExpectation();
        response.ShouldEqualAsJson(expected);
    }

    [Fact]
    public async Task GetPublishedLanguageVariantAsync_StronglyTyped_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.GetPublishedLanguageVariantAsync<ComplexTestModel>(null!))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Theory]
    [ClassData(typeof(CombinationOfIdentifiersAndUrl))]
    public async Task GetPublishedLanguageVariantAsync_DynamicallyTyped_GetsVariant(LanguageVariantIdentifier identifier, string expectedUrl)
    {
        var (client, mock) = MockClientFactory.Create();
        var fixture = Fixture("LanguageVariant.json");
        mock.Expect(HttpMethod.Get, expectedUrl + "/published")
            .Respond("application/json", fixture);

        var expected = JsonSerializer.Deserialize<LanguageVariantModel>(fixture, SharedTestJsonOptions.Default)!;

        var response = await client.GetPublishedLanguageVariantAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
        response.ShouldEqualAsJson(expected);
    }

    [Fact]
    public async Task GetPublishedLanguageVariantAsync_DynamicallyTyped_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.GetPublishedLanguageVariantAsync(null!))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Theory]
    [ClassData(typeof(CombinationOfIdentifiersAndUrl))]
    public async Task UpsertLanguageVariantAsync_DynamicallyTyped_ByLanguageVariantUpsertModel_UpsertsVariant(LanguageVariantIdentifier identifier, string expectedUrl)
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

        var response = await client.UpsertLanguageVariantAsync(identifier, upsertModel);

        mock.VerifyNoOutstandingExpectation();
        response.ShouldEqualAsJson(expected);
        capturedBody.Should().NotBeNull();
        JsonSerializer.Deserialize<LanguageVariantUpsertModel>(capturedBody!, SharedTestJsonOptions.Default)
            .ShouldEqualAsJson(JsonSerializer.Deserialize<LanguageVariantUpsertModel>(JsonSerializer.Serialize(upsertModel, SharedTestJsonOptions.Default), SharedTestJsonOptions.Default)!);
    }

    [Fact]
    public async Task UpsertLanguageVariantAsync_DynamicallyTyped_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.UpsertLanguageVariantAsync(null!, new LanguageVariantUpsertModel()))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Theory]
    [ClassData(typeof(CombinationOfIdentifiers))]
    public async Task UpsertLanguageVariantAsync_DynamicallyTyped_LanguageVariantUpsertModelIsNull_Throws(LanguageVariantIdentifier identifier)
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.UpsertLanguageVariantAsync(identifier, (LanguageVariantUpsertModel)null!))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Theory]
    [ClassData(typeof(CombinationOfIdentifiersAndUrl))]
    public async Task UpsertLanguageVariantAsync_DynamicallyTyped_ByLanguageVariantModel_UpsertsVariant(LanguageVariantIdentifier identifier, string expectedUrl)
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

        var response = await client.UpsertLanguageVariantAsync(identifier, expected);

        mock.VerifyNoOutstandingExpectation();
        response.ShouldEqualAsJson(expected);
        capturedBody.Should().NotBeNull();
        var sentModel = new LanguageVariantUpsertModel(expected);
        JsonSerializer.Deserialize<LanguageVariantUpsertModel>(capturedBody!, SharedTestJsonOptions.Default)
            .ShouldEqualAsJson(JsonSerializer.Deserialize<LanguageVariantUpsertModel>(JsonSerializer.Serialize(sentModel, SharedTestJsonOptions.Default), SharedTestJsonOptions.Default)!);
    }

    [Fact]
    public async Task UpsertLanguageVariantAsync_DynamicallyTyped_ByLanguageVariantModel_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.UpsertLanguageVariantAsync(null!, new LanguageVariantModel()))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Theory]
    [ClassData(typeof(CombinationOfIdentifiers))]
    public async Task UpsertLanguageVariantAsync_DynamicallyTyped_ByLanguageVariantModel_LanguageVariantModelIsNull_Throws(LanguageVariantIdentifier identifier)
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

        await client.DeleteLanguageVariantAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
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

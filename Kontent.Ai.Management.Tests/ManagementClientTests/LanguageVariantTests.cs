using System.Collections;
using FluentAssertions;
using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.LanguageVariants;
using Kontent.Ai.Management.Models.Publishing;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.StronglyTyped;
using Kontent.Ai.Management.Models.Workflow;
using Kontent.Ai.Management.Tests.Base;
using Kontent.Ai.Management.Tests.Data;
using Newtonsoft.Json;
using RichardSzalay.MockHttp;
using Xunit;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class LanguageVariantTests
{
    private static string Fixture(string name)
        => File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Data", "LanguageVariant", name));

    [Fact]
    public async Task ListLanguageVariantsByItemAsync_StronglyTyped_ListsVariants()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ById(Guid.Parse("4b628214-e4fe-4fe0-b1ff-955df33e1515"));
        mock.Expect(HttpMethod.Get, $"{MockClientFactory.BaseUrl}/items/{identifier.Id}/variants")
            .Respond("application/json", Fixture("LanguageVariants.json"));

        var response = await client.ListLanguageVariantsByItemAsync<ComplexTestModel>(identifier);

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(GetExpectedComplexTestModels("00000000-0000-0000-0000-000000000000", "10000000-0000-0000-0000-000000000000"));
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
        mock.Expect(HttpMethod.Get, $"{MockClientFactory.BaseUrl}/items/{identifier.Id}/variants")
            .Respond("application/json", Fixture("LanguageVariants.json"));

        var response = await client.ListLanguageVariantsByItemAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(GetExpectedLanguageVariantModels("00000000-0000-0000-0000-000000000000", "10000000-0000-0000-0000-000000000000"));
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

        var expected = new[]
        {
            (itemId: "00000000-0000-0000-0000-000000000000", languageId: "00000000-0000-0000-0000-000000000000"),
            (itemId: "00000000-0000-0000-0000-000000000000", languageId: "10000000-0000-0000-0000-000000000000"),
            (itemId: "10000000-0000-0000-0000-000000000000", languageId: "00000000-0000-0000-0000-000000000000"),
            (itemId: "10000000-0000-0000-0000-000000000000", languageId: "10000000-0000-0000-0000-000000000000"),
            (itemId: "20000000-0000-0000-0000-000000000000", languageId: "00000000-0000-0000-0000-000000000000"),
            (itemId: "20000000-0000-0000-0000-000000000000", languageId: "10000000-0000-0000-0000-000000000000")
        }.Select(x => GetExpectedComplexTestModel(x.languageId, x.itemId));

        var identifier = Reference.ById(Guid.Parse("17ff8a28-ebe6-5c9d-95ea-18fe1ff86d2d"));
        var url = $"{MockClientFactory.BaseUrl}/types/{identifier.Id}/variants";
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page1);
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page2);
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page3);

        var response = await client.ListLanguageVariantsByTypeAsync<ComplexTestModel>(identifier).GetAllAsync();

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(expected);
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

        var expected = new[]
        {
            (itemId: "00000000-0000-0000-0000-000000000000", languageId: "00000000-0000-0000-0000-000000000000"),
            (itemId: "00000000-0000-0000-0000-000000000000", languageId: "10000000-0000-0000-0000-000000000000"),
            (itemId: "10000000-0000-0000-0000-000000000000", languageId: "00000000-0000-0000-0000-000000000000"),
            (itemId: "10000000-0000-0000-0000-000000000000", languageId: "10000000-0000-0000-0000-000000000000"),
            (itemId: "20000000-0000-0000-0000-000000000000", languageId: "00000000-0000-0000-0000-000000000000"),
            (itemId: "20000000-0000-0000-0000-000000000000", languageId: "10000000-0000-0000-0000-000000000000")
        }.Select(x => GetExpectedLanguageVariantModel(x.languageId, x.itemId));

        var identifier = Reference.ById(Guid.Parse("17ff8a28-ebe6-5c9d-95ea-18fe1ff86d2d"));
        var url = $"{MockClientFactory.BaseUrl}/types/{identifier.Id}/variants";
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page1);
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page2);
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page3);

        var response = await client.ListLanguageVariantsByTypeAsync(identifier).GetAllAsync();

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(expected);
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

        var expected = new[]
        {
            (itemId: "00000000-0000-0000-0000-000000000000", languageId: "00000000-0000-0000-0000-000000000000"),
            (itemId: "00000000-0000-0000-0000-000000000000", languageId: "10000000-0000-0000-0000-000000000000"),
            (itemId: "10000000-0000-0000-0000-000000000000", languageId: "00000000-0000-0000-0000-000000000000"),
            (itemId: "10000000-0000-0000-0000-000000000000", languageId: "10000000-0000-0000-0000-000000000000"),
            (itemId: "20000000-0000-0000-0000-000000000000", languageId: "00000000-0000-0000-0000-000000000000"),
            (itemId: "20000000-0000-0000-0000-000000000000", languageId: "10000000-0000-0000-0000-000000000000")
        }.Select(x => GetExpectedLanguageVariantModel(x.languageId, x.itemId));

        var identifier = Reference.ById(Guid.Parse("17ff8a28-ebe6-5c9d-95ea-18fe1ff86d2d"));
        var url = $"{MockClientFactory.BaseUrl}/types/{identifier.Id}/components";
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page1);
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page2);
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page3);

        var response = await client.ListLanguageVariantsOfContentTypeWithComponentsAsync(identifier).GetAllAsync();

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(expected);
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

        var expected = new[]
        {
            (itemId: "00000000-0000-0000-0000-000000000000", languageId: "00000000-0000-0000-0000-000000000000"),
            (itemId: "00000000-0000-0000-0000-000000000000", languageId: "10000000-0000-0000-0000-000000000000"),
            (itemId: "10000000-0000-0000-0000-000000000000", languageId: "00000000-0000-0000-0000-000000000000"),
            (itemId: "10000000-0000-0000-0000-000000000000", languageId: "10000000-0000-0000-0000-000000000000"),
            (itemId: "20000000-0000-0000-0000-000000000000", languageId: "00000000-0000-0000-0000-000000000000"),
            (itemId: "20000000-0000-0000-0000-000000000000", languageId: "10000000-0000-0000-0000-000000000000")
        }.Select(x => GetExpectedLanguageVariantModel(x.languageId, x.itemId));

        var identifier = Reference.ById(Guid.Parse("17ff8a28-ebe6-5c9d-95ea-18fe1ff86d2d"));
        var url = $"{MockClientFactory.BaseUrl}/collections/{identifier.Id}/variants";
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page1);
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page2);
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page3);

        var response = await client.ListLanguageVariantsByCollectionAsync(identifier).GetAllAsync();

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(expected);
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

        var expected = new[]
        {
            (itemId: "00000000-0000-0000-0000-000000000000", languageId: "00000000-0000-0000-0000-000000000000"),
            (itemId: "00000000-0000-0000-0000-000000000000", languageId: "10000000-0000-0000-0000-000000000000"),
            (itemId: "10000000-0000-0000-0000-000000000000", languageId: "00000000-0000-0000-0000-000000000000"),
            (itemId: "10000000-0000-0000-0000-000000000000", languageId: "10000000-0000-0000-0000-000000000000"),
            (itemId: "20000000-0000-0000-0000-000000000000", languageId: "00000000-0000-0000-0000-000000000000"),
            (itemId: "20000000-0000-0000-0000-000000000000", languageId: "10000000-0000-0000-0000-000000000000")
        }.Select(x => GetExpectedLanguageVariantModel(x.languageId, x.itemId));

        var identifier = Reference.ById(Guid.Parse("f81647c8-778a-4b20-a47e-d09dc8541151"));
        var url = $"{MockClientFactory.BaseUrl}/spaces/{identifier.Id}/variants";
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page1);
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page2);
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page3);

        var response = await client.ListLanguageVariantsBySpaceAsync(identifier).GetAllAsync();

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(expected);
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
        mock.Expect(HttpMethod.Get, expectedUrl)
            .Respond("application/json", Fixture("LanguageVariant.json"));

        var response = await client.GetLanguageVariantAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(GetExpectedLanguageVariantModel());
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
        mock.Expect(HttpMethod.Get, expectedUrl + "/published")
            .Respond("application/json", Fixture("LanguageVariant.json"));

        var response = await client.GetPublishedLanguageVariantAsync<ComplexTestModel>(identifier);

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(GetExpectedComplexTestModel());
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
        mock.Expect(HttpMethod.Get, expectedUrl + "/published")
            .Respond("application/json", Fixture("LanguageVariant.json"));

        var response = await client.GetPublishedLanguageVariantAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(GetExpectedLanguageVariantModel());
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
        var expected = GetExpectedLanguageVariantModel();
        var upsertModel = new LanguageVariantUpsertModel { Elements = expected.Elements };

        string? capturedBody = null;
        mock.Expect(HttpMethod.Put, expectedUrl)
            .With(r =>
            {
                capturedBody = r.Content!.ReadAsStringAsync().GetAwaiter().GetResult();
                return true;
            })
            .Respond("application/json", Fixture("LanguageVariant.json"));

        var response = await client.UpsertLanguageVariantAsync(identifier, upsertModel);

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(expected);
        capturedBody.Should().NotBeNull();
        JsonConvert.DeserializeObject<LanguageVariantUpsertModel>(capturedBody!)
            .Should().BeEquivalentTo(JsonConvert.DeserializeObject<LanguageVariantUpsertModel>(JsonConvert.SerializeObject(upsertModel)));
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
        var expected = GetExpectedLanguageVariantModel();

        string? capturedBody = null;
        mock.Expect(HttpMethod.Put, expectedUrl)
            .With(r =>
            {
                capturedBody = r.Content!.ReadAsStringAsync().GetAwaiter().GetResult();
                return true;
            })
            .Respond("application/json", Fixture("LanguageVariant.json"));

        var response = await client.UpsertLanguageVariantAsync(identifier, expected);

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(expected);
        capturedBody.Should().NotBeNull();
        var sentModel = new LanguageVariantUpsertModel(expected);
        JsonConvert.DeserializeObject<LanguageVariantUpsertModel>(capturedBody!)
            .Should().BeEquivalentTo(JsonConvert.DeserializeObject<LanguageVariantUpsertModel>(JsonConvert.SerializeObject(sentModel)));
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

    private static List<LanguageVariantModel> GetExpectedLanguageVariantModels(params string[] languageIds)
        => languageIds.Select(x => GetExpectedLanguageVariantModel(x)).ToList();

    private static LanguageVariantModel GetExpectedLanguageVariantModel(
        string languageId = "78dbefe8-831b-457e-9352-f4c4eacd5024",
        string itemId = "4b628214-e4fe-4fe0-b1ff-955df33e1515") => new()
        {
            Item = Reference.ById(Guid.Parse(itemId)),
            Language = Reference.ById(Guid.Parse(languageId)),
            LastModified = DateTimeOffset.Parse("2021-11-06T13:57:26.7069564Z").UtcDateTime,
            Workflow = new WorkflowStepIdentifier(Reference.ById(Guid.Parse("00000000-0000-0000-0000-000000000000")), Reference.ById(Guid.Parse("eee6db3b-545a-4785-8e86-e3772c8756f9"))),
            Schedule = GetExpectedScheduleResponseModel(),
            DueDate = GetExpectedDueDateModel(),
            Contributors = GetExpectedContributors(),
            Note = "Just a note",
            Elements = ElementsData.GetExpectedDynamicElements(),
        };

    private static List<LanguageVariantModel<ComplexTestModel>> GetExpectedComplexTestModels(params string[] languageIds)
        => languageIds.Select(x => GetExpectedComplexTestModel(x)).ToList();

    private static LanguageVariantModel<ComplexTestModel> GetExpectedComplexTestModel(
        string languageId = "78dbefe8-831b-457e-9352-f4c4eacd5024",
        string itemId = "4b628214-e4fe-4fe0-b1ff-955df33e1515") => new()
        {
            Item = Reference.ById(Guid.Parse(itemId)),
            Language = Reference.ById(Guid.Parse(languageId)),
            LastModified = DateTimeOffset.Parse("2021-11-06T13:57:26.7069564Z").UtcDateTime,
            Workflow = new WorkflowStepIdentifier(Reference.ById(Guid.Parse("00000000-0000-0000-0000-000000000000")), Reference.ById(Guid.Parse("eee6db3b-545a-4785-8e86-e3772c8756f9"))),
            Schedule = GetExpectedScheduleResponseModel(),
            DueDate = GetExpectedDueDateModel(),
            Contributors = GetExpectedContributors(),
            Note = "Just a note",
            Elements = ElementsData.GetExpectedStronglyTypedElementsModel(),
        };

    private static ScheduleResponseModel GetExpectedScheduleResponseModel() => new()
    {
        PublishTime = DateTimeOffset.Parse("2024-03-31T08:00:00Z").UtcDateTime,
        PublishDisplayTimeZone = "Europe/Prague",
        UnpublishTime = DateTimeOffset.Parse("2024-04-30T08:00:00Z").UtcDateTime,
        UnpublishDisplayTimeZone = "Europe/Prague"
    };

    private static DueDateModel GetExpectedDueDateModel() =>
        new() { Value = DateTimeOffset.Parse("2092-01-07T06:04:00.7069564Z").UtcDateTime };

    private static List<UserIdentifier> GetExpectedContributors() =>
        new() { UserIdentifier.ById("4b628214-e4fe-4fe0-b1ff-955df33e1515") };

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

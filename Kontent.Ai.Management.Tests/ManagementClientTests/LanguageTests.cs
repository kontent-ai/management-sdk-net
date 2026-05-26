using AwesomeAssertions;
using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.Languages;
using Kontent.Ai.Management.Tests.Base;
using RichardSzalay.MockHttp;
using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class LanguageTests
{
    private static string SingleLanguage => Fixture("SingleLanguageResponse.json");
    private static string CreateLanguage => Fixture("CreateLanguage_CreatesLanguage.json");
    private static string ModifyLanguages => Fixture("ModifyLanguages_Replace_ModifiesLanguages.json");

    private static string Fixture(string name)
        => File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Data", "Language", name));

    private static List<T> ConcatPages<T>(params string[] pages)
        => pages
            .SelectMany(p => JsonSerializer.Deserialize<List<T>>(JsonNode.Parse(p)!.AsObject().First().Value!.ToString(), SharedTestJsonOptions.Default)!)
            .ToList();

    [Fact]
    public async Task EnumerateLanguagePagesAsync_PagesThroughAllLanguages()
    {
        var (client, mock) = MockClientFactory.Create();
        var page1 = Fixture("LanguagesPage1.json");
        var page2 = Fixture("LanguagesPage2.json");
        var page3 = Fixture("LanguagesPage3.json");
        var url = $"{MockClientFactory.BaseUrl}/languages";
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page1);
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page2);
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page3);

        var languages = new List<LanguageModel>();
        await foreach (var page in client.EnumerateLanguagePagesAsync())
        {
            page.IsSuccess.Should().BeTrue();
            languages.AddRange(page.Value);
        }

        mock.VerifyNoOutstandingExpectation();
        languages.Should().BeEquivalentTo(ConcatPages<LanguageModel>(page1, page2, page3));
    }

    [Fact]
    public async Task EnumerateLanguagePagesAsync_PageFails_YieldsFailureThenStops()
    {
        var (client, mock) = MockClientFactory.Create();
        var url = $"{MockClientFactory.BaseUrl}/languages";
        mock.Expect(HttpMethod.Get, url).Respond("application/json", Fixture("LanguagesPage1.json"));
        mock.Expect(HttpMethod.Get, url).Respond(HttpStatusCode.InternalServerError, "application/json", """{ "message": "Server error." }""");

        var pages = new List<IManagementResult<IReadOnlyList<LanguageModel>>>();
        await foreach (var page in client.EnumerateLanguagePagesAsync())
        {
            pages.Add(page);
        }

        mock.VerifyNoOutstandingExpectation();
        pages.Should().HaveCount(2);
        pages[0].IsSuccess.Should().BeTrue();
        pages[1].IsSuccess.Should().BeFalse();
        pages[1].StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }

    [Fact]
    public async Task EnumerateLanguagePagesAsync_Items_FlattensAllPages()
    {
        var (client, mock) = MockClientFactory.Create();
        var page1 = Fixture("LanguagesPage1.json");
        var page2 = Fixture("LanguagesPage2.json");
        var page3 = Fixture("LanguagesPage3.json");
        var url = $"{MockClientFactory.BaseUrl}/languages";
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page1);
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page2);
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page3);

        var languages = new List<LanguageModel>();
        await foreach (var language in client.EnumerateLanguagePagesAsync().Items())
        {
            languages.Add(language);
        }

        mock.VerifyNoOutstandingExpectation();
        languages.Should().BeEquivalentTo(ConcatPages<LanguageModel>(page1, page2, page3));
    }

    [Fact]
    public async Task EnumerateLanguagePagesAsync_Items_PageFails_Throws()
    {
        var (client, mock) = MockClientFactory.Create();
        var url = $"{MockClientFactory.BaseUrl}/languages";
        mock.Expect(HttpMethod.Get, url).Respond("application/json", Fixture("LanguagesPage1.json"));
        mock.Expect(HttpMethod.Get, url).Respond(HttpStatusCode.InternalServerError, "application/json", """{ "message": "Server error." }""");

        var enumerate = async () =>
        {
            await foreach (var _ in client.EnumerateLanguagePagesAsync().Items())
            {
            }
        };

        await enumerate.Should().ThrowAsync<ManagementResultException>();
    }

    [Fact]
    public async Task GetLanguageAsync_ById_GetsLanguage()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ById(Guid.NewGuid());
        mock.Expect(HttpMethod.Get, $"{MockClientFactory.BaseUrl}/languages/{identifier.Id}")
            .Respond("application/json", SingleLanguage);

        var result = await client.GetLanguageAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<LanguageModel>(SingleLanguage, SharedTestJsonOptions.Default));
    }

    [Fact]
    public async Task GetLanguageAsync_ByCodename_GetsLanguage()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ByCodename("mycodename");
        mock.Expect(HttpMethod.Get, $"{MockClientFactory.BaseUrl}/languages/codename/{identifier.Codename}")
            .Respond("application/json", SingleLanguage);

        var result = await client.GetLanguageAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<LanguageModel>(SingleLanguage, SharedTestJsonOptions.Default));
    }

    [Fact]
    public async Task GetLanguageAsync_ByExternalId_GetsLanguage()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ByExternalId("externalId");
        mock.Expect(HttpMethod.Get, $"{MockClientFactory.BaseUrl}/languages/external-id/{identifier.ExternalId}")
            .Respond("application/json", SingleLanguage);

        var result = await client.GetLanguageAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<LanguageModel>(SingleLanguage, SharedTestJsonOptions.Default));
    }

    [Fact]
    public async Task GetLanguageAsync_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.GetLanguageAsync(null!)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task CreateLanguageAsync_CreatesLanguage()
    {
        var (client, mock) = MockClientFactory.Create();
        var createModel = new LanguageCreateModel
        {
            Name = "German (Germany)",
            Codename = "de-DE",
            IsActive = false,
            ExternalId = "standard-german",
            FallbackLanguage = Reference.ById(Guid.Parse("00000000-0000-0000-0000-000000000000"))
        };

        string? capturedBody = null;
        mock.Expect(HttpMethod.Post, $"{MockClientFactory.BaseUrl}/languages")
            .With(r =>
            {
                capturedBody = r.Content!.ReadAsStringAsync().GetAwaiter().GetResult();
                return true;
            })
            .Respond("application/json", CreateLanguage);

        var result = await client.CreateLanguageAsync(createModel);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<LanguageModel>(CreateLanguage, SharedTestJsonOptions.Default));
        capturedBody.Should().NotBeNull();
        JsonSerializer.Deserialize<LanguageCreateModel>(capturedBody!, SharedTestJsonOptions.Default)
            .Should().BeEquivalentTo(JsonSerializer.Deserialize<LanguageCreateModel>(JsonSerializer.Serialize(createModel, SharedTestJsonOptions.Default), SharedTestJsonOptions.Default));
    }

    [Fact]
    public async Task CreateLanguageAsync_CreateModelIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.CreateLanguageAsync(null!)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task ModifyLanguagesAsync_ById_ModifiesLanguages()
    {
        var (client, mock) = MockClientFactory.Create();
        var changes = GetChanges();
        var identifier = Reference.ById(Guid.NewGuid());

        string? capturedBody = null;
        mock.Expect(new HttpMethod("PATCH"), $"{MockClientFactory.BaseUrl}/languages/{identifier.Id}")
            .With(r =>
            {
                capturedBody = r.Content!.ReadAsStringAsync().GetAwaiter().GetResult();
                return true;
            })
            .Respond("application/json", ModifyLanguages);

        var result = await client.ModifyLanguageAsync(identifier, changes);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<LanguageModel>(ModifyLanguages, SharedTestJsonOptions.Default));
        capturedBody.Should().NotBeNull();
        JsonSerializer.Deserialize<LanguagePatchModel[]>(capturedBody!, SharedTestJsonOptions.Default)!
            .ShouldEqualAsJson(JsonSerializer.Deserialize<LanguagePatchModel[]>(JsonSerializer.Serialize(changes, SharedTestJsonOptions.Default), SharedTestJsonOptions.Default)!);
    }

    [Fact]
    public async Task ModifyLanguagesAsync_ByCodename_ModifiesLanguages()
    {
        var (client, mock) = MockClientFactory.Create();
        var changes = GetChanges();
        var identifier = Reference.ByCodename("code");

        string? capturedBody = null;
        mock.Expect(new HttpMethod("PATCH"), $"{MockClientFactory.BaseUrl}/languages/codename/{identifier.Codename}")
            .With(r =>
            {
                capturedBody = r.Content!.ReadAsStringAsync().GetAwaiter().GetResult();
                return true;
            })
            .Respond("application/json", ModifyLanguages);

        var result = await client.ModifyLanguageAsync(identifier, changes);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<LanguageModel>(ModifyLanguages, SharedTestJsonOptions.Default));
        capturedBody.Should().NotBeNull();
        JsonSerializer.Deserialize<LanguagePatchModel[]>(capturedBody!, SharedTestJsonOptions.Default)!
            .ShouldEqualAsJson(JsonSerializer.Deserialize<LanguagePatchModel[]>(JsonSerializer.Serialize(changes, SharedTestJsonOptions.Default), SharedTestJsonOptions.Default)!);
    }

    [Fact]
    public async Task ModifyLanguagesAsync_ByExternalId_ModifiesLanguages()
    {
        var (client, mock) = MockClientFactory.Create();
        var changes = GetChanges();
        var identifier = Reference.ByExternalId("externalId");

        string? capturedBody = null;
        mock.Expect(new HttpMethod("PATCH"), $"{MockClientFactory.BaseUrl}/languages/external-id/{identifier.ExternalId}")
            .With(r =>
            {
                capturedBody = r.Content!.ReadAsStringAsync().GetAwaiter().GetResult();
                return true;
            })
            .Respond("application/json", ModifyLanguages);

        var result = await client.ModifyLanguageAsync(identifier, changes);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<LanguageModel>(ModifyLanguages, SharedTestJsonOptions.Default));
        capturedBody.Should().NotBeNull();
        JsonSerializer.Deserialize<LanguagePatchModel[]>(capturedBody!, SharedTestJsonOptions.Default)!
            .ShouldEqualAsJson(JsonSerializer.Deserialize<LanguagePatchModel[]>(JsonSerializer.Serialize(changes, SharedTestJsonOptions.Default), SharedTestJsonOptions.Default)!);
    }

    [Fact]
    public async Task ModifyLanguages_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.ModifyLanguageAsync(null!, GetChanges())).Should().ThrowAsync<ArgumentNullException>();
    }

    private static LanguagePatchModel[] GetChanges() => new[]
    {
        new LanguagePatchModel
        {
            PropertyName = LanguagePropertyName.FallbackLanguage,
            Value = Reference.ByCodename("en-US")
        },
        new LanguagePatchModel
        {
            PropertyName = LanguagePropertyName.Name,
            Value = "Deutsch"
        },
        new LanguagePatchModel
        {
            PropertyName= LanguagePropertyName.Codename,
            Value = "de-DE"
        },
        new LanguagePatchModel
        {
            PropertyName = LanguagePropertyName.IsActive,
            Value = false
        }
    };
}

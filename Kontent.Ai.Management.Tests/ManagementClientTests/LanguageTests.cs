using AwesomeAssertions;
using Kontent.Ai.Management.Models.Languages;
using Kontent.Ai.Management.Tests.Base;
using RichardSzalay.MockHttp;
using System.Net;
using System.Text.Json;

using static Kontent.Ai.Management.Tests.Base.PagedFixtures;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class LanguageTests
{
    private static string SingleLanguage => Fixture("SingleLanguageResponse.json");
    private static string CreateLanguage => Fixture("CreateLanguage_CreatesLanguage.json");
    private static string ModifyLanguages => Fixture("ModifyLanguages_Replace_ModifiesLanguages.json");

    private static string Fixture(string name)
        => File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Data", "Language", name));

    [Fact]
    public async Task ListLanguagesAsync_ReturnsAllLanguagesAcrossPages()
    {
        // Paging is internal: every page is drained and merged into one result.
        var (client, mock) = MockClientFactory.Create();
        var page1 = Fixture("LanguagesPage1.json");
        var page2 = Fixture("LanguagesPage2.json");
        var page3 = Fixture("LanguagesPage3.json");
        var url = $"{MockClientFactory.BaseUrl}/languages";
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page1);
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page2);
        mock.Expect(HttpMethod.Get, url).Respond("application/json", page3);

        var result = await client.ListLanguagesAsync();

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(ConcatPages<LanguageModel>(page1, page2, page3));
    }

    [Fact]
    public async Task ListLanguagesAsync_PageFails_ReturnsFailureWithoutPartialList()
    {
        // A failed page short-circuits the whole listing to a failed result — all-or-nothing, no partial set.
        var (client, mock) = MockClientFactory.Create();
        var url = $"{MockClientFactory.BaseUrl}/languages";
        mock.Expect(HttpMethod.Get, url).Respond("application/json", Fixture("LanguagesPage1.json"));
        mock.Expect(HttpMethod.Get, url).Respond(HttpStatusCode.InternalServerError, "application/json", """{ "message": "Server error." }""");

        var result = await client.ListLanguagesAsync();

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        result.Value.Should().BeNull();
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

        mock.Expect(HttpMethod.Post, $"{MockClientFactory.BaseUrl}/languages")
            .CaptureBody(out var capturedBody)
            .Respond("application/json", CreateLanguage);

        var result = await client.CreateLanguageAsync(createModel);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<LanguageModel>(CreateLanguage, SharedTestJsonOptions.Default));
        capturedBody.ShouldMatchSerialized(createModel);
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

        mock.Expect(new HttpMethod("PATCH"), $"{MockClientFactory.BaseUrl}/languages/{identifier.Id}")
            .CaptureBody(out var capturedBody)
            .Respond("application/json", ModifyLanguages);

        var result = await client.ModifyLanguageAsync(identifier, changes);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<LanguageModel>(ModifyLanguages, SharedTestJsonOptions.Default));
        capturedBody.Value.Should().NotBeNull();
        JsonSerializer.Deserialize<LanguagePatchModel[]>(capturedBody.Value!, SharedTestJsonOptions.Default)!
            .ShouldEqualAsJson(JsonSerializer.Deserialize<LanguagePatchModel[]>(JsonSerializer.Serialize(changes, SharedTestJsonOptions.Default), SharedTestJsonOptions.Default)!);
    }

    [Fact]
    public async Task ModifyLanguagesAsync_ByCodename_ModifiesLanguages()
    {
        var (client, mock) = MockClientFactory.Create();
        var changes = GetChanges();
        var identifier = Reference.ByCodename("code");

        mock.Expect(new HttpMethod("PATCH"), $"{MockClientFactory.BaseUrl}/languages/codename/{identifier.Codename}")
            .CaptureBody(out var capturedBody)
            .Respond("application/json", ModifyLanguages);

        var result = await client.ModifyLanguageAsync(identifier, changes);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<LanguageModel>(ModifyLanguages, SharedTestJsonOptions.Default));
        capturedBody.Value.Should().NotBeNull();
        JsonSerializer.Deserialize<LanguagePatchModel[]>(capturedBody.Value!, SharedTestJsonOptions.Default)!
            .ShouldEqualAsJson(JsonSerializer.Deserialize<LanguagePatchModel[]>(JsonSerializer.Serialize(changes, SharedTestJsonOptions.Default), SharedTestJsonOptions.Default)!);
    }

    [Fact]
    public async Task ModifyLanguagesAsync_ByExternalId_ModifiesLanguages()
    {
        var (client, mock) = MockClientFactory.Create();
        var changes = GetChanges();
        var identifier = Reference.ByExternalId("externalId");

        mock.Expect(new HttpMethod("PATCH"), $"{MockClientFactory.BaseUrl}/languages/external-id/{identifier.ExternalId}")
            .CaptureBody(out var capturedBody)
            .Respond("application/json", ModifyLanguages);

        var result = await client.ModifyLanguageAsync(identifier, changes);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<LanguageModel>(ModifyLanguages, SharedTestJsonOptions.Default));
        capturedBody.Value.Should().NotBeNull();
        JsonSerializer.Deserialize<LanguagePatchModel[]>(capturedBody.Value!, SharedTestJsonOptions.Default)!
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

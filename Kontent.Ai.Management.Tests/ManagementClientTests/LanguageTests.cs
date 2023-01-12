using FluentAssertions;
using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.Languages;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Tests.Base;
using System;
using System.Net.Http;
using Xunit;
using static Kontent.Ai.Management.Tests.Base.Scenario;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class LanguageTests : IClassFixture<FileSystemFixture>
{
    private readonly Scenario _scenario;

    public LanguageTests()
    {
        _scenario = new Scenario(folder: "Language");
    }

    [Fact]
    public async void ListLanguagesAsync_ListsLanguages()
    {
        var client = _scenario
            .WithResponses("LanguagesPage1.json", "LanguagesPage2.json", "LanguagesPage3.json")
            .CreateManagementClient();

        var response = await client.ListLanguagesAsync().GetAllAsync();

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Get)
            .ListingResponse(response)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/languages")
            .Validate();
    }

    [Fact]
    public async void GetLanguageAsync_ById_GetsLanguage()
    {
        var client = _scenario
        .WithResponses("SingleLanguageResponse.json")
        .CreateManagementClient();

        var identifier = Reference.ById(Guid.NewGuid());
        var response = await client.GetLanguageAsync(identifier);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Get)
            .Response(response)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/languages/{identifier.Id}")
            .Validate();
    }

    [Fact]
    public async void GetLanguageAsync_ByCodename_GetsLanguage()
    {
        var client = _scenario
        .WithResponses("SingleLanguageResponse.json")
        .CreateManagementClient();

        var identifier = Reference.ByCodename("mycodename");
        var response = await client.GetLanguageAsync(identifier);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Get)
            .Response(response)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/languages/codename/{identifier.Codename}")
            .Validate();
    }

    [Fact]
    public async void GetLanguageAsync_ByExternalId_GetsLanguage()
    {
        var client = _scenario
        .WithResponses("SingleLanguageResponse.json")
        .CreateManagementClient();

        var identifier = Reference.ByExternalId("externalId");
        var response = await client.GetLanguageAsync(identifier);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Get)
            .Response(response)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/languages/external-id/{identifier.ExternalId}")
            .Validate();
    }

    [Fact]
    public async void GetLanguageAsync_IdentifierIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.GetLanguageAsync(null)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async void CreateLanguageAsync_CreatesLanguage()
    {
        var client = _scenario
            .WithResponses("CreateLanguage_CreatesLanguage.json")
            .CreateManagementClient();

        var expected = _scenario.GetExpectedResponse<LanguageModel>();

        var createModel = new LanguageCreateModel
        {
            Name = "German (Germany)",
            Codename = "de-DE",
            IsActive = false,
            ExternalId = "standard-german",
            FallbackLanguage = Reference.ById(Guid.Parse("00000000-0000-0000-0000-000000000000"))
        };

        var response = await client.CreateLanguageAsync(createModel);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Post)
            .RequestPayload(createModel)
            .Response(response)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/languages")
            .Validate();
    }

    [Fact]
    public async void CreateLanguageAsync_CreateModelIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.CreateLanguageAsync(null)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async void ModifyLanguagesAsync_ById_ModifiesLanguages()
    {
        var client = _scenario
            .WithResponses("ModifyLanguages_Replace_ModifiesLanguages.json")
            .CreateManagementClient();
        var changes = GetChanges();

        var identifier = Reference.ById(Guid.NewGuid());
        var response = await client.ModifyLanguageAsync(identifier, changes);

        _scenario
            .CreateExpectations()
            .HttpMethod(new HttpMethod("PATCH"))
            .RequestPayload(changes)
            .Response(response)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/languages/{identifier.Id}")
            .Validate();
    }

    [Fact]
    public async void ModifyLanguagesAsync_ByCodename_ModifiesLanguages()
    {
        var client = _scenario
            .WithResponses("ModifyLanguages_Replace_ModifiesLanguages.json")
            .CreateManagementClient();
        var changes = GetChanges();

        var identifier = Reference.ByCodename("code");
        var response = await client.ModifyLanguageAsync(identifier, changes);

        _scenario
            .CreateExpectations()
            .HttpMethod(new HttpMethod("PATCH"))
            .RequestPayload(changes)
            .Response(response)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/languages/codename/{identifier.Codename}")
            .Validate();
    }

    [Fact]
    public async void ModifyLanguagesAsync_ByExternalId_ModifiesLanguages()
    {
        var client = _scenario
            .WithResponses("ModifyLanguages_Replace_ModifiesLanguages.json")
            .CreateManagementClient();
        var changes = GetChanges();

        var identifier = Reference.ByExternalId("externalId");
        var response = await client.ModifyLanguageAsync(identifier, changes);

        _scenario
            .CreateExpectations()
            .HttpMethod(new HttpMethod("PATCH"))
            .RequestPayload(changes)
            .Response(response)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/languages/external-id/{identifier.ExternalId}")
            .Validate();
    }

    [Fact]
    public async void ModifyLanguages_IdentifierIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.ModifyLanguageAsync(null, GetChanges())).Should().ThrowAsync<ArgumentNullException>();
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

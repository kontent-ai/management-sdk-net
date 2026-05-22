using AwesomeAssertions;
using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.CustomApps;
using Kontent.Ai.Management.Models.CustomApps.Patch;
using Kontent.Ai.Management.Tests.Base;
using RichardSzalay.MockHttp;
using Xunit;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class CustomAppTests
{
    private static string CustomAppBaseUrl => $"{MockClientFactory.BaseUrl}/custom-apps";

    private static string CustomApp => Fixture("CustomApp.json");
    private static string ModifyAddInto => Fixture("ModifyCustomApp_AddInto_ModifiesCustomApp.json");
    private static string ModifyRemove => Fixture("ModifyCustomApp_Remove_ModifiesCustomApp.json");
    private static string ModifyReplace => Fixture("ModifyCustomApp_Replace_ModifiesCustomApp.json");

    private static string Fixture(string name)
        => File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Data", "CustomApp", name));

    private static List<T> ConcatPages<T>(params string[] pages)
        => pages
            .SelectMany(p => JsonSerializer.Deserialize<List<T>>(JsonNode.Parse(p)!.AsObject().First().Value!.ToString(), SharedTestJsonOptions.Default)!)
            .ToList();

    [Fact]
    public async Task EnumerateCustomAppPagesAsync_PagesThroughAllCustomApps()
    {
        var (client, mock) = MockClientFactory.Create();
        var page1 = Fixture("CustomAppsPage1.json");
        var page2 = Fixture("CustomAppsPage2.json");
        var page3 = Fixture("CustomAppsPage3.json");
        mock.Expect(HttpMethod.Get, CustomAppBaseUrl).Respond("application/json", page1);
        mock.Expect(HttpMethod.Get, CustomAppBaseUrl).Respond("application/json", page2);
        mock.Expect(HttpMethod.Get, CustomAppBaseUrl).Respond("application/json", page3);

        var customApps = new List<CustomAppModel>();
        await foreach (var page in client.EnumerateCustomAppPagesAsync())
        {
            page.IsSuccess.Should().BeTrue();
            customApps.AddRange(page.Value);
        }

        mock.VerifyNoOutstandingExpectation();
        customApps.Should().BeEquivalentTo(ConcatPages<CustomAppModel>(page1, page2, page3));
    }

    [Fact]
    public async Task CreateCustomApp_ModelIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.CreateCustomAppAsync(null!)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task CrateCustomApp_CreatesCustomApp()
    {
        var (client, mock) = MockClientFactory.Create();
        var expected = JsonSerializer.Deserialize<CustomAppModel>(CustomApp, SharedTestJsonOptions.Default)!;
        var createModel = new CustomAppCreateModel
        {
            Name = expected.Name,
            Codename = expected.Codename,
            SourceUrl = expected.SourceUrl,
            Config = expected.Config,
            AllowedRoles = expected.AllowedRoles
        };

        string? capturedBody = null;
        mock.Expect(HttpMethod.Post, CustomAppBaseUrl)
            .With(r =>
            {
                capturedBody = r.Content!.ReadAsStringAsync().GetAwaiter().GetResult();
                return true;
            })
            .Respond("application/json", CustomApp);

        var result = await client.CreateCustomAppAsync(createModel);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<CustomAppModel>(CustomApp, SharedTestJsonOptions.Default));
        capturedBody.Should().NotBeNull();
        JsonSerializer.Deserialize<CustomAppCreateModel>(capturedBody!, SharedTestJsonOptions.Default)
            .Should().BeEquivalentTo(JsonSerializer.Deserialize<CustomAppCreateModel>(JsonSerializer.Serialize(createModel, SharedTestJsonOptions.Default), SharedTestJsonOptions.Default));
    }

    [Fact]
    public async Task GetCustomApp_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.GetCustomAppAsync(null!)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task GetCustomApp_ById_GetsCustomApp()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ById(Guid.NewGuid());
        mock.Expect(HttpMethod.Get, CustomAppBaseUrl + $"/{identifier.Id}")
            .Respond("application/json", CustomApp);

        var result = await client.GetCustomAppAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<CustomAppModel>(CustomApp, SharedTestJsonOptions.Default));
    }

    [Fact]
    public async Task GetCustomApp_ByCodename_GetsCustomApp()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ByCodename("custom_app");
        mock.Expect(HttpMethod.Get, CustomAppBaseUrl + $"/codename/{identifier.Codename}")
            .Respond("application/json", CustomApp);

        var result = await client.GetCustomAppAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<CustomAppModel>(CustomApp, SharedTestJsonOptions.Default));
    }

    [Fact]
    public async Task ModifyCustomApp_AddInto_ModifiesCustomApp()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ById(Guid.NewGuid());
        var changes = new CustomAppAddIntoPatchModel[]
        {
            new()
            {
                PropertyName = PropertyName.AllowedRoles,
                Value = new[]
                {
                    Reference.ByCodename("new_allowed_role_codename")
                }
            }
        };

        string? capturedBody = null;
        mock.Expect(HttpMethod.Patch, CustomAppBaseUrl + $"/{identifier.Id}")
            .With(r =>
            {
                capturedBody = r.Content!.ReadAsStringAsync().GetAwaiter().GetResult();
                return true;
            })
            .Respond("application/json", ModifyAddInto);

        var result = await client.ModifyCustomAppAsync(identifier, changes);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<CustomAppModel>(ModifyAddInto, SharedTestJsonOptions.Default));
        capturedBody.Should().NotBeNull();
        JsonSerializer.Deserialize<CustomAppAddIntoPatchModel[]>(capturedBody!, SharedTestJsonOptions.Default)!
            .ShouldEqualAsJson(JsonSerializer.Deserialize<CustomAppAddIntoPatchModel[]>(JsonSerializer.Serialize(changes, SharedTestJsonOptions.Default), SharedTestJsonOptions.Default)!);
    }

    [Fact]
    public async Task ModifyCustomApp_Remove_ModifiesCustomApp()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ById(Guid.NewGuid());
        var changes = new CustomAppRemovePatchModel[]
        {
            new()
            {
                PropertyName = PropertyName.AllowedRoles,
                Value = new[]
                {
                    Reference.ByCodename("allowed_role_codename")
                }
            }
        };

        string? capturedBody = null;
        mock.Expect(HttpMethod.Patch, CustomAppBaseUrl + $"/{identifier.Id}")
            .With(r =>
            {
                capturedBody = r.Content!.ReadAsStringAsync().GetAwaiter().GetResult();
                return true;
            })
            .Respond("application/json", ModifyRemove);

        var result = await client.ModifyCustomAppAsync(identifier, changes);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<CustomAppModel>(ModifyRemove, SharedTestJsonOptions.Default));
        capturedBody.Should().NotBeNull();
        JsonSerializer.Deserialize<CustomAppRemovePatchModel[]>(capturedBody!, SharedTestJsonOptions.Default)!
            .ShouldEqualAsJson(JsonSerializer.Deserialize<CustomAppRemovePatchModel[]>(JsonSerializer.Serialize(changes, SharedTestJsonOptions.Default), SharedTestJsonOptions.Default)!);
    }

    [Fact]
    public async Task ModifyCustomApp_Replace_ModifiesCustomApp()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ById(Guid.NewGuid());
        var changes = new CustomAppReplacePatchModel[]
        {
            new() { PropertyName = PropertyName.Name, Value = "New Custom App Name" },
            new() { PropertyName = PropertyName.Codename, Value = "new_custom_app_codename" },
            new() { PropertyName = PropertyName.SourceUrl, Value = "https://newcustomapplication.net" },
            new() { PropertyName = PropertyName.Config, Value = "{ \"enabled\": \"False\" }" },
            new()
            {
                PropertyName = PropertyName.AllowedRoles,
                Value = new[]
                {
                    Reference.ByCodename("allowed_role_codename"),
                    Reference.ById(new Guid("f8f0b5cb-f5b7-42e8-af85-fbdab3ddfacf"))
                }
            }
        };

        string? capturedBody = null;
        mock.Expect(HttpMethod.Patch, CustomAppBaseUrl + $"/{identifier.Id}")
            .With(r =>
            {
                capturedBody = r.Content!.ReadAsStringAsync().GetAwaiter().GetResult();
                return true;
            })
            .Respond("application/json", ModifyReplace);

        var result = await client.ModifyCustomAppAsync(identifier, changes);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<CustomAppModel>(ModifyReplace, SharedTestJsonOptions.Default));
        capturedBody.Should().NotBeNull();
        JsonSerializer.Deserialize<CustomAppReplacePatchModel[]>(capturedBody!, SharedTestJsonOptions.Default)!
            .ShouldEqualAsJson(JsonSerializer.Deserialize<CustomAppReplacePatchModel[]>(JsonSerializer.Serialize(changes, SharedTestJsonOptions.Default), SharedTestJsonOptions.Default)!);
    }

    [Fact]
    public async Task ModifyCustomApp_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();
        var changes = new CustomAppReplacePatchModel[]
        {
            new() { PropertyName = PropertyName.Name, Value = "New space name" }
        };

        await client.Invoking(x => x.ModifyCustomAppAsync(null!, changes)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task ModifyCustomApp_ChangesAreNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();
        var identifier = Reference.ById(Guid.NewGuid());

        await client.Invoking(x => x.ModifyCustomAppAsync(identifier, null!)).Should()
            .ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task DeleteCustomApp_ById_DeletesCustomApp()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ById(Guid.NewGuid());
        mock.Expect(HttpMethod.Delete, CustomAppBaseUrl + $"/{identifier.Id}")
            .Respond(System.Net.HttpStatusCode.OK);

        var result = await client.DeleteCustomAppAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteCustomApp_ByCodename_DeletesCustomApp()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ByCodename("custom_app");
        mock.Expect(HttpMethod.Delete, CustomAppBaseUrl + $"/codename/{identifier.Codename}")
            .Respond(System.Net.HttpStatusCode.OK);

        var result = await client.DeleteCustomAppAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteCustomApp_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.DeleteCustomAppAsync(null!)).Should().ThrowAsync<ArgumentNullException>();
    }
}

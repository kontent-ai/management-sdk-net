using FluentAssertions;
using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.CustomApps;
using Kontent.Ai.Management.Models.CustomApps.Patch;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Tests.Base;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RichardSzalay.MockHttp;
using Xunit;

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
            .SelectMany(p => JsonConvert.DeserializeObject<List<T>>(JObject.Parse(p).Properties().First().Value.ToString())!)
            .ToList();

    [Fact]
    public async Task ListCustomAppsAsync_WithContinuation_ListsContentTypes()
    {
        var (client, mock) = MockClientFactory.Create();
        var page1 = Fixture("CustomAppsPage1.json");
        var page2 = Fixture("CustomAppsPage2.json");
        var page3 = Fixture("CustomAppsPage3.json");
        mock.Expect(HttpMethod.Get, CustomAppBaseUrl).Respond("application/json", page1);
        mock.Expect(HttpMethod.Get, CustomAppBaseUrl).Respond("application/json", page2);
        mock.Expect(HttpMethod.Get, CustomAppBaseUrl).Respond("application/json", page3);

        var response = await client.ListCustomAppsAsync().GetAllAsync();

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(ConcatPages<CustomAppModel>(page1, page2, page3));
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
        var expected = JsonConvert.DeserializeObject<CustomAppModel>(CustomApp)!;
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

        var response = await client.CreateCustomAppAsync(createModel);

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(JsonConvert.DeserializeObject<CustomAppModel>(CustomApp));
        capturedBody.Should().NotBeNull();
        JsonConvert.DeserializeObject<CustomAppCreateModel>(capturedBody!)
            .Should().BeEquivalentTo(JsonConvert.DeserializeObject<CustomAppCreateModel>(JsonConvert.SerializeObject(createModel)));
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

        var response = await client.GetCustomAppAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(JsonConvert.DeserializeObject<CustomAppModel>(CustomApp));
    }

    [Fact]
    public async Task GetCustomApp_ByCodename_GetsCustomApp()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ByCodename("custom_app");
        mock.Expect(HttpMethod.Get, CustomAppBaseUrl + $"/codename/{identifier.Codename}")
            .Respond("application/json", CustomApp);

        var response = await client.GetCustomAppAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(JsonConvert.DeserializeObject<CustomAppModel>(CustomApp));
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

        var response = await client.ModifyCustomAppAsync(identifier, changes);

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(JsonConvert.DeserializeObject<CustomAppModel>(ModifyAddInto));
        capturedBody.Should().NotBeNull();
        JsonConvert.DeserializeObject<CustomAppAddIntoPatchModel[]>(capturedBody!)
            .Should().BeEquivalentTo(JsonConvert.DeserializeObject<CustomAppAddIntoPatchModel[]>(JsonConvert.SerializeObject(changes)), opt => opt.WithStrictOrdering());
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

        var response = await client.ModifyCustomAppAsync(identifier, changes);

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(JsonConvert.DeserializeObject<CustomAppModel>(ModifyRemove));
        capturedBody.Should().NotBeNull();
        JsonConvert.DeserializeObject<CustomAppRemovePatchModel[]>(capturedBody!)
            .Should().BeEquivalentTo(JsonConvert.DeserializeObject<CustomAppRemovePatchModel[]>(JsonConvert.SerializeObject(changes)), opt => opt.WithStrictOrdering());
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

        var response = await client.ModifyCustomAppAsync(identifier, changes);

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(JsonConvert.DeserializeObject<CustomAppModel>(ModifyReplace));
        capturedBody.Should().NotBeNull();
        JsonConvert.DeserializeObject<CustomAppReplacePatchModel[]>(capturedBody!)
            .Should().BeEquivalentTo(JsonConvert.DeserializeObject<CustomAppReplacePatchModel[]>(JsonConvert.SerializeObject(changes)), opt => opt.WithStrictOrdering());
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

        await client.DeleteCustomAppAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task DeleteCustomApp_ByCodename_DeletesCustomApp()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ByCodename("custom_app");
        mock.Expect(HttpMethod.Delete, CustomAppBaseUrl + $"/codename/{identifier.Codename}")
            .Respond(System.Net.HttpStatusCode.OK);

        await client.DeleteCustomAppAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task DeleteCustomApp_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.DeleteCustomAppAsync(null!)).Should().ThrowAsync<ArgumentNullException>();
    }
}

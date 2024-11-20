using FluentAssertions;
using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.CustomApps;
using Kontent.Ai.Management.Models.CustomApps.Patch;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Tests.Base;
using System;
using System.Net.Http;
using Xunit;
using static Kontent.Ai.Management.Tests.Base.Scenario;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class CustomAppTests : IClassFixture<FileSystemFixture>
{
    private static readonly string CustomAppBaseUrl = $"{Endpoint}/projects/{ENVIRONMENT_ID}/custom-apps";
    private readonly Scenario _scenario = new(folder: "CustomApp");

    [Fact]
    public async void ListCustomAppsAsync_WithContinuation_ListsContentTypes()
    {
        var client = _scenario
            .WithResponses("CustomAppsPage1.json", "CustomAppsPage2.json", "CustomAppsPage3.json")
            .CreateManagementClient();

        var response = await client.ListCustomAppsAsync().GetAllAsync();

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Get)
            .ListingResponse(response)
            .Url(CustomAppBaseUrl)
            .Validate();
    }

    [Fact]
    public async void CreateCustomApp_ModelIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.CreateCustomAppAsync(null)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async void CrateCustomApp_CreatesCustomApp()
    {
        var client = _scenario.WithResponses("CustomApp.json").CreateManagementClient();
        var expected = _scenario.GetExpectedResponse<CustomAppModel>();
        var createModel = new CustomAppCreateModel
        {
            Name = expected.Name,
            Codename = expected.Codename,
            SourceUrl = expected.SourceUrl,
            Config = expected.Config,
            AllowedRoles = expected.AllowedRoles
        };

        var response = await client.CreateCustomAppAsync(createModel);

        _scenario.CreateExpectations()
            .HttpMethod(HttpMethod.Post)
            .RequestPayload(createModel)
            .Response(response)
            .Url(CustomAppBaseUrl)
            .Validate();
    }

    [Fact]
    public async void GetCustomApp_IdentifierIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.GetCustomAppAsync(null)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async void GetCustomApp_ById_GetsCustomApp()
    {
        var client = _scenario.WithResponses("CustomApp.json").CreateManagementClient();
        var identifier = Reference.ById(Guid.NewGuid());

        var response = await client.GetCustomAppAsync(identifier);

        _scenario.CreateExpectations()
            .HttpMethod(HttpMethod.Get)
            .Response(response)
            .Url(CustomAppBaseUrl + $"/{identifier.Id}")
            .Validate();
    }

    [Fact]
    public async void GetCustomApp_ByCodename_GetsCustomApp()
    {
        var client = _scenario.WithResponses("CustomApp.json").CreateManagementClient();
        var identifier = Reference.ByCodename("custom_app");

        var response = await client.GetCustomAppAsync(identifier);

        _scenario.CreateExpectations()
            .HttpMethod(HttpMethod.Get)
            .Response(response)
            .Url(CustomAppBaseUrl + $"/codename/{identifier.Codename}")
            .Validate();
    }

    [Fact]
    public async void ModifyCustomApp_AddInto_ModifiesCustomApp()
    {
        var client = _scenario.WithResponses("ModifyCustomApp_AddInto_ModifiesCustomApp.json").CreateManagementClient();
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

        var response = await client.ModifyCustomAppAsync(identifier, changes);

        _scenario.CreateExpectations()
            .HttpMethod(HttpMethod.Patch)
            .RequestPayload(changes)
            .Response(response)
            .Url(CustomAppBaseUrl + $"/{identifier.Id}")
            .Validate();
    }
    

    [Fact]
    public async void ModifyCustomApp_Remove_ModifiesCustomApp()
    {
        var client = _scenario.WithResponses("ModifyCustomApp_Remove_ModifiesCustomApp.json").CreateManagementClient();
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

        var response = await client.ModifyCustomAppAsync(identifier, changes);

        _scenario.CreateExpectations()
            .HttpMethod(HttpMethod.Patch)
            .RequestPayload(changes)
            .Response(response)
            .Url(CustomAppBaseUrl + $"/{identifier.Id}")
            .Validate();
    }

    [Fact]
    public async void ModifyCustomApp_Replace_ModifiesCustomApp()
    {
        var client = _scenario.WithResponses("ModifyCustomApp_Replace_ModifiesCustomApp.json").CreateManagementClient();
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

        var response = await client.ModifyCustomAppAsync(identifier, changes);

        _scenario.CreateExpectations()
            .HttpMethod(HttpMethod.Patch)
            .RequestPayload(changes)
            .Response(response)
            .Url(CustomAppBaseUrl + $"/{identifier.Id}")
            .Validate();
    }

    [Fact]
    public async void ModifyCustomApp_IdentifierIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();
        var changes = new CustomAppReplacePatchModel[]
        {
            new() { PropertyName = PropertyName.Name, Value = "New space name" }
        };

        await client.Invoking(x => x.ModifyCustomAppAsync(null, changes)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async void ModifyCustomApp_ChangesAreNull_Throws()
    {
        var client = _scenario.CreateManagementClient();
        var identifier = Reference.ById(Guid.NewGuid());

        await client.Invoking(x => x.ModifyCustomAppAsync(identifier, null)).Should()
            .ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async void DeleteCustomApp_ById_DeletesCustomApp()
    {
        var client = _scenario.CreateManagementClient();
        var identifier = Reference.ById(Guid.NewGuid());

        await client.DeleteCustomAppAsync(identifier);

        _scenario
            .CreateExpectations()
            .Url(CustomAppBaseUrl + $"/{identifier.Id}")
            .HttpMethod(HttpMethod.Delete)
            .Validate();
    }

    [Fact]
    public async void DeleteCustomApp_ByCodename_DeletesCustomApp()
    {
        var client = _scenario.CreateManagementClient();
        var identifier = Reference.ByCodename("custom_app");

        await client.DeleteCustomAppAsync(identifier);

        _scenario.CreateExpectations()
            .Url(CustomAppBaseUrl + $"/codename/{identifier.Codename}")
            .HttpMethod(HttpMethod.Delete)
            .Validate();
    }

    [Fact]
    public async void DeleteCustomApp_IdentifierIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.DeleteCustomAppAsync(null)).Should().ThrowAsync<ArgumentNullException>();
    }
}
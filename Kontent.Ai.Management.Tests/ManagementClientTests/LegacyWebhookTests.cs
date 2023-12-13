using FluentAssertions;
using Kontent.Ai.Management.Models.LegacyWebhooks;
using Kontent.Ai.Management.Models.LegacyWebhooks.Triggers;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Tests.Base;
using System;
using System.Net.Http;
using Xunit;
using static Kontent.Ai.Management.Tests.Base.Scenario;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class LegacyWebhookTests : IClassFixture<FileSystemFixture>
{
    private readonly Scenario _scenario;

    public LegacyWebhookTests()
    {
        _scenario = new Scenario(folder: "LegacyWebhook");
    }

    [Fact]
    public async void ListLegacyWebhooksAsync_ListsWebhooks()
    {
        var client = _scenario
            .WithResponses("LegacyWebhooks.json")
            .CreateManagementClient();

        var response = await client.ListLegacyWebhooksAsync();

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Get)
            .Response(response)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/webhooks")
            .Validate();
    }

    [Fact]
    public async void GetLegacyWebhookAsync_ById_GetsWebhook()
    {
        var client = _scenario
            .WithResponses("LegacyWebhook.json")
            .CreateManagementClient();

        var identifier = Reference.ById(Guid.NewGuid());
        var response = await client.GetLegacyWebhookAsync(identifier);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Get)
            .Response(response)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/webhooks/{identifier.Id}")
            .Validate();
    }

    [Fact]
    public async void GetLegacyWebhookAsync_ByCodename_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.GetLegacyWebhookAsync(Reference.ByCodename("codename"))).Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async void GetLegacyWebhookAsync_ByExternalId_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.GetLegacyWebhookAsync(Reference.ByExternalId("externalId"))).Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async void GetLegacyWebhookAsync_IdentifierIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.GetLegacyWebhookAsync(null)).Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async void CreateLegacyWebhookAsync_CreatesWebhook()
    {
        var client = _scenario
            .WithResponses("LegacyWebhook.json")
            .CreateManagementClient();

        var request = new LegacyWebhookCreateModel
        {
            Enabled = true,
            Name = "name",
            Secret= "password",
            Url= "url",
            Triggers = new LegacyWebhookTriggersModel
            {
                DeliveryApiContentChanges = new[] { new DeliveryApiTriggerModel { Operations = new[] { "neco" }, Type = TriggerChangeType.LanguageVariant } }
            }
        };

        var response = await client.CreateLegacyWebhookAsync(request);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Post)
            .Response(response)
            .RequestPayload(request)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/webhooks")
            .Validate();
    }

    [Fact]
    public async void CreateLegacyWebhookAsync_CreateModelIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.CreateLegacyWebhookAsync(null)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async void DeleteLegacyWebhookAsync_ById_DeletesWebhook()
    {
        var client = _scenario.CreateManagementClient();

        var identifier = Reference.ById(Guid.NewGuid());

        await client.DeleteLegacyWebhookAsync(identifier);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Delete)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/webhooks/{identifier.Id}")
            .Validate();
    }

    [Fact]
    public async void DeleteLegacyWebhookAsync_ByCodename_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.DeleteLegacyWebhookAsync(Reference.ByCodename("codename"))).Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async void DeleteLegacyWebhookAsync_ByExternalId_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.DeleteLegacyWebhookAsync(Reference.ByExternalId("externalId"))).Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async void DeleteLegacyWebhookAsync_IdentifierIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.DeleteLegacyWebhookAsync(null)).Should().ThrowAsync<Exception>();
    }
    
    [Fact]
    public async void EnableLegacyWebhookAsync_ById_EnablesWebhook()
    {
        var client = _scenario.CreateManagementClient();

        var identifier = Reference.ById(Guid.NewGuid());

        await client.EnableLegacyWebhookAsync(identifier);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Put)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/webhooks/{identifier.Id}/enable")
            .Validate();
    }

    [Fact]
    public async void EnableLegacyWebhookAsync_ByCodename_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.EnableLegacyWebhookAsync(Reference.ByCodename("codename"))).Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async void EnableLegacyWebhookAsync_ByExternalId_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.EnableLegacyWebhookAsync(Reference.ByExternalId("externalId"))).Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async void EnableLegacyWebhookAsync_IdentifierIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.EnableLegacyWebhookAsync(null)).Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async void DisableLegacyWebhookAsync_ById_EnablesWebhook()
    {
        var client = _scenario.CreateManagementClient();

        var identifier = Reference.ById(Guid.NewGuid());

        await client.DisableLegacyWebhookAsync(identifier);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Put)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/webhooks/{identifier.Id}/disable")
            .Validate();
    }

    [Fact]
    public async void DisableLegacyWebhookAsync_ByCodename_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.DisableLegacyWebhookAsync(Reference.ByCodename("codename"))).Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async void DisableLegacyWebhookAsync_ByExternalId_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.DisableLegacyWebhookAsync(Reference.ByExternalId("externalId"))).Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async void DisableLegacyWebhookAsync_IdentifierIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.DisableLegacyWebhookAsync(null)).Should().ThrowAsync<Exception>();
    }
}
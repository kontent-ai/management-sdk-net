using FluentAssertions;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.Webhooks;
using Kontent.Ai.Management.Models.Webhooks.Triggers;
using Kontent.Ai.Management.Models.Webhooks.Triggers.ContentType;
using Kontent.Ai.Management.Tests.Base;
using System;
using System.Net.Http;
using Xunit;
using static Kontent.Ai.Management.Tests.Base.Scenario;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class WebhookTests : IClassFixture<FileSystemFixture>
{
    private readonly Scenario _scenario;

    public WebhookTests()
    {
        _scenario = new Scenario(folder: "Webhook");
    }

    [Fact]
    public async void ListWebhooksAsync_ListsWebhooks()
    {
        var client = _scenario
            .WithResponses("Webhooks.json")
            .CreateManagementClient();

        var response = await client.ListWebhooksAsync();
        
        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Get)
            .Response(response)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/webhooks-vnext")
            .Validate();
    }

    [Fact]
    public async void GetWebhookAsync_ById_GetsWebhook()
    {
        var client = _scenario
            .WithResponses("Webhook.json")
            .CreateManagementClient();
        
        var identifier = Reference.ById(Guid.NewGuid());
        var response = await client.GetWebhookAsync(identifier);
        
        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Get)
            .Response(response)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/webhooks-vnext/{identifier.Id}")
            .Validate();
    }

    [Fact]
    public async void GetWebhookAsync_ByCodename_Throws()
    {
        var client = _scenario.CreateManagementClient();
        
        await client.Invoking(x => x.GetWebhookAsync(Reference.ByCodename("codename"))).Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async void GetWebhookAsync_ByExternalId_Throws()
    {
        var client = _scenario.CreateManagementClient();
        
        await client.Invoking(x => x.GetWebhookAsync(Reference.ByExternalId("externalId"))).Should().ThrowAsync<Exception>();
    }
    
    [Fact]
    public async void GetWebhookAsync_IdentifierIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();
        
        await client.Invoking(x => x.GetWebhookAsync(null)).Should().ThrowAsync<Exception>();
    }
    
    [Fact]
    public async void CreateWebhookAsync_CreatesWebhook()
    {
        var client = _scenario
            .WithResponses("Webhook.json")
            .CreateManagementClient();

        var request = new WebhookCreateModel {
            Enabled = true,
            Name = "name",
            Secret = "password",
            Url = "url",
            DeliveryTriggers = new DeliveryTriggersModel {
                ContentType = new ContentTypeTriggerModel {
                    Enabled = true,
                    Actions = new[] { new ContentTypeActionModel { Action = ContentTypeAction.Created } }
                }
            }
        };

        var response = await client.CreateWebhookAsync(request);
        
        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Post)
            .Response(response)
            .RequestPayload(request)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/webhooks-vnext")
            .Validate();
    }

    [Fact]
    public async void CreateWebhookAsync_CreateModelIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();
        
        await client.Invoking(x => x.CreateWebhookAsync(null)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async void DeleteWebhookAsync_ById_DeletesWebhook()
    {
        var client = _scenario.CreateManagementClient();

        var identifier = Reference.ById(Guid.NewGuid());

        await client.DeleteWebhookAsync(identifier);
        
        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Delete)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/webhooks-vnext/{identifier.Id}")
            .Validate();
    }

    [Fact]
    public async void DeleteWebhookAsync_ByCodename_Throws()
    {
        var client = _scenario.CreateManagementClient();
        
        await client.Invoking(x => x.DeleteWebhookAsync(Reference.ByCodename("codename"))).Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async void DeleteWebhookAsync_ByExternalId_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.DeleteWebhookAsync(Reference.ByExternalId("externalId"))).Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async void DeleteWebhookAsync_IdentifierIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();
        
        await client.Invoking(x => x.DeleteWebhookAsync(null)).Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async void EnableWebhookAsync_ById_EnablesWebhook()
    {
        var client = _scenario.CreateManagementClient();

        var identifier = Reference.ById(Guid.NewGuid());

        await client.EnableWebhookAsync(identifier);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Put)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/webhooks-vnext/{identifier.Id}/enable")
            .Validate();
    }

    [Fact]
    public async void EnableWebhookAsync_ByCodename_Throws()
    {
        var client = _scenario.CreateManagementClient();
        
        await client.Invoking(x => x.EnableWebhookAsync(Reference.ByCodename("codename"))).Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async void EnableWebhookAsync_ByExternalId_Throws()
    {
        var client = _scenario.CreateManagementClient();
        
        await client.Invoking(x => x.EnableWebhookAsync(Reference.ByExternalId("externalId"))).Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async void EnableWebhookAsync_IdentifierIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();
        
        await client.Invoking(x => x.EnableWebhookAsync(null)).Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async void DisableWebhookAsync_ById_DisablesWebhook()
    {
        var client = _scenario.CreateManagementClient();

        var identifier = Reference.ById(Guid.NewGuid());

        await client.DisableWebhookAsync(identifier);
        
        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Put)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/webhooks-vnext/{identifier.Id}/disable")
            .Validate();
    }

    [Fact]
    public async void DisableWebhookAsync_ByCodename_Throws()
    {
        var client = _scenario.CreateManagementClient();
        
        await client.Invoking(x => x.DisableWebhookAsync(Reference.ByCodename("codename"))).Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async void DisableWebhookAsync_ByExternalId_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.DisableWebhookAsync(Reference.ByExternalId("externalId"))).Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async void DisableWebhookAsync_IdentifierIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();
        
        await client.Invoking(x => x.DisableWebhookAsync(null)).Should().ThrowAsync<Exception>();
    }
}
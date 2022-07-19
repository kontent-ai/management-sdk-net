using FluentAssertions;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.Webhooks;
using Kontent.Ai.Management.Tests.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class WebhookTests : IClassFixture<FileSystemFixture>
{
    private readonly FileSystemFixture _fileSystemFixture;

    public WebhookTests(FileSystemFixture fileSystemFixture)
    {
        _fileSystemFixture = fileSystemFixture;
        _fileSystemFixture.SetSubFolder("Webhook");
    }


    [Fact]
    public async void ListWebhooks_ListsWebhooks()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("Webhooks.json");

        var webhooks = _fileSystemFixture.GetExpectedResponse<IEnumerable<WebhookModel>>("Webhooks.json");

        var response = await client.ListWebhooksAsync();

        response.Should().BeEquivalentTo(webhooks);
    }

    [Fact]
    public async void GetWebhook_ById_GetsWebhook()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("Webhook.json");

        var webhook = _fileSystemFixture.GetExpectedResponse<WebhookModel>("Webhook.json");

        var response = await client.GetWebhookAsync(Reference.ById(webhook.Id));

        response.Should().BeEquivalentTo(webhook);
    }

    [Fact]
    public async void CreateWebhookGroup_CreatesWebhookGroup()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("Webhook.json");

        var webhook = _fileSystemFixture.GetExpectedResponse<WebhookModel>("Webhook.json");

        var response = await client.CreateWebhookAsync(ToCreateModel(webhook));

        response.Should().BeEquivalentTo(webhook);
    }

    [Fact]
    public async void DeleteWebhook_ById_DeletesWebhook()
    {
        var client = _fileSystemFixture.CreateMockClientWithoutResponse();

        var identifier = Reference.ById(Guid.NewGuid());

        Func<Task> deleteWebhook = async () => await client.DeleteWebhookAsync(identifier);

        await deleteWebhook.Should().NotThrowAsync();
    }

    [Fact]
    public async void EnableWebhook_ById_EnablesWebhook()
    {
        var client = _fileSystemFixture.CreateMockClientWithoutResponse();

        Func<Task> enableWebhook = async () => await client.EnableWebhookAsync(Reference.ById(Guid.NewGuid()));

        await enableWebhook.Should().NotThrowAsync();
    }

    [Fact]
    public async void DisableWebhook_ById_DisablesWebhook()
    {
        var client = _fileSystemFixture.CreateMockClientWithoutResponse();

        Func<Task> disableWebhook = async () => await client.DisableWebhookAsync(Reference.ById(Guid.NewGuid()));

        await disableWebhook.Should().NotThrowAsync();
    }

    private static WebhookCreateModel ToCreateModel(WebhookModel webhook) => new()
    {
        Enabled = webhook.Enabled,
        Name = webhook.Name,
        Secret = webhook.Secret,
        Url = webhook.Url,
        Triggers = webhook.Triggers,
    };
}

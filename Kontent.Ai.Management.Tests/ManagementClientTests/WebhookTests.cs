using System.Net;
using AwesomeAssertions;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.Webhooks;
using Kontent.Ai.Management.Models.Webhooks.Triggers;
using Kontent.Ai.Management.Models.Webhooks.Triggers.ContentType;
using Kontent.Ai.Management.Tests.Base;
using Newtonsoft.Json;
using RichardSzalay.MockHttp;
using Xunit;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class WebhookTests
{
    private static string Webhooks => Fixture("Webhooks.json");
    private static string Webhook => Fixture("Webhook.json");

    private static string Fixture(string name)
        => File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Data", "Webhook", name));

    // ById is the only kind webhooks accept; the rest must throw before any HTTP call.
    public static TheoryData<Reference?> InvalidIdentifiers =>
    [
        Reference.ByCodename("codename"),
        Reference.ByExternalId("externalId"),
        null,
    ];

    [Fact]
    public async Task ListWebhooksAsync_ListsWebhooks()
    {
        var (client, mock) = MockClientFactory.Create();
        mock.Expect(HttpMethod.Get, $"{MockClientFactory.BaseUrl}/webhooks-vnext")
            .Respond("application/json", Webhooks);

        var response = await client.ListWebhooksAsync();

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(JsonConvert.DeserializeObject<List<WebhookModel>>(Webhooks));
    }

    [Fact]
    public async Task GetWebhookAsync_ById_GetsWebhook()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ById(Guid.NewGuid());
        mock.Expect(HttpMethod.Get, $"{MockClientFactory.BaseUrl}/webhooks-vnext/{identifier.Id}")
            .Respond("application/json", Webhook);

        var response = await client.GetWebhookAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(JsonConvert.DeserializeObject<WebhookModel>(Webhook));
    }

    [Theory]
    [MemberData(nameof(InvalidIdentifiers))]
    public async Task GetWebhookAsync_InvalidIdentifier_Throws(Reference? identifier)
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.GetWebhookAsync(identifier!)).Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async Task CreateWebhookAsync_CreatesWebhook()
    {
        var (client, mock) = MockClientFactory.Create();
        var request = new WebhookCreateModel
        {
            Enabled = true,
            Name = "name",
            Secret = "password",
            Headers = [new CustomHeaderModel { Key = "key1", Value = "value1" }],
            Url = "url",
            DeliveryTriggers = new DeliveryTriggersModel
            {
                ContentType = new ContentTypeTriggerModel
                {
                    Enabled = true,
                    Actions = [new ContentTypeActionModel { Action = ContentTypeAction.Created }],
                },
            },
        };

        string? capturedBody = null;
        mock.Expect(HttpMethod.Post, $"{MockClientFactory.BaseUrl}/webhooks-vnext")
            .With(r =>
            {
                capturedBody = r.Content!.ReadAsStringAsync().GetAwaiter().GetResult();
                return true;
            })
            .Respond("application/json", Webhook);

        var response = await client.CreateWebhookAsync(request);

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(JsonConvert.DeserializeObject<WebhookModel>(Webhook));
        capturedBody.Should().NotBeNull();
        // Object-graph compare (serializer-robust); both sides round-tripped through the current Newtonsoft transport.
        JsonConvert.DeserializeObject<WebhookCreateModel>(capturedBody!)
            .Should().BeEquivalentTo(JsonConvert.DeserializeObject<WebhookCreateModel>(JsonConvert.SerializeObject(request)));
    }

    [Fact]
    public async Task CreateWebhookAsync_CreateModelIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.CreateWebhookAsync(null!)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task DeleteWebhookAsync_ById_DeletesWebhook()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ById(Guid.NewGuid());
        mock.Expect(HttpMethod.Delete, $"{MockClientFactory.BaseUrl}/webhooks-vnext/{identifier.Id}")
            .Respond(HttpStatusCode.OK);

        await client.DeleteWebhookAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
    }

    [Theory]
    [MemberData(nameof(InvalidIdentifiers))]
    public async Task DeleteWebhookAsync_InvalidIdentifier_Throws(Reference? identifier)
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.DeleteWebhookAsync(identifier!)).Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async Task EnableWebhookAsync_ById_EnablesWebhook()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ById(Guid.NewGuid());
        mock.Expect(HttpMethod.Put, $"{MockClientFactory.BaseUrl}/webhooks-vnext/{identifier.Id}/enable")
            .Respond(HttpStatusCode.OK);

        await client.EnableWebhookAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
    }

    [Theory]
    [MemberData(nameof(InvalidIdentifiers))]
    public async Task EnableWebhookAsync_InvalidIdentifier_Throws(Reference? identifier)
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.EnableWebhookAsync(identifier!)).Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async Task DisableWebhookAsync_ById_DisablesWebhook()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ById(Guid.NewGuid());
        mock.Expect(HttpMethod.Put, $"{MockClientFactory.BaseUrl}/webhooks-vnext/{identifier.Id}/disable")
            .Respond(HttpStatusCode.OK);

        await client.DisableWebhookAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
    }

    [Theory]
    [MemberData(nameof(InvalidIdentifiers))]
    public async Task DisableWebhookAsync_InvalidIdentifier_Throws(Reference? identifier)
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.DisableWebhookAsync(identifier!)).Should().ThrowAsync<Exception>();
    }
}

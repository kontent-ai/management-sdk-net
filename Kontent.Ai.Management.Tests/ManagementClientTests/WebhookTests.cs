using System.Net;
using System.Text.Json;
using AwesomeAssertions;
using Kontent.Ai.Management.Models.Webhooks;
using Kontent.Ai.Management.Models.Webhooks.Triggers;
using Kontent.Ai.Management.Models.Webhooks.Triggers.ContentType;
using Kontent.Ai.Management.Tests.Base;
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

        var result = await client.ListWebhooksAsync();

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.StatusCode.Should().Be(HttpStatusCode.OK);

        var webhooks = result.Value.ToList();
        webhooks.Should().HaveCount(2);
        webhooks[0].Id.Should().Be(Guid.Parse("3e18029a-ed40-406f-a49a-1a8856200e11"));
        webhooks[0].Name.Should().Be("webhooks_all_triggers");
        webhooks[1].Id.Should().Be(Guid.Parse("78d634ed-445f-4b2e-aefc-8914c9bef201"));
        webhooks[1].Name.Should().Be("second_webhook");
        webhooks[1].Headers.Should().BeEmpty();
    }

    [Fact]
    public async Task GetWebhookAsync_ById_GetsWebhook()
    {
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ById(Guid.NewGuid());
        mock.Expect(HttpMethod.Get, $"{MockClientFactory.BaseUrl}/webhooks-vnext/{identifier.Id}")
            .Respond("application/json", Webhook);

        var result = await client.GetWebhookAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        AssertIsAllTriggersWebhook(result.Value);
    }

    [Theory]
    [MemberData(nameof(InvalidIdentifiers))]
    public async Task GetWebhookAsync_InvalidIdentifier_Throws(Reference? identifier)
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.GetWebhookAsync(identifier!)).Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async Task GetWebhookAsync_ApiError_ReturnsFailureResult()
    {
        const string errorBody = """
            { "message": "The requested webhook was not found.", "request_id": "req-42", "error_code": 124 }
            """;
        var (client, mock) = MockClientFactory.Create();
        var identifier = Reference.ById(Guid.NewGuid());
        mock.Expect(HttpMethod.Get, $"{MockClientFactory.BaseUrl}/webhooks-vnext/{identifier.Id}")
            .Respond(HttpStatusCode.NotFound, "application/json", errorBody);

        var result = await client.GetWebhookAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        result.Value.Should().BeNull();
        result.Error.Should().NotBeNull();
        result.Error!.Message.Should().Be("The requested webhook was not found.");
        result.Error.RequestId.Should().Be("req-42");
        result.Error.ErrorCode.Should().Be(124);
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

        WebhookCreateModel? sentBody = null;
        mock.Expect(HttpMethod.Post, $"{MockClientFactory.BaseUrl}/webhooks-vnext")
            .With(r =>
            {
                var body = r.Content!.ReadAsStringAsync().GetAwaiter().GetResult();
                sentBody = JsonSerializer.Deserialize<WebhookCreateModel>(body, SharedTestJsonOptions.Default);
                return true;
            })
            .Respond("application/json", Webhook);

        var result = await client.CreateWebhookAsync(request);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        AssertIsAllTriggersWebhook(result.Value);

        // The body the SDK sent round-trips back to the model the caller passed.
        sentBody.Should().BeEquivalentTo(request);
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

        var result = await client.DeleteWebhookAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.StatusCode.Should().Be(HttpStatusCode.OK);
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

        var result = await client.EnableWebhookAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.StatusCode.Should().Be(HttpStatusCode.OK);
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

        var result = await client.DisableWebhookAsync(identifier);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory]
    [MemberData(nameof(InvalidIdentifiers))]
    public async Task DisableWebhookAsync_InvalidIdentifier_Throws(Reference? identifier)
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.DisableWebhookAsync(identifier!)).Should().ThrowAsync<Exception>();
    }

    // Webhook.json — asserted with literal expected values rather than a fixture round-trip.
    private static void AssertIsAllTriggersWebhook(WebhookModel webhook)
    {
        webhook.Id.Should().Be(Guid.Parse("3e18029a-ed40-406f-a49a-1a8856200e11"));
        webhook.Name.Should().Be("Webhook_all_triggers");
        webhook.Url.Should().Be("http://test");
        webhook.Secret.Should().Be("AQA1jsNDjLBUcRRD69GQtuzBBqxGPdz+0XhTKhICv4o=");
        webhook.Enabled.Should().BeTrue();
        webhook.Headers.Should().ContainSingle()
            .Which.Should().BeEquivalentTo(new CustomHeaderModel { Key = "key1", Value = "value1" });
        webhook.DeliveryTriggers.Should().NotBeNull();
        webhook.DeliveryTriggers.ContentType.Enabled.Should().BeTrue();
    }
}

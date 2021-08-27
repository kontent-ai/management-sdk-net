using Kentico.Kontent.Management.Exceptions;
using Kentico.Kontent.Management.Models.Shared;
using Kentico.Kontent.Management.Models.Webhooks;
using Kentico.Kontent.Management.Models.Webhooks.Triggers;
using System.Linq;
using System.Reflection;
using Xunit;
using Xunit.Abstractions;
using static Kentico.Kontent.Management.Tests.ManagementClientTests.Scenario;

namespace Kentico.Kontent.Management.Tests.ManagementClientTests
{
    [Trait("ManagementClient", "Webhooks")]
    public class WebhookTests
    {
        private readonly ManagementClient _client;
        private readonly Scenario _scenario;

        public WebhookTests(ITestOutputHelper output)
        {
            //this magic can be replace once new xunit is delivered
            //https://github.com/xunit/xunit/issues/621
            var type = output.GetType();
            var testMember = type.GetField("test", BindingFlags.Instance | BindingFlags.NonPublic);
            var test = (ITest)testMember.GetValue(output);

            _scenario = new Scenario(test.TestCase.TestMethod.Method.Name);
            _client = _scenario.Client;
        }

        [Fact]
        public async void ListWebhooks_ListsWebhooks()
        {
            var response = await _client.ListWebhooksAsync();

            Assert.NotNull(response);
            Assert.NotNull(response.FirstOrDefault(x => x.Name == EXISTING_WEBHOOK_NAME));
        }

        [Fact]
        public async void GetWebhook_ById_GetsWebhook()
        {
            var identifier = ObjectIdentifier.ById(EXISTING_WEBHOOK_ID);

            var response = await _client.GetWebhookAsync(identifier);
            Assert.Equal(EXISTING_WEBHOOK_ID, response.Id.ToString("d"));
        }

        [Fact]
        public async void CreateWebhookGroup_CreatesWebhookGroup()
        {
            var webhook = new WebhookCreateModel
            {
                Name = "webhookTestName",
                Url = "http://url",
                Enabled = true,
                Secret = "secret",
                Triggers = new WebhookTriggersModel
                {
                    DeliveryApiContentChanges = new[] 
                    { 
                        new DeliveryApiTriggerModel 
                        { 
                            Type = TriggerChangeType.ContentItemVariant,
                            Operations = new [] { "publish", "unpublish" }
                        },
                        new DeliveryApiTriggerModel
                        {
                            Type = TriggerChangeType.Taxonomy,
                            Operations = new [] { "archive", "restore", "upsert" }
                        },
                    }
                }
            };

            var createdWebhook = await _client.CreateWebhookAsync(webhook);

            Assert.Equal(webhook.Name, createdWebhook.Name);
            Assert.Equal(webhook.Url, createdWebhook.Url);
            Assert.Equal(webhook.Secret, createdWebhook.Secret);
            Assert.Equal(webhook.Enabled, createdWebhook.Enabled);
            Assert.Equal(webhook.Triggers.DeliveryApiContentChanges.Count(), createdWebhook.Triggers.DeliveryApiContentChanges.Count());

            // Cleanup
            var webhookToClean = ObjectIdentifier.ById(createdWebhook.Id);
            await _client.DeleteWebhookAsync(webhookToClean);
        }

        [Fact]
        public async void DeleteWebhook_ById_DeletesWebhook()
        {
            var webhook = new WebhookCreateModel
            {
                Name = "webhookTestName",
                Url = "http://url",
                Enabled = true,
                Secret = "secret",
                Triggers = new WebhookTriggersModel
                {
                    DeliveryApiContentChanges = new[]
                    {
                        new DeliveryApiTriggerModel
                        {
                            Type = TriggerChangeType.ContentItemVariant,
                            Operations = new [] { "publish", "unpublish" }
                        },
                        new DeliveryApiTriggerModel
                        {
                            Type = TriggerChangeType.Taxonomy,
                            Operations = new [] { "archive", "restore", "upsert" }
                        },
                    }
                }
            };

            var createdWebhook = await _client.CreateWebhookAsync(webhook);


            var exception = await Record.ExceptionAsync(async () => await _client.DeleteWebhookAsync(ObjectIdentifier.ById(createdWebhook.Id)));

            if (RunType != TestUtils.TestRunType.MockFromFileSystem)
            {
                await Assert.ThrowsAsync<ManagementException>(async () => await _client.DeleteWebhookAsync(ObjectIdentifier.ById(createdWebhook.Id)));
            }

            Assert.Null(exception);
        }

        [Fact]
        public async void EnableWebhook_ById_EnablesWebhook()
        {
            var webhook = new WebhookCreateModel
            {
                Name = "disabledWebhook",
                Url = "http://url",
                Enabled = false,
                Secret = "secret",
                Triggers = new WebhookTriggersModel
                {
                    DeliveryApiContentChanges = new[]
                    {
                        new DeliveryApiTriggerModel
                        {
                            Type = TriggerChangeType.Taxonomy,
                            Operations = new [] { "archive", "restore", "upsert" }
                        },
                    }
                }
            };

            var createdWebhook = await _client.CreateWebhookAsync(webhook);

            //todo mistake in docs enable/disable does not return payload
            await _client.EnableWebhookAsync(ObjectIdentifier.ById(createdWebhook.Id));

            var enabledWebhook = await _client.GetWebhookAsync(ObjectIdentifier.ById(createdWebhook.Id));

            Assert.True(enabledWebhook.Enabled);

            // Cleanup
            var webhookToClean = ObjectIdentifier.ById(createdWebhook.Id);
            await _client.DeleteWebhookAsync(webhookToClean);
        }

        [Fact]
        public async void DisableWebhook_ById_DisablesWebhook()
        {
            var webhook = new WebhookCreateModel
            {
                Name = "enabledWebhook",
                Url = "http://url",
                Enabled = true,
                Secret = "secret",
                Triggers = new WebhookTriggersModel 
                {
                    DeliveryApiContentChanges = new[]
                    {
                        new DeliveryApiTriggerModel
                        {
                            Type = TriggerChangeType.ContentItemVariant,
                            Operations = new [] { "publish", "unpublish" }
                        }
                    }
                }
            };

            var createdWebhook = await _client.CreateWebhookAsync(webhook);

            //todo mistake in docs enable/disable does not return payload
            await _client.DisableWebhookAsync(ObjectIdentifier.ById(createdWebhook.Id));

            var enabledWebhook = await _client.GetWebhookAsync(ObjectIdentifier.ById(createdWebhook.Id));

            Assert.False(enabledWebhook.Enabled);

            // Cleanup
            var webhookToClean = ObjectIdentifier.ById(createdWebhook.Id);
            await _client.DeleteWebhookAsync(webhookToClean);
        }
    }
}

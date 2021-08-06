using Kentico.Kontent.Management.Exceptions;
using Kentico.Kontent.Management.Models;
using Kentico.Kontent.Management.Models.Webhooks;
using Kentico.Kontent.Management.Models.Webhooks.Triggers;
using System.Linq;
using Xunit;

namespace Kentico.Kontent.Management.Tests.ManagementClientTests
{
    partial class ManagementClientTests
    {
        [Fact]
        [Trait("Category", "Webhooks")]
        public async void ListWebhooks_ListsWebhooks()
        {
            var client = CreateManagementClient(nameof(ListWebhooks_ListsWebhooks));

            var response = await client.ListWebhooksAsync();

            Assert.NotNull(response);
            Assert.NotNull(response.FirstOrDefault(x => x.Name == EXISTING_WEBHOOK_NAME));
        }

        [Fact]
        [Trait("Category", "Webhooks")]
        public async void GetWebhook_ById_GetsWebhook()
        {
            var client = CreateManagementClient(nameof(GetWebhook_ById_GetsWebhook));

            var identifier = ObjectIdentifier.ById(EXISTING_WEBHOOK_ID);

            var response = await client.GetWebhookAsync(identifier);
            Assert.Equal(EXISTING_WEBHOOK_ID, response.Id.ToString("d"));
        }

        [Fact]
        [Trait("Category", "Webhooks")]
        public async void CreateWebhookGroup_CreatesWebhookGroup()
        {
            var client = CreateManagementClient(nameof(CreateContentType_CreatesContentType));

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

            var createdWebhook = await client.CreateWebhookAsync(webhook);

            Assert.Equal(webhook.Name, createdWebhook.Name);
            Assert.Equal(webhook.Url, createdWebhook.Url);
            Assert.Equal(webhook.Secret, createdWebhook.Secret);
            Assert.Equal(webhook.Enabled, createdWebhook.Enabled);
            Assert.Equal(webhook.Triggers.DeliveryApiContentChanges.Count(), createdWebhook.Triggers.DeliveryApiContentChanges.Count());

            // Cleanup
            var webhookToClean = ObjectIdentifier.ById(createdWebhook.Id);
            await client.DeleteWebhookAsync(webhookToClean);
        }

        [Fact]
        [Trait("Category", "Webhooks")]
        public async void DeleteWebhook_ById_DeletesWebhook()
        {
            var client = CreateManagementClient(nameof(DeleteWebhook_ById_DeletesWebhook));

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

            var createdWebhook = await client.CreateWebhookAsync(webhook);


            var exception = await Record.ExceptionAsync(async () => await client.DeleteWebhookAsync(ObjectIdentifier.ById(createdWebhook.Id)));

            if (_runType != TestUtils.TestRunType.MockFromFileSystem)
            {
                await Assert.ThrowsAsync<ManagementException>(async () => await client.DeleteWebhookAsync(ObjectIdentifier.ById(createdWebhook.Id)));
            }

            Assert.Null(exception);
        }

        [Fact]
        [Trait("Category", "Webhooks")]
        public async void EnableWebhook_ById_EnablesWebhook()
        {
            var client = CreateManagementClient(nameof(EnableWebhook_ById_EnablesWebhook));

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

            var createdWebhook = await client.CreateWebhookAsync(webhook);

            //todo mistake in docs enable/disable does not return payload
            await client.EnableWebhookAsync(ObjectIdentifier.ById(createdWebhook.Id));

            var enabledWebhook = await client.GetWebhookAsync(ObjectIdentifier.ById(createdWebhook.Id));

            Assert.True(enabledWebhook.Enabled);

            // Cleanup
            var webhookToClean = ObjectIdentifier.ById(createdWebhook.Id);
            await client.DeleteWebhookAsync(webhookToClean);
        }

        [Fact]
        [Trait("Category", "Webhooks")]
        public async void DisableWebhook_ById_DisablesWebhook()
        {
            var client = CreateManagementClient(nameof(EnableWebhook_ById_EnablesWebhook));

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

            var createdWebhook = await client.CreateWebhookAsync(webhook);

            //todo mistake in docs enable/disable does not return payload
            await client.DisableWebhookAsync(ObjectIdentifier.ById(createdWebhook.Id));

            var enabledWebhook = await client.GetWebhookAsync(ObjectIdentifier.ById(createdWebhook.Id));

            Assert.False(enabledWebhook.Enabled);

            // Cleanup
            var webhookToClean = ObjectIdentifier.ById(createdWebhook.Id);
            await client.DeleteWebhookAsync(webhookToClean);
        }
    }
}

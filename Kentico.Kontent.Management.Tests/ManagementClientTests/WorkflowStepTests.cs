using Kentico.Kontent.Management.Models.LanguageVariants;
using Kentico.Kontent.Management.Models.Shared;
using Kentico.Kontent.Management.Models.Workflow;
using System;
using System.Linq;
using Xunit;

namespace Kentico.Kontent.Management.Tests.ManagementClientTests
{
    public partial class ManagementClientTests
    {
        [Fact]
        [Trait("Category", "Workflow")]
        public async void ListWorkflowSteps_ListsWorkflowSteps()
        {
            var client = CreateManagementClient(nameof(ListWorkflowSteps_ListsWorkflowSteps));

            var response = await client.ListWorkflowStepsAsync();

            Assert.NotNull(response);
            Assert.NotNull(response.FirstOrDefault(x => x.Id == PUBLISHED_WORKFLOW_STEP_ID));
        }

        [Fact]
        [Trait("Category", "Workflow")]
        public async void ChangeWorkflowStepOfVariant_ChangesWorkflowStepOfVariant()
        {
            var client = CreateManagementClient(nameof(ChangeWorkflowStepOfVariant_ChangesWorkflowStepOfVariant));

            // Arrange
            var externalId = "11102811b05f429284006ea94c68333";
            var preparedItem = await TestUtils.PrepareTestItem(client, EXISTING_CONTENT_TYPE_CODENAME, externalId);
            var preparedVariant = await TestUtils.PrepareTestVariant(client, EXISTING_LANGUAGE_CODENAME, _elements, preparedItem);

            // Test
            var itemIdentifier = Reference.ByExternalId(externalId);
            var languageIdentifier = Reference.ByCodename(EXISTING_LANGUAGE_CODENAME);
            var variantIdentifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

            var identifier = new WorkflowIdentifier(itemIdentifier, languageIdentifier, NoExternalIdIdentifier.ByCodename("test"));

            await client.ChangeWorkflowStep(identifier);

            var updatedVariant = await client.GetContentItemVariantAsync(variantIdentifier);
            var workflowSteps = await client.ListWorkflowStepsAsync();

            Assert.Equal(CUSTOM_WORKFLOW_STEP_ID, updatedVariant.WorkflowStep.Id);

            // Cleanup
            var itemToClean = Reference.ByExternalId(externalId);
            await client.DeleteContentItemAsync(itemToClean);
        }

        [Fact]
        [Trait("Category", "Workflow")]
        public async void PublishVariant_PublishesVariant()
        {
            var client = CreateManagementClient(nameof(PublishVariant_PublishesVariant));

            // Arrange
            var externalId = "22202811b05f429284006ea94c68333";
            var preparedItem = await TestUtils.PrepareTestItem(client, EXISTING_CONTENT_TYPE_CODENAME, externalId);
            var preparedVariant = await TestUtils.PrepareTestVariant(client, EXISTING_LANGUAGE_CODENAME, _elements, preparedItem);

            // Test
            var itemIdentifier = Reference.ByExternalId(externalId);
            var languageIdentifier = Reference.ByCodename(EXISTING_LANGUAGE_CODENAME);
            var variantIdentifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

            await client.PublishContentItemVariant(variantIdentifier);

            var updatedVariant = await client.GetContentItemVariantAsync(variantIdentifier);

            Assert.Equal(PUBLISHED_WORKFLOW_STEP_ID, updatedVariant.WorkflowStep.Id);

            // Cleanup
            var itemToClean = Reference.ByExternalId(externalId);
            await client.DeleteContentItemAsync(itemToClean);
        }

        [Fact]
        [Trait("Category", "Workflow")]
        public async void UnpublishVariant_UnpublishesVariant()
        {
            var client = CreateManagementClient(nameof(PublishVariant_PublishesVariant));

            // Arrange
            var externalId = "78902811b05f429284006ea94c68333";
            var preparedItem = await TestUtils.PrepareTestItem(client, EXISTING_CONTENT_TYPE_CODENAME, externalId);
            var preparedVariant = await TestUtils.PrepareTestVariant(client, EXISTING_LANGUAGE_CODENAME, _elements, preparedItem);

            // Test
            var itemIdentifier = Reference.ByExternalId(externalId);
            var languageIdentifier = Reference.ByCodename(EXISTING_LANGUAGE_CODENAME);
            var variantIdentifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

            await client.PublishContentItemVariant(variantIdentifier);

            await client.UnpublishContentItemVariant(variantIdentifier);

            var updatedVariant = await client.GetContentItemVariantAsync(variantIdentifier);

            Assert.Equal(ARCHIVED_WORKFLOW_STEP_ID, updatedVariant.WorkflowStep.Id);

            // Cleanup
            var itemToClean = Reference.ByExternalId(externalId);
            await client.DeleteContentItemAsync(itemToClean);
        }

        [Fact]
        [Trait("Category", "Workflow")]
        public async void CreateNewVersionsOfVariant_CreatesNewVersionsOfVariant()
        {
            var client = CreateManagementClient(nameof(CreateNewVersionsOfVariant_CreatesNewVersionsOfVariant));

            // Arrange
            var externalId = "33302811b05f429284006ea94c68333";
            var preparedItem = await TestUtils.PrepareTestItem(client, EXISTING_CONTENT_TYPE_CODENAME, externalId);
            var preparedVariant = await TestUtils.PrepareTestVariant(client, EXISTING_LANGUAGE_CODENAME, _elements, preparedItem);

            // Test
            var itemIdentifier = Reference.ByExternalId(externalId);
            var languageIdentifier = Reference.ByCodename(EXISTING_LANGUAGE_CODENAME);
            var variantIdentifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

            await client.PublishContentItemVariant(variantIdentifier);
            await client.CreateNewVersionOfContentItemVariant(variantIdentifier);

            var updatedVariant = await client.GetContentItemVariantAsync(variantIdentifier);

            Assert.Equal(DRAFT_WORKFLOW_STEP_ID, updatedVariant.WorkflowStep.Id);

            // Cleanup
            var itemToClean = Reference.ByExternalId(externalId);
            await client.DeleteContentItemAsync(itemToClean);
        }

        [Fact]
        [Trait("Category", "Workflow")]
        public async void SchedulePublishingOfVariant_SchedulesPublishingOfVariant()
        {
            var client = CreateManagementClient(nameof(SchedulePublishingOfVariant_SchedulesPublishingOfVariant));

            // Arrange
            var externalId = "32302811b05f429284006ea94c68333";
            var preparedItem = await TestUtils.PrepareTestItem(client, EXISTING_CONTENT_TYPE_CODENAME, externalId);
            var preparedVariant = await TestUtils.PrepareTestVariant(client, EXISTING_LANGUAGE_CODENAME, _elements, preparedItem);

            // Test
            var itemIdentifier = Reference.ByExternalId(externalId);
            var languageIdentifier = Reference.ByCodename(EXISTING_LANGUAGE_CODENAME);
            var variantIdentifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

            await client.SchedulePublishingOfContentItemVariant(variantIdentifier, new ScheduleModel { ScheduleTo = new DateTimeOffset(2050, 1, 1, 10, 10, 0, TimeSpan.FromHours(2)) });

            var updatedVariant = await client.GetContentItemVariantAsync(variantIdentifier);

            Assert.Equal(SCHEDULED_WORKFLOW_STEP_ID, updatedVariant.WorkflowStep.Id);

            // Cleanup
            var itemToClean = Reference.ByExternalId(externalId);
            await client.DeleteContentItemAsync(itemToClean);
        }

        [Fact]
        [Trait("Category", "Workflow")]
        public async void ScheduleUnpublishingOfVariant_SchedulesUnpublishingOfVariant()
        {
            var client = CreateManagementClient(nameof(ScheduleUnpublishingOfVariant_SchedulesUnpublishingOfVariant));

            // Arrange
            var externalId = "35302811b05f429284006ea94c68333";
            var preparedItem = await TestUtils.PrepareTestItem(client, EXISTING_CONTENT_TYPE_CODENAME, externalId);
            var preparedVariant = await TestUtils.PrepareTestVariant(client, EXISTING_LANGUAGE_CODENAME, _elements, preparedItem);

            // Test
            var itemIdentifier = Reference.ByExternalId(externalId);
            var languageIdentifier = Reference.ByCodename(EXISTING_LANGUAGE_CODENAME);
            var variantIdentifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

            await client.PublishContentItemVariant(variantIdentifier);

            await client.ScheduleUnpublishingOfContentItemVariant(variantIdentifier, new ScheduleModel { ScheduleTo = new DateTimeOffset(2050, 1, 1, 10, 10, 0, TimeSpan.FromHours(2)) });

            var updatedVariant = await client.GetContentItemVariantAsync(variantIdentifier);

            Assert.Equal(PUBLISHED_WORKFLOW_STEP_ID, updatedVariant.WorkflowStep.Id);

            // Cleanup
            await client.UnpublishContentItemVariant(variantIdentifier);
            var itemToClean = Reference.ByExternalId(externalId);
            await client.DeleteContentItemAsync(itemToClean);
        }

        [Fact]
        [Trait("Category", "Workflow")]
        public async void CancelPublishingOfVariant_CancelsPublishingOfVariant()
        {
            var client = CreateManagementClient(nameof(CancelPublishingOfVariant_CancelsPublishingOfVariant));

            // Arrange
            var externalId = "37302811b05f429284006ea94c68333";
            var preparedItem = await TestUtils.PrepareTestItem(client, EXISTING_CONTENT_TYPE_CODENAME, externalId);
            var preparedVariant = await TestUtils.PrepareTestVariant(client, EXISTING_LANGUAGE_CODENAME, _elements, preparedItem);

            // Test
            var itemIdentifier = Reference.ByExternalId(externalId);
            var languageIdentifier = Reference.ByCodename(EXISTING_LANGUAGE_CODENAME);
            var variantIdentifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

            await client.SchedulePublishingOfContentItemVariant(variantIdentifier, new ScheduleModel { ScheduleTo = new DateTimeOffset(2050, 1, 1, 10, 10, 0, TimeSpan.FromHours(2)) });
            await client.CancelPublishingOfContentItemVariant(variantIdentifier);

            var updatedVariant = await client.GetContentItemVariantAsync(variantIdentifier);

            Assert.Equal(DRAFT_WORKFLOW_STEP_ID, updatedVariant.WorkflowStep.Id);

            // Cleanup
            var itemToClean = Reference.ByExternalId(externalId);
            await client.DeleteContentItemAsync(itemToClean);
        }

        [Fact]
        [Trait("Category", "Workflow")]
        public async void CancelUnpublishingOfVariant_CancelsUnpublishingOfVariant()
        {
            var client = CreateManagementClient(nameof(CancelUnpublishingOfVariant_CancelsUnpublishingOfVariant));

            // Arrange
            var externalId = "37302811b05f429284006ea94c68333";
            var preparedItem = await TestUtils.PrepareTestItem(client, EXISTING_CONTENT_TYPE_CODENAME, externalId);
            var preparedVariant = await TestUtils.PrepareTestVariant(client, EXISTING_LANGUAGE_CODENAME, _elements, preparedItem);

            // Test
            var itemIdentifier = Reference.ByExternalId(externalId);
            var languageIdentifier = Reference.ByCodename(EXISTING_LANGUAGE_CODENAME);
            var variantIdentifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

            await client.PublishContentItemVariant(variantIdentifier);
            await client.ScheduleUnpublishingOfContentItemVariant(variantIdentifier, new ScheduleModel { ScheduleTo = new DateTimeOffset(2050,1,1,10,10,0, TimeSpan.FromHours(2)) });
            await client.CancelUnpublishingOfContentItemVariant(variantIdentifier);

            var updatedVariant = await client.GetContentItemVariantAsync(variantIdentifier);

            Assert.Equal(PUBLISHED_WORKFLOW_STEP_ID, updatedVariant.WorkflowStep.Id);

            // Cleanup
            var itemToClean = Reference.ByExternalId(externalId);
            await client.DeleteContentItemAsync(itemToClean);
        }
    }
}

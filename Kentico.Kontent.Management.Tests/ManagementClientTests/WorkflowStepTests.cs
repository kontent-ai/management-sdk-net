﻿using Kentico.Kontent.Management.Models.LanguageVariants;
using Kentico.Kontent.Management.Models.Shared;
using Kentico.Kontent.Management.Models.Workflow;
using System;
using System.Linq;
using System.Reflection;
using Xunit;
using Xunit.Abstractions;
using static Kentico.Kontent.Management.Tests.ManagementClientTests.Scenario;

namespace Kentico.Kontent.Management.Tests.ManagementClientTests
{
    [Trait("ManagementClient", "Workflow")]
    public class WorkflowStepTests
    {
        private readonly ManagementClient _client;
        private readonly Scenario _scenario;

        public WorkflowStepTests(ITestOutputHelper output)
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
        public async void ListWorkflowSteps_ListsWorkflowSteps()
        {
            var response = await _client.ListWorkflowStepsAsync();

            Assert.NotNull(response);
            Assert.NotNull(response.FirstOrDefault(x => x.Id == PUBLISHED_WORKFLOW_STEP_ID));
        }

        [Fact]
        public async void ChangeLanguageVariantWorkflowStep_ChangesWorkflowStepOfVariant()
        {
            // Arrange
            var externalId = "11102811b05f429284006ea94c68333";
            var preparedItem = await TestUtils.PrepareTestItem(_client, EXISTING_CONTENT_TYPE_CODENAME, externalId);
            _ = await TestUtils.PrepareTestVariant(_client, EXISTING_LANGUAGE_CODENAME, Elements, preparedItem);

            // Test
            var itemIdentifier = Reference.ByExternalId(externalId);
            var languageIdentifier = Reference.ByCodename(EXISTING_LANGUAGE_CODENAME);
            var variantIdentifier = new LanguageVariantIdentifier(itemIdentifier, languageIdentifier);

            var identifier = new WorkflowIdentifier(itemIdentifier, languageIdentifier, Reference.ByCodename("test"));

            await _client.ChangeLanguageVariantWorkflowStep(identifier);

            var updatedVariant = await _client.GetLanguageVariantAsync(variantIdentifier);
            _ = await _client.ListWorkflowStepsAsync();

            Assert.Equal(CUSTOM_WORKFLOW_STEP_ID, updatedVariant.WorkflowStep.Id);

            // Cleanup
            var itemToClean = Reference.ByExternalId(externalId);
            await _client.DeleteContentItemAsync(itemToClean);
        }

        [Fact]
        public async void PublishVariant_PublishesVariant()
        {
            // Arrange
            var externalId = "22202811b05f429284006ea94c68333";
            var preparedItem = await TestUtils.PrepareTestItem(_client, EXISTING_CONTENT_TYPE_CODENAME, externalId);
            _ = await TestUtils.PrepareTestVariant(_client, EXISTING_LANGUAGE_CODENAME, Elements, preparedItem);

            // Test
            var itemIdentifier = Reference.ByExternalId(externalId);
            var languageIdentifier = Reference.ByCodename(EXISTING_LANGUAGE_CODENAME);
            var variantIdentifier = new LanguageVariantIdentifier(itemIdentifier, languageIdentifier);

            await _client.PublishLanguageVariant(variantIdentifier);

            var updatedVariant = await _client.GetLanguageVariantAsync(variantIdentifier);

            Assert.Equal(PUBLISHED_WORKFLOW_STEP_ID, updatedVariant.WorkflowStep.Id);

            // Cleanup
            var itemToClean = Reference.ByExternalId(externalId);
            await _client.DeleteContentItemAsync(itemToClean);
        }

        [Fact]
        public async void UnpublishVariant_UnpublishesVariant()
        {
            // Arrange
            var externalId = "78902811b05f429284006ea94c68333";
            var preparedItem = await TestUtils.PrepareTestItem(_client, EXISTING_CONTENT_TYPE_CODENAME, externalId);
            _ = await TestUtils.PrepareTestVariant(_client, EXISTING_LANGUAGE_CODENAME, Elements, preparedItem);

            // Test
            var itemIdentifier = Reference.ByExternalId(externalId);
            var languageIdentifier = Reference.ByCodename(EXISTING_LANGUAGE_CODENAME);
            var variantIdentifier = new LanguageVariantIdentifier(itemIdentifier, languageIdentifier);

            await _client.PublishLanguageVariant(variantIdentifier);

            await _client.UnpublishLanguageVariant(variantIdentifier);

            var updatedVariant = await _client.GetLanguageVariantAsync(variantIdentifier);

            Assert.Equal(ARCHIVED_WORKFLOW_STEP_ID, updatedVariant.WorkflowStep.Id);

            // Cleanup
            var itemToClean = Reference.ByExternalId(externalId);
            await _client.DeleteContentItemAsync(itemToClean);
        }

        [Fact]
        public async void CreateNewVersionsOfVariant_CreatesNewVersionsOfVariant()
        {
            // Arrange
            var externalId = "33302811b05f429284006ea94c68333";
            var preparedItem = await TestUtils.PrepareTestItem(_client, EXISTING_CONTENT_TYPE_CODENAME, externalId);
            _ = await TestUtils.PrepareTestVariant(_client, EXISTING_LANGUAGE_CODENAME, Elements, preparedItem);

            // Test
            var itemIdentifier = Reference.ByExternalId(externalId);
            var languageIdentifier = Reference.ByCodename(EXISTING_LANGUAGE_CODENAME);
            var variantIdentifier = new LanguageVariantIdentifier(itemIdentifier, languageIdentifier);

            await _client.PublishLanguageVariant(variantIdentifier);
            await _client.CreateNewVersionOfLanguageVariant(variantIdentifier);

            var updatedVariant = await _client.GetLanguageVariantAsync(variantIdentifier);

            Assert.Equal(DRAFT_WORKFLOW_STEP_ID, updatedVariant.WorkflowStep.Id);

            // Cleanup
            var itemToClean = Reference.ByExternalId(externalId);
            await _client.DeleteContentItemAsync(itemToClean);
        }

        [Fact]
        public async void SchedulePublishingOfVariant_SchedulesPublishingOfVariant()
        {
            // Arrange
            var externalId = "32302811b05f429284006ea94c68333";
            var preparedItem = await TestUtils.PrepareTestItem(_client, EXISTING_CONTENT_TYPE_CODENAME, externalId);
            _ = await TestUtils.PrepareTestVariant(_client, EXISTING_LANGUAGE_CODENAME, Elements, preparedItem);

            // Test
            var itemIdentifier = Reference.ByExternalId(externalId);
            var languageIdentifier = Reference.ByCodename(EXISTING_LANGUAGE_CODENAME);
            var variantIdentifier = new LanguageVariantIdentifier(itemIdentifier, languageIdentifier);

            await _client.SchedulePublishingOfLanguageVariant(variantIdentifier, new ScheduleModel { ScheduleTo = new DateTimeOffset(2050, 1, 1, 10, 10, 0, TimeSpan.FromHours(2)) });

            var updatedVariant = await _client.GetLanguageVariantAsync(variantIdentifier);

            Assert.Equal(SCHEDULED_WORKFLOW_STEP_ID, updatedVariant.WorkflowStep.Id);

            // Cleanup
            var itemToClean = Reference.ByExternalId(externalId);
            await _client.DeleteContentItemAsync(itemToClean);
        }

        [Fact]
        public async void ScheduleUnpublishingOfVariant_SchedulesUnpublishingOfVariant()
        {
            // Arrange
            var externalId = "35302811b05f429284006ea94c68333";
            var preparedItem = await TestUtils.PrepareTestItem(_client, EXISTING_CONTENT_TYPE_CODENAME, externalId);
            _ = await TestUtils.PrepareTestVariant(_client, EXISTING_LANGUAGE_CODENAME, Elements, preparedItem);

            // Test
            var itemIdentifier = Reference.ByExternalId(externalId);
            var languageIdentifier = Reference.ByCodename(EXISTING_LANGUAGE_CODENAME);
            var variantIdentifier = new LanguageVariantIdentifier(itemIdentifier, languageIdentifier);

            await _client.PublishLanguageVariant(variantIdentifier);

            await _client.ScheduleUnpublishingOfLanguageVariant(variantIdentifier, new ScheduleModel { ScheduleTo = new DateTimeOffset(2050, 1, 1, 10, 10, 0, TimeSpan.FromHours(2)) });

            var updatedVariant = await _client.GetLanguageVariantAsync(variantIdentifier);

            Assert.Equal(PUBLISHED_WORKFLOW_STEP_ID, updatedVariant.WorkflowStep.Id);

            // Cleanup
            await _client.UnpublishLanguageVariant(variantIdentifier);
            var itemToClean = Reference.ByExternalId(externalId);
            await _client.DeleteContentItemAsync(itemToClean);
        }

        [Fact]
        public async void CancelPublishingOfVariant_CancelsPublishingOfVariant()
        {
            // Arrange
            var externalId = "37302811b05f429284006ea94c68333";
            var preparedItem = await TestUtils.PrepareTestItem(_client, EXISTING_CONTENT_TYPE_CODENAME, externalId);
            _ = await TestUtils.PrepareTestVariant(_client, EXISTING_LANGUAGE_CODENAME, Elements, preparedItem);

            // Test
            var itemIdentifier = Reference.ByExternalId(externalId);
            var languageIdentifier = Reference.ByCodename(EXISTING_LANGUAGE_CODENAME);
            var variantIdentifier = new LanguageVariantIdentifier(itemIdentifier, languageIdentifier);

            await _client.SchedulePublishingOfLanguageVariant(variantIdentifier, new ScheduleModel { ScheduleTo = new DateTimeOffset(2050, 1, 1, 10, 10, 0, TimeSpan.FromHours(2)) });
            await _client.CancelPublishingOfLanguageVariant(variantIdentifier);

            var updatedVariant = await _client.GetLanguageVariantAsync(variantIdentifier);

            Assert.Equal(DRAFT_WORKFLOW_STEP_ID, updatedVariant.WorkflowStep.Id);

            // Cleanup
            var itemToClean = Reference.ByExternalId(externalId);
            await _client.DeleteContentItemAsync(itemToClean);
        }

        [Fact]
        public async void CancelUnpublishingOfVariant_CancelsUnpublishingOfVariant()
        {
            // Arrange
            var externalId = "37302811b05f429284006ea94c68333";
            var preparedItem = await TestUtils.PrepareTestItem(_client, EXISTING_CONTENT_TYPE_CODENAME, externalId);
            _ = await TestUtils.PrepareTestVariant(_client, EXISTING_LANGUAGE_CODENAME, Elements, preparedItem);

            // Test
            var itemIdentifier = Reference.ByExternalId(externalId);
            var languageIdentifier = Reference.ByCodename(EXISTING_LANGUAGE_CODENAME);
            var variantIdentifier = new LanguageVariantIdentifier(itemIdentifier, languageIdentifier);

            await _client.PublishLanguageVariant(variantIdentifier);
            await _client.ScheduleUnpublishingOfLanguageVariant(variantIdentifier, new ScheduleModel { ScheduleTo = new DateTimeOffset(2050,1,1,10,10,0, TimeSpan.FromHours(2)) });
            await _client.CancelUnpublishingOfLanguageVariant(variantIdentifier);

            var updatedVariant = await _client.GetLanguageVariantAsync(variantIdentifier);

            Assert.Equal(PUBLISHED_WORKFLOW_STEP_ID, updatedVariant.WorkflowStep.Id);

            // Cleanup
            var itemToClean = Reference.ByExternalId(externalId);
            await _client.DeleteContentItemAsync(itemToClean);
        }
    }
}

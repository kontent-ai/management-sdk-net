using Kentico.Kontent.Management.Models.LanguageVariants;
using Kentico.Kontent.Management.Models.Shared;
using Kentico.Kontent.Management.Models.Workflow;
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
            Assert.NotNull(response.FirstOrDefault(x => x.Id == EXISTING_WORKFLOW_STEP_ID));
        }

        [Fact]
        [Trait("Category", "Workflow")]
        // todo refactor this test
        public async void ChangeWorkflowStepOfVariant_ChangesWorkflowStepOfVariant()
        {
            var client = CreateManagementClient(nameof(ChangeWorkflowStepOfVariant_ChangesWorkflowStepOfVariant));

            // Arrange
            var externalId = "73e02811b05f429284006ea94c68333";
            var preparedItem = await TestUtils.PrepareTestItem(client, EXISTING_CONTENT_TYPE_CODENAME, externalId);

            // Test
            var contentItemVariantUpsertModel = new ContentItemVariantUpsertModel() { Elements = _elements };

            var itemIdentifier = Reference.ByExternalId(externalId);
            var languageIdentifier = Reference.ById(EXISTING_LANGUAGE_ID);
            var variantIdentifier = new ContentItemVariantIdentifier(itemIdentifier, languageIdentifier);

            var responseVariant = await client.UpsertContentItemVariantAsync(variantIdentifier, contentItemVariantUpsertModel);


            var identifier = new WorkflowIdentifier(itemIdentifier, languageIdentifier, NoExternalIdIdentifier.ByCodename("test"));

            await client.ChangeWorkflowStep(identifier);

            var updatedVariant = await client.GetContentItemVariantAsync(variantIdentifier);
            var workflowSteps = await client.ListWorkflowStepsAsync();

            Assert.Equal(workflowSteps.FirstOrDefault(x => x.Codename == "test").Id, updatedVariant.WorkflowStep.Id);

            // Cleanup
            var itemToClean = Reference.ByExternalId(externalId);
            await client.DeleteContentItemAsync(itemToClean);
        }
    }
}

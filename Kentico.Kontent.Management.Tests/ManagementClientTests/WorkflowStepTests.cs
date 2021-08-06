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
        public async void ChangeWorkflowStepOfVariant_ChangesWorkflowStepOfVariant()
        {
            var client = CreateManagementClient(nameof(ChangeWorkflowStepOfVariant_ChangesWorkflowStepOfVariant));

            var response = await client.ListWorkflowStepsAsync();

            Assert.NotNull(response);
            Assert.NotNull(response.FirstOrDefault(x => x.Id == EXISTING_WORKFLOW_STEP_ID));
        }
    }
}

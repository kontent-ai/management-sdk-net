using Kentico.Kontent.Management.Models.ProjectReport;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using static Kentico.Kontent.Management.Tests.ManagementClientTests.Scenario;

namespace Kentico.Kontent.Management.Tests.ManagementClientTests
{
    [Trait("ManagementClient", "Validation")]
    public class ValidationTests
    {
        private readonly IManagementClient _client;
        private readonly Scenario _scenario;

        public ValidationTests(ITestOutputHelper output)
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
        public async Task ValidateProject_ReturnsProjectReportModel()
        {
            var responseElementIssueMessage = "Element 'Related articles' is required but has no value";

            var project = new Project
            {
                Id = PROJECT_ID,
                Name = ".NET MAPI V2 SDK Tests"
            };

            var itemMetadata = new Metadata
            {
                Id = new Guid("deee0b3c-7b3c-4841-a603-5ada23f550fd"),
                Name = "Coffee Beverages Explained",
                Codename = "coffee_beverages_explained"
            };

            var languageMetadata = new Metadata
            {
                Id = EXISTING_LANGUAGE_ID,
                Name = "Spanish (Spain)",
                Codename = "es-ES"
            };

            var elementMetadata = new Metadata
            {
                Id = new Guid("77108990-3c30-5ffb-8dcd-8eb85fc52cb1"),
                Name = "Related articles",
                Codename = "related_articles"
            };

            var typeMessage = "Element 'To delete' contains references to non-existing taxonomy group with ID fc563f94-26a2-456f-967c-d130e68c07d8.";

            var typeMetadata = new Metadata
            {
                Id = new Guid("cb484d32-414d-4b76-bd69-5578cffd1571"),
                Name = "With deleted taxonomy",
                Codename = "with_deleted_taxonomy"
            };

            var response = await _client.ValidateProjectAsync();

            Assert.Equal(project.Id, response.Project.Id);
            Assert.Equal(project.Name, response.Project.Name);
            Assert.NotEmpty(response.VariantIssues);

            // select issue we are interested in
            var relatedArticlesVariantIssue = response.VariantIssues.Where(i => i.Item.Id == itemMetadata.Id && i.Language.Id == EXISTING_LANGUAGE_ID).FirstOrDefault();
            AssertMetadataEqual(itemMetadata, relatedArticlesVariantIssue.Item);
            AssertMetadataEqual(languageMetadata, relatedArticlesVariantIssue.Language);
            Assert.NotEmpty(relatedArticlesVariantIssue.Issues);

            var firstResponseElementIssue = relatedArticlesVariantIssue.Issues.First();
            AssertMetadataEqual(elementMetadata, firstResponseElementIssue.Element);
            Assert.Contains(responseElementIssueMessage, firstResponseElementIssue.Messages.First());

            var typeIssue = response.TypeIssues.First();
            var issue = typeIssue.Issues.First();

            AssertMetadataEqual(typeMetadata, response.TypeIssues.First().Type);
            Assert.Equal(typeMessage, issue.Messages.First());
        }

        private static void AssertMetadataEqual(Metadata expected, Metadata actual)
        {
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Codename, actual.Codename);
        }
    }
}

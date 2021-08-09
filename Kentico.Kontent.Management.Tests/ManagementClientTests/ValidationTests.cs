using Kentico.Kontent.Management.Models.ProjectReport;
using System;
using System.Linq;
using Xunit;

namespace Kentico.Kontent.Management.Tests.ManagementClientTests
{
    partial class ManagementClientTests
    {
        [Fact]
        [Trait("Category", "Validation")]
        public async void ValidateProject_ReturnsProjectReportModel()
        {
            var responseElementIssueMessage = "Element 'Related articles' is required but has no value";

            var project = new Project
            {
                Id = _options.ProjectId,
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

            var client = CreateManagementClient(nameof(ValidateProject_ReturnsProjectReportModel));
            var response = await client.ValidateProjectAsync();

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
        }

        private static void AssertMetadataEqual(Metadata expected, Metadata actual)
        {
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Codename, actual.Codename);
        }
    }
}

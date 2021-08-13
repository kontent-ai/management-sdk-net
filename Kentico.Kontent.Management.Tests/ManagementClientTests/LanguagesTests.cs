using Kentico.Kontent.Management.Models;
using Kentico.Kontent.Management.Models.Languages;
using Kentico.Kontent.Management.Models.Shared;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Kentico.Kontent.Management.Tests.ManagementClientTests
{
    public partial class ManagementClientTests
    {
        [Fact]
        [Trait("Category", "Languages")]
        public async void ListLanguages_ListsLanguages()
        {
            var client = CreateManagementClient();

            var response = await client.ListLanguagesAsync();

            Assert.NotNull(response);
            Assert.NotNull(response.FirstOrDefault(x => x.Codename == EXISTING_LANGUAGE_CODENAME));
        }

        [Fact]
        [Trait("Category", "Languages")]
        public async void GetLanguage_ById_GetsLanguages()
        {
            var client = CreateManagementClient();

            var identifier = Reference.ById(EXISTING_LANGUAGE_ID);

            var response = await client.GetLanguageAsync(identifier);
            Assert.Equal(EXISTING_LANGUAGE_ID, response.Id);
        }

        [Fact]
        [Trait("Category", "Languages")]
        public async void GetLanguages_ByCodename_GetsLanguages()
        {
            var client = CreateManagementClient();

            var identifier = Reference.ByCodename(EXISTING_LANGUAGE_CODENAME);

            var response = await client.GetLanguageAsync(identifier);
            Assert.Equal(EXISTING_LANGUAGE_CODENAME, response.Codename);
        }

        [Fact]
        [Trait("Category", "Languages")]
        public async void GetLanguages_ByExternalId_GetsLanguages()
        {
            var externalid = "standard_german";
            var client = CreateManagementClient();

            var identifier = Reference.ByExternalId(externalid);

            var response = await client.GetLanguageAsync(identifier);
            Assert.Equal(externalid, response.ExternalId);
        }

        [Fact(Skip = "Language cannot be deleted")]
        [Trait("Category", "Languages")]
        public async void CreateLanguage_CreatesLanguage()
        {
            //language can't be deleted...
            if(_runType != TestUtils.TestRunType.MockFromFileSystem)
            {
                return;
            }

            var client = CreateManagementClient();

            var newLanguage = new LanguageCreateModel
            {
                Name = "German (Germany)",
                Codename = "de-DE",
                IsActive = false,
                ExternalId = "standard_german",
                FallbackLanguage = Reference.ByCodename("en-US")
            };

            var response = await client.CreateLanguageAsync(newLanguage);

            Assert.Equal(newLanguage.Name, response.Name);
            Assert.Equal(newLanguage.Codename, response.Codename);
            Assert.Equal(newLanguage.ExternalId, response.ExternalId);
        }

        [Fact]
        [Trait("Category", "Languages")]
        public async void ModifyLanguages_Replace_ModifiesLanguages()
        {
            //Arrange
            var client = CreateManagementClient();

            var newCodename = "new codename";

            var patchModel = new LanguagePatchModel
            {
                PropertyName = LanguangePropertyName.Codename,
                Value = newCodename
            };

            //act
            var modifiedLanguange = await client.ModifyLanguageAsync(Reference.ByCodename(EXISTING_LANGUAGE_CODENAME), new List<LanguagePatchModel> { patchModel });


            //assert
            Assert.Equal(newCodename, modifiedLanguange.Codename);


            // Cleanup
            patchModel.Value = EXISTING_LANGUAGE_CODENAME;
            await client.ModifyLanguageAsync(Reference.ByCodename(newCodename), new List<LanguagePatchModel> { patchModel });
        }
    }
}

using Kentico.Kontent.Management.Models;
using Kentico.Kontent.Management.Models.Languages;
using Kentico.Kontent.Management.Models.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;
using Xunit.Abstractions;
using static Kentico.Kontent.Management.Tests.ManagementClientTests.Scenario;

namespace Kentico.Kontent.Management.Tests.ManagementClientTests
{
    [Trait("ManagementClient", "Languages")]
    public class LanguageTests
    {
        private ManagementClient _client;
        private Scenario _scenario;

        public LanguageTests(ITestOutputHelper output)
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
        public async void ListLanguages_ListsLanguages()
        {
            var response = await _client.ListLanguagesAsync();

            Assert.NotNull(response);
            Assert.NotNull(response.FirstOrDefault(x => x.Codename == EXISTING_LANGUAGE_CODENAME));
        }

        [Fact]
        public async void GetLanguage_ById_GetsLanguages()
        {
            var identifier = Reference.ById(EXISTING_LANGUAGE_ID);

            var response = await _client.GetLanguageAsync(identifier);
            Assert.Equal(EXISTING_LANGUAGE_ID, response.Id);
        }

        [Fact]
        public async void GetLanguages_ByCodename_GetsLanguages()
        {
            var identifier = Reference.ByCodename(EXISTING_LANGUAGE_CODENAME);

            var response = await _client.GetLanguageAsync(identifier);
            Assert.Equal(EXISTING_LANGUAGE_CODENAME, response.Codename);
        }

        [Fact]
        public async void GetLanguages_ByExternalId_GetsLanguages()
        {
            var externalid = "standard_german";
            var identifier = Reference.ByExternalId(externalid);

            var response = await _client.GetLanguageAsync(identifier);
            Assert.Equal(externalid, response.ExternalId);
        }

        [Fact(Skip = "Language cannot be deleted")]
        public async void CreateLanguage_CreatesLanguage()
        {
            //language can't be deleted...
            if(_scenario.RunType != TestUtils.TestRunType.MockFromFileSystem)
            {
                return;
            }

            var newLanguage = new LanguageCreateModel
            {
                Name = "German (Germany)",
                Codename = "de-DE",
                IsActive = false,
                ExternalId = "standard_german",
                FallbackLanguage = Reference.ByCodename("en-US")
            };

            var response = await _client.CreateLanguageAsync(newLanguage);

            Assert.Equal(newLanguage.Name, response.Name);
            Assert.Equal(newLanguage.Codename, response.Codename);
            Assert.Equal(newLanguage.ExternalId, response.ExternalId);
        }

        [Fact]
        public async void ModifyLanguages_Replace_ModifiesLanguages()
        {
            //Arrange
            var newCodename = "new codename";

            var patchModel = new LanguagePatchModel
            {
                PropertyName = LanguangePropertyName.Codename,
                Value = newCodename
            };

            //act
            var modifiedLanguange = await _client.ModifyLanguageAsync(Reference.ByCodename(EXISTING_LANGUAGE_CODENAME), new List<LanguagePatchModel> { patchModel });


            //assert
            Assert.Equal(newCodename, modifiedLanguange.Codename);


            // Cleanup
            patchModel.Value = EXISTING_LANGUAGE_CODENAME;
            await _client.ModifyLanguageAsync(Reference.ByCodename(newCodename), new List<LanguagePatchModel> { patchModel });
        }
    }
}

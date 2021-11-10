using System;
using FluentAssertions;
using FluentAssertions.Execution;
using Kentico.Kontent.Management.Models.Languages;
using Kentico.Kontent.Management.Models.Shared;
using Kentico.Kontent.Management.Tests.Unit.Base;
using Xunit;

namespace Kentico.Kontent.Management.Tests.Unit.ManagementClientTests
{
    public class LanguagesTests : IClassFixture<FileSystemFixture>
    {
        private FileSystemFixture _fileSystemFixture;

        public LanguagesTests(FileSystemFixture fileSystemFixture)
        {
            _fileSystemFixture = fileSystemFixture;
        }

        [Fact]
        public async void CreateLanguage_CreatesLanguage()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("CreateLanguage_CreatesLanguage.json");

            var newLanguage = new LanguageCreateModel
            {
                Name = "German (Germany)",
                Codename = "de-DE",
                IsActive = false,
                ExternalId = "standard-german",
                FallbackLanguage = Reference.ById(Guid.Parse("00000000-0000-0000-0000-000000000000"))
            };

            var response = await client.CreateLanguageAsync(newLanguage);

            using (new AssertionScope())
            {
                response.Name.Should().BeEquivalentTo(newLanguage.Name);
                response.Codename.Should().BeEquivalentTo(newLanguage.Codename);
                response.ExternalId.Should().BeEquivalentTo(newLanguage.ExternalId);
                response.FallbackLanguage.Id.Should().Equals(newLanguage.FallbackLanguage.Id);
            }
        }

        [Fact]
        public async void ListLanguages_ListsLanguages()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("ListLanguages_ListsLanguages.json");

            var response = await client.ListLanguagesAsync();

            Assert.Single(response, item => item.Codename == "default");
        }

        [Fact]
        public async void GetLanguage_ById_GetsLanguage()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("SingleLanguageResponse.json");

            var response = await client.GetLanguageAsync(Reference.ById(Guid.Parse("00000000-0000-0000-0000-000000000000")));

            using (new AssertionScope())
            {
                response.Name.Should().BeEquivalentTo("Default project language");
                response.Codename.Should().BeEquivalentTo("default");
                response.ExternalId.Should().BeEquivalentTo("string");
                response.FallbackLanguage.Id.Should().Equals(Guid.Parse("00000000-0000-0000-0000-000000000000"));
                response.IsActive.Should().BeTrue();
                response.IsActive.Should().BeTrue();
            }
        }

        [Fact]
        public async void GetLanguage_ByCodename_GetsLanguage()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("SingleLanguageResponse.json");

            var response = await client.GetLanguageAsync(Reference.ByCodename("default"));

            using (new AssertionScope())
            {
                response.Name.Should().BeEquivalentTo("Default project language");
                response.Codename.Should().BeEquivalentTo("default");
                response.ExternalId.Should().BeEquivalentTo("string");
                response.FallbackLanguage.Id.Should().Equals(Guid.Parse("00000000-0000-0000-0000-000000000000"));
                response.IsActive.Should().BeTrue();
                response.IsActive.Should().BeTrue();
            }
        }

        [Fact]
        public async void GetLanguage_ByExternalId_GetsLanguage()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("SingleLanguageResponse.json");

            var response = await client.GetLanguageAsync(Reference.ByExternalId("string"));

            using (new AssertionScope())
            {
                response.Name.Should().BeEquivalentTo("Default project language");
                response.Codename.Should().BeEquivalentTo("default");
                response.ExternalId.Should().BeEquivalentTo("string");
                response.FallbackLanguage.Id.Should().Equals(Guid.Parse("00000000-0000-0000-0000-000000000000"));
                response.IsActive.Should().BeTrue();
                response.IsActive.Should().BeTrue();
            }
        }

        [Fact]
        public async void ModifyLanguages_Replace_ModifiesLanguages()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("ModifyLanguages_Replace_ModifiesLanguages.json");

            var patchModel = new[]
            {
                new LanguagePatchModel
                {
                    PropertyName = LanguagePropertyName.FallbackLanguage,
                    Value = new {
                        Codename = "en-US"
                    }
                },
                new LanguagePatchModel
                {
                    PropertyName = LanguagePropertyName.Name,
                    Value = "Deutsch"
                }
            };

            var modifiedLanguage = await client.ModifyLanguageAsync(Reference.ByCodename("de-DE"), patchModel);
            using (new AssertionScope())
            {
                modifiedLanguage.Name.Should().BeEquivalentTo("Deutsch");
                modifiedLanguage.FallbackLanguage.Id.Should().Equals(Guid.Parse("00000000-0000-0000-0000-000000000000"));

            }
        }
    }
}
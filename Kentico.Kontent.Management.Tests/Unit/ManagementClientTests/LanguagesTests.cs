using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Kentico.Kontent.Management.Models.Languages;
using Kentico.Kontent.Management.Models.Shared;
using Kentico.Kontent.Management.Modules.ActionInvoker;
using Kentico.Kontent.Management.Modules.HttpClient;
using NSubstitute;
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
            var mockedHttpClient = Substitute.For<IManagementHttpClient>();

            mockedHttpClient.SendAsync(Arg.Any<IMessageCreator>(), Arg.Any<string>(), Arg.Any<HttpMethod>(), Arg.Any<HttpContent>(), Arg.Any<Dictionary<string, string>>())
             .Returns(x =>
                {
                    string dataPath = Path.Combine(Environment.CurrentDirectory, "Unit", "Data");

                    var responsePath = Path.Combine(dataPath, "CreateLanguage_CreatesLanguage.json");
                    var result = new HttpResponseMessage();
                    result.Content = new StringContent(File.ReadAllText(responsePath));

                    return Task.FromResult<HttpResponseMessage>(result);
                });
            var client = _fileSystemFixture.CreateMockClient(mockedHttpClient);
<<<<<<< HEAD
=======

>>>>>>> 52848e0c (extract common configuration to test ficture container)

            var newLanguage = new LanguageCreateModel
            {
                Name = "German (Germany)",
                Codename = "de-DE",
                IsActive = false,
                ExternalId = "standard-german",
                FallbackLanguage = Reference.ById(Guid.Parse("00000000-0000-0000-0000-000000000000"))
            };

            var response = await client.CreateLanguageAsync(newLanguage);

            Assert.Equal(newLanguage.Name, response.Name);
            Assert.Equal(newLanguage.Codename, response.Codename);
            Assert.Equal(newLanguage.ExternalId, response.ExternalId);
            Assert.Equal(newLanguage.FallbackLanguage.Id, response.FallbackLanguage.Id);
        }

        [Fact]
        public async void ListLanguages_ListsLanguages()
        {
            var mockedHttpClient = Substitute.For<IManagementHttpClient>();
            mockedHttpClient.SendAsync(Arg.Any<IMessageCreator>(), Arg.Any<string>(), Arg.Any<HttpMethod>(), Arg.Any<HttpContent>(), Arg.Any<Dictionary<string, string>>())
             .Returns(x =>
                {
                    string dataPath = Path.Combine(Environment.CurrentDirectory, "Unit", "Data");

                    var responsePath = Path.Combine(dataPath, "ListLanguages_ListsLanguages.json");
                    var result = new HttpResponseMessage();
                    result.Content = new StringContent(File.ReadAllText(responsePath));

                    return Task.FromResult<HttpResponseMessage>(result);
                });
            var client = _fileSystemFixture.CreateMockClient(mockedHttpClient);

            var response = await client.ListLanguagesAsync();

            Assert.Single(response, item => item.Codename == "default");
        }

        [Fact]
        public async void GetLanguage_ById_GetsLanguages()
        {
            var mockedHttpClient = Substitute.For<IManagementHttpClient>();
            mockedHttpClient.SendAsync(Arg.Any<IMessageCreator>(), Arg.Any<string>(), Arg.Any<HttpMethod>(), Arg.Any<HttpContent>(), Arg.Any<Dictionary<string, string>>())
             .Returns(x =>
                {
                    string dataPath = Path.Combine(Environment.CurrentDirectory, "Unit", "Data");

                    var responsePath = Path.Combine(dataPath, "SingleLanguageResponse.json");
                    var result = new HttpResponseMessage();
                    result.Content = new StringContent(File.ReadAllText(responsePath));

                    return Task.FromResult<HttpResponseMessage>(result);
                });
            var client = _fileSystemFixture.CreateMockClient(mockedHttpClient);

            var response = await client.GetLanguageAsync(Reference.ById(Guid.Parse("00000000-0000-0000-0000-000000000000")));

            Assert.Equal("Default project language", response.Name);
            Assert.Equal("default", response.Codename);
            Assert.Equal("string", response.ExternalId);
            Assert.Equal(Guid.Parse("00000000-0000-0000-0000-000000000000"), response.FallbackLanguage.Id);
            Assert.True(response.IsActive);
            Assert.True(response.IsDefault);
        }

        [Fact]
        public async void GetLanguage_ByCodename_GetsLanguages()
        {
            var mockedHttpClient = Substitute.For<IManagementHttpClient>();
            mockedHttpClient.SendAsync(Arg.Any<IMessageCreator>(), Arg.Any<string>(), Arg.Any<HttpMethod>(), Arg.Any<HttpContent>(), Arg.Any<Dictionary<string, string>>())
             .Returns(x =>
                {
                    string dataPath = Path.Combine(Environment.CurrentDirectory, "Unit", "Data");

                    var responsePath = Path.Combine(dataPath, "SingleLanguageResponse.json");
                    var result = new HttpResponseMessage();
                    result.Content = new StringContent(File.ReadAllText(responsePath));

                    return Task.FromResult<HttpResponseMessage>(result);
                });
            var client = _fileSystemFixture.CreateMockClient(mockedHttpClient);

            var response = await client.GetLanguageAsync(Reference.ByCodename("default"));

            Assert.Equal("Default project language", response.Name);
            Assert.Equal("default", response.Codename);
            Assert.Equal("string", response.ExternalId);
            Assert.Equal(Guid.Parse("00000000-0000-0000-0000-000000000000"), response.FallbackLanguage.Id);
            Assert.True(response.IsActive);
            Assert.True(response.IsDefault);
        }

        [Fact]
        public async void ModifyLanguages_Replace_ModifiesLanguages()
        {
            var mockedHttpClient = Substitute.For<IManagementHttpClient>();
            mockedHttpClient.SendAsync(Arg.Any<IMessageCreator>(), Arg.Any<string>(), Arg.Any<HttpMethod>(), Arg.Any<HttpContent>(), Arg.Any<Dictionary<string, string>>())
             .Returns(x =>
                {
                    string dataPath = Path.Combine(Environment.CurrentDirectory, "Unit", "Data");

                    var responsePath = Path.Combine(dataPath, "ModifyLanguages_Replace_ModifiesLanguages.json");
                    var result = new HttpResponseMessage();
                    result.Content = new StringContent(File.ReadAllText(responsePath));

                    return Task.FromResult<HttpResponseMessage>(result);
                });
            var client = _fileSystemFixture.CreateMockClient(mockedHttpClient);

            var patchModel = new[]
            {
                new LanguagePatchModel
                {
                    PropertyName = LanguagePropertyName.FallbackLanguage,
                    // TODO is the format OK?
                    Value = new {
                        codename = "en-US"
                    }
                },
                new LanguagePatchModel
                {
                    PropertyName = LanguagePropertyName.Name,
                    Value = "Deutsch"
                }
            };

            var modifiedLanguage = await client.ModifyLanguageAsync(Reference.ByCodename("de-DE"), patchModel);

            Assert.Equal("Deutsch", modifiedLanguage.Name);
            Assert.Equal(Guid.Parse("00000000-0000-0000-0000-000000000000"), modifiedLanguage.FallbackLanguage.Id);
        }
    }
}
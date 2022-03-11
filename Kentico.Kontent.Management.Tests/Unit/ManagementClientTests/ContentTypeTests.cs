using Kentico.Kontent.Management.Models.Types;
using Kentico.Kontent.Management.Tests.Unit.Base;
using Kentico.Kontent.Management.Extensions;
using Xunit;
using FluentAssertions;
using Kentico.Kontent.Management.Models.Shared;
using System;
using Kentico.Kontent.Management.Models.Types.Patch;
using System.Collections.Generic;
using Kentico.Kontent.Management.Models.Types.Elements;

namespace Kentico.Kontent.Management.Tests.Unit.ManagementClientTests
{
    public class ContentTypeTests : IClassFixture<FileSystemFixture>
    {
        private readonly FileSystemFixture _fileSystemFixture;

        public ContentTypeTests(FileSystemFixture fileSystemFixture)
        {
            _fileSystemFixture = fileSystemFixture;
            _fileSystemFixture.SetSubFolder("ContentType");
        }

        [Fact]
        public async void ListContentTypesAsync_WithContinuation_ListsContentTypes()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("ContentTypesPage1.json", "ContentTypesPage2.json", "ContentTypesPage3.json");

            var expected = _fileSystemFixture.GetItemsOfExpectedListingResponse<ContentTypeModel>("ContentTypesPage1.json", "ContentTypesPage2.json", "ContentTypesPage3.json");

            var response = await client.ListContentTypesAsync().GetAllAsync();

            response.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async void GetContentTypeAsync_GetsContentTypeAsync()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("ContentType.json");

            var expected = _fileSystemFixture.GetExpectedResponse<ContentTypeModel>("ContentType.json");

            var response = await client.GetContentTypeAsync(Reference.ByCodename("codename"));

            response.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async void GetContentTypeAsync_IdentifierIsNull_Throws()
        {
            var client = _fileSystemFixture.CreateMockClientWithoutResponse();

            await client.Invoking(x => x.GetContentTypeAsync(null)).Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async void CreateContentTypeAsync_CreatesContentTypeAsync()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("ContentType.json");

            var expected = _fileSystemFixture.GetExpectedResponse<ContentTypeModel>("ContentType.json");

            var response = await client.CreateContentTypeAsync(new ContentTypeCreateModel
            {
                Codename = expected.Codename,
                ContentGroups = expected.ContentGroups,
                Elements = expected.Elements,
                ExternalId = expected.ExternalId,
                Name = expected.Name
            });

            response.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async void CreateContentTypeAsync_CreateModelIsNull_Throws()
        {
            var client = _fileSystemFixture.CreateMockClientWithoutResponse();

            await client.Invoking(x => x.CreateContentTypeAsync(null)).Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async void DeleteContentTypeAsync_DeletesContentTypeAsync()
        {
            var client = _fileSystemFixture.CreateMockClientWithoutResponse();

            await client.Invoking(x => x.DeleteContentTypeAsync(Reference.ByCodename("codename"))).Should().NotThrowAsync();
        }

        [Fact]
        public async void DeleteContentTypeAsync_IdentifierIsNull_Throws()
        {
            var client = _fileSystemFixture.CreateMockClientWithoutResponse();

            await client.Invoking(x => x.DeleteContentTypeAsync(null)).Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async void ModifyContentTypeAsync_ModifiesContentType()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("ContentType.json");

            var expected = _fileSystemFixture.GetExpectedResponse<ContentTypeModel>("ContentType.json");

            List<ContentTypeOperationBaseModel> changes = new()
            {
                new ContentTypeRemovePatchModel
                {
                    Path = $"/elements/codename:none"
                },
                new ContentTypeReplacePatchModel
                {
                    Value = "bla bla bla",
                    Path = $"/elements/codename:display_options/guidelines"
                },
                new ContentTypeAddIntoPatchModel
                {
                    Value = new TextElementMetadataModel
                    {
                        Name = "Tweet link",
                        Guidelines = "new guidelines",
                        IsRequired = true,
                        ExternalId = "20bf9ba1-28fe-203c-5920-6f9610498fb9",
                        Codename = "tweet_link",
                        MaximumTextLength = null
                    },
                    Before = Reference.ByCodename("theme"),
                    Path = "/elements"
                }
            };

            var response = await client.ModifyContentTypeAsync(Reference.ByCodename("tweet"), changes);

            response.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async void ModifyContentTypeAsync_IdentifierIsNull_ModifiesContentType()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("ContentType.json");

            var expected = _fileSystemFixture.GetExpectedResponse<ContentTypeModel>("ContentType.json");

            List<ContentTypeOperationBaseModel> changes = new() { };

            await client.Invoking(x => x.ModifyContentTypeAsync(null, changes)).Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async void ModifyContentTypeAsync_ChangesAreNull_ModifiesContentType()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("ContentType.json");

            var expected = _fileSystemFixture.GetExpectedResponse<ContentTypeModel>("ContentType.json");

            await client.Invoking(x => x.ModifyContentTypeAsync(Reference.ByCodename("tweet"), null)).Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async void ModifyContentTypeAsync_NoChanges_ModifiesContentType()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("ContentType.json");

            var expected = _fileSystemFixture.GetExpectedResponse<ContentTypeModel>("ContentType.json");

            await client.Invoking(x => x.ModifyContentTypeAsync(Reference.ByCodename("tweet"), new List<ContentTypeOperationBaseModel> { }))
                .Should().ThrowAsync<ArgumentException>();
        }
    }
}

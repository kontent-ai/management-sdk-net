using FluentAssertions;
using Kentico.Kontent.Management.Extensions;
using Kentico.Kontent.Management.Models.Shared;
using Kentico.Kontent.Management.Models.Types.Elements;
using Kentico.Kontent.Management.Models.TypeSnippets;
using Kentico.Kontent.Management.Models.TypeSnippets.Patch;
using Kentico.Kontent.Management.Tests.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Kentico.Kontent.Management.Tests.ManagementClientTests;

public class ContentTypeSnippetTests : IClassFixture<FileSystemFixture>
{
    private readonly FileSystemFixture _fileSystemFixture;

    public ContentTypeSnippetTests(FileSystemFixture fileSystemFixture)
    {
        _fileSystemFixture = fileSystemFixture;
        _fileSystemFixture.SetSubFolder("ContentTypeSnippet");
    }

    [Fact]
    public async Task ListContentTypeSnippetsAsync_WithContinuation_ListsSnippets()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("SnippetsPage1.json", "SnippetsPage2.json", "SnippetsPage3.json");

        var expected = _fileSystemFixture.GetItemsOfExpectedListingResponse<ContentTypeSnippetModel>("SnippetsPage1.json", "SnippetsPage2.json", "SnippetsPage3.json");

        var response = await client.ListContentTypeSnippetsAsync().GetAllAsync();

        response.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task GetContentTypeSnippetAsync_GetsContentTypeSnippetAsync()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("Snippet.json");

        var expected = _fileSystemFixture.GetExpectedResponse<ContentTypeSnippetModel>("Snippet.json");

        var response = await client.GetContentTypeSnippetAsync(Reference.ByCodename("metadata"));

        response.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async void GetContentTypeSnippetAsync_IdentifierIsNull_Throws()
    {
        var client = _fileSystemFixture.CreateMockClientWithoutResponse();

        await client.Invoking(x => x.GetContentTypeSnippetAsync(null)).Should().ThrowAsync<ArgumentNullException>();
    }


    [Fact]
    public async void CreateContentTypeSnippetAsync_CreatesContentTypeAsync()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("Snippet.json");

        var expected = _fileSystemFixture.GetExpectedResponse<ContentTypeSnippetModel>("Snippet.json");

        var response = await client.CreateContentTypeSnippetAsync(new ContentTypeSnippetCreateModel
        {
            Codename = expected.Codename,
            Elements = expected.Elements,
            ExternalId = expected.ExternalId,
            Name = expected.Name
        });

        response.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async void CreateContentTypeSnippetAsync_CreateModelIsNull_Throws()
    {
        var client = _fileSystemFixture.CreateMockClientWithoutResponse();

        await client.Invoking(x => x.CreateContentTypeSnippetAsync(null)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async void DeleteContentTypeSnippetAsync_DeletesContentTypeAsync()
    {
        var client = _fileSystemFixture.CreateMockClientWithoutResponse();

        await client.Invoking(x => x.DeleteContentTypeSnippetAsync(Reference.ByCodename("codename"))).Should().NotThrowAsync();
    }

    [Fact]
    public async void DeleteContentTypeSnippetAsync_IdentifierIsNull_Throws()
    {
        var client = _fileSystemFixture.CreateMockClientWithoutResponse();

        await client.Invoking(x => x.DeleteContentTypeSnippetAsync(null)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async void ModifyContentTypeSnippetAsync_ModifiesContentType()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("Snippet.json");

        var expected = _fileSystemFixture.GetExpectedResponse<ContentTypeSnippetModel>("Snippet.json");

        List<ContentTypeSnippetOperationBaseModel> changes = new()
        {
            new ContentTypeSnippetPatchRemoveModel
            {
                Path = $"/elements/codename:none"
            },
            new ContentTypeSnippetPatchReplaceModel
            {
                Value = "Provide all personas for which this article is relevant.",
                Path = $"/elements/codename:personas/guidelines"
            },
            new ContentTypeSnippetAddIntoPatchModel
            {
                Value = new TextElementMetadataModel
                {
                    Name = "Meta description",
                    Guidelines = "Sum up the blog for SEO purposes. Limit for the meta description is 160 characters.",
                    IsRequired = false,
                    ExternalId = "b9dc537c-2518-e4f5-8325-ce4fce26171e",
                    Codename = "meta_description",
                    MaximumTextLength = null
                },
                After = Reference.ByCodename("personas"),
                Path = "/elements"
            }
        };

        var response = await client.ModifyContentTypeSnippetAsync(Reference.ByCodename("metadata"), changes);

        response.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async void ModifyContentTypeSnippetAsync_IdentifierIsNull_Throws()
    {
        var client = _fileSystemFixture.CreateMockClientWithoutResponse();

        List<ContentTypeSnippetOperationBaseModel> changes = new()
        {
            new ContentTypeSnippetPatchRemoveModel
            {
                Path = $"/elements/codename:none"
            }
        };

        await client.Invoking(x => x.ModifyContentTypeSnippetAsync(null, changes)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async void ModifyContentTypeSnippetAsync_ChangesAreNull_Throws()
    {
        var client = _fileSystemFixture.CreateMockClientWithoutResponse();

        await client.Invoking(x => x.ModifyContentTypeSnippetAsync(Reference.ByCodename("metadata"), null)).Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async void ModifyContentTypeSnippetAsync_NoChanges_Throws()
    {
        var client = _fileSystemFixture.CreateMockClientWithoutResponse();

        await client.Invoking(x => x.ModifyContentTypeSnippetAsync(Reference.ByCodename("tweet"), new List<ContentTypeSnippetOperationBaseModel> { }))
            .Should().ThrowAsync<ArgumentException>();
    }
}

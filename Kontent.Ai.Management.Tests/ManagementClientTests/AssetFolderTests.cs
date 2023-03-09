using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Kontent.Ai.Management.Tests.Base;
using System.Net.Http;
using System.Collections.Generic;
using Kontent.Ai.Management.Models.AssetFolders;
using FluentAssertions;
using System;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.AssetFolders.Patch;
using static Kontent.Ai.Management.Tests.Base.Scenario;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class AssetFolderTests
{
    private readonly Scenario _scenario;

    public AssetFolderTests()
    {
        _scenario = new Scenario(folder: "AssetFolder");
    }

    [Fact]
    public async Task GetAssetFoldersAsync_GetsFolder()
    {
        var client = _scenario
            .WithResponses("Folder.json")
            .CreateManagementClient();


        var response = await client.GetAssetFoldersAsync();

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Get)
            .Response(response)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/folders")
            .Validate();
    }

    [Fact]
    public async Task CreateAssetFoldersAsync_CreatesFolder()
    {
        var client = _scenario
            .WithResponses("Folder.json")
            .CreateManagementClient();

        var folderModel = new AssetFolderCreateModel
        {
            Folders = new List<AssetFolderHierarchy>
            {
                new AssetFolderHierarchy
                {
                    ExternalId = "external-id",
                    Name= "name",
                    Folders = Enumerable.Empty<AssetFolderHierarchy>()
                }
            }
        };

        var response = await client.CreateAssetFoldersAsync(folderModel);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Post)
            .RequestPayload(folderModel)
            .Response(response)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/folders")
            .Validate();
    }

    [Fact]
    public async Task CreateAssetFoldersAsync_FolderModelIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(c => c.CreateAssetFoldersAsync(null)).Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async void ModifyAssetFoldersAsync_ModifiesFolder()
    {
        var client = _scenario
            .WithResponses("Folder.json")
            .CreateManagementClient();

        var changes = GetChanges();
        var response = await client.ModifyAssetFoldersAsync(changes);

        _scenario
            .CreateExpectations()
            .HttpMethod(new HttpMethod("PATCH"))
            .RequestPayload(changes)
            .Response(response)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/folders")
            .Validate();
    }

    [Fact]
    public async Task ModifyAssetFoldersAsync_ChangesAreNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(c => c.ModifyAssetFoldersAsync(null)).Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    private static List<AssetFolderOperationBaseModel> GetChanges() => new()
        {
            new AssetFolderAddIntoModel
            {
                Value = new AssetFolderHierarchy
                {
                    ExternalId = "external-id",
                    Name= "name",
                    Folders = Enumerable.Empty<AssetFolderHierarchy>()
                },
                Before = Reference.ByCodename("codename"),
                After = Reference.ById(Guid.NewGuid())
            },
            new AssetFolderRenameModel
            {
                Value = "new folder name",
            },
            new AssetFolderRemoveModel()
        };
}
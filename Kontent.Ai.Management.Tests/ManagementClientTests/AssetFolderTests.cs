using AwesomeAssertions;
using Kontent.Ai.Management.Models.AssetFolders;
using Kontent.Ai.Management.Models.AssetFolders.Patch;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Tests.Base;
using RichardSzalay.MockHttp;
using Xunit;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class AssetFolderTests
{
    private static string Folder => Fixture("Folder.json");

    private static string Fixture(string name)
        => File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Data", "AssetFolder", name));

    [Fact]
    public async Task GetAssetFoldersAsync_GetsFolder()
    {
        var (client, mock) = MockClientFactory.Create();
        mock.Expect(HttpMethod.Get, $"{MockClientFactory.BaseUrl}/folders")
            .Respond("application/json", Folder);

        var response = await client.GetAssetFoldersAsync();

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(JsonSerializer.Deserialize<AssetFoldersModel>(Folder, SharedTestJsonOptions.Default));
    }

    [Fact]
    public async Task CreateAssetFoldersAsync_CreatesFolder()
    {
        var (client, mock) = MockClientFactory.Create();
        var folderModel = new AssetFolderCreateModel
        {
            Folders = new List<AssetFolderHierarchy>
            {
                new AssetFolderHierarchy
                {
                    ExternalId = "external-id",
                    Name= "name",
                    Codename = "codename"
                }
            }
        };

        string? capturedBody = null;
        mock.Expect(HttpMethod.Post, $"{MockClientFactory.BaseUrl}/folders")
            .With(r =>
            {
                capturedBody = r.Content!.ReadAsStringAsync().GetAwaiter().GetResult();
                return true;
            })
            .Respond("application/json", Folder);

        var response = await client.CreateAssetFoldersAsync(folderModel);

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(JsonSerializer.Deserialize<AssetFoldersModel>(Folder, SharedTestJsonOptions.Default));
        capturedBody.Should().NotBeNull();
        JsonSerializer.Deserialize<AssetFolderCreateModel>(capturedBody!, SharedTestJsonOptions.Default)
            .Should().BeEquivalentTo(JsonSerializer.Deserialize<AssetFolderCreateModel>(JsonSerializer.Serialize(folderModel, SharedTestJsonOptions.Default), SharedTestJsonOptions.Default));
    }

    [Fact]
    public async Task CreateAssetFoldersAsync_FolderModelIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(c => c.CreateAssetFoldersAsync(null!)).Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task ModifyAssetFoldersAsync_ModifiesFolder()
    {
        var (client, mock) = MockClientFactory.Create();
        var changes = GetChanges();

        string? capturedBody = null;
        mock.Expect(new HttpMethod("PATCH"), $"{MockClientFactory.BaseUrl}/folders")
            .With(r =>
            {
                capturedBody = r.Content!.ReadAsStringAsync().GetAwaiter().GetResult();
                return true;
            })
            .Respond("application/json", Folder);

        var response = await client.ModifyAssetFoldersAsync(changes);

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(JsonSerializer.Deserialize<AssetFoldersModel>(Folder, SharedTestJsonOptions.Default));
        capturedBody.Should().NotBeNull();
        // Heterogeneous polymorphic operation list: deep per-field equivalence needed the demolished test-only
        // converter. Assert the part that's behaviourally meaningful and converter-free — the ordered sequence of
        // operation kinds (PATCH order matters), via each element's stable "op" discriminator.
        var sentOps = JsonNode.Parse(capturedBody!)!.AsArray().Select(t => (string?)t!["op"]);
        var expectedOps = JsonNode.Parse(JsonSerializer.Serialize(changes, SharedTestJsonOptions.Default))!.AsArray().Select(t => (string?)t!["op"]);
        sentOps.Should().Equal(expectedOps);
    }

    [Fact]
    public async Task ModifyAssetFoldersAsync_ChangesAreNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(c => c.ModifyAssetFoldersAsync(null!)).Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    private static List<AssetFolderOperationBaseModel> GetChanges() => new()
        {
            new AssetFolderAddIntoModel
            {
                Value = new AssetFolderHierarchy
                {
                    ExternalId = "external-id",
                    Name= "name",
                    Codename = "codename"
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

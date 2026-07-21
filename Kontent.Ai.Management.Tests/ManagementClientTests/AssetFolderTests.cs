using AwesomeAssertions;
using Kontent.Ai.Management.Models.AssetFolders;
using Kontent.Ai.Management.Models.AssetFolders.Patch;
using Kontent.Ai.Management.Tests.Base;
using RichardSzalay.MockHttp;
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

        var result = await client.GetAssetFoldersAsync();

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<AssetFoldersModel>(Folder, SharedTestJsonOptions.Default));
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
                    Name = "name",
                    Codename = "codename"
                }
            }
        };

        mock.Expect(HttpMethod.Post, $"{MockClientFactory.BaseUrl}/folders")
            .CaptureBody(out var capturedBody)
            .Respond("application/json", Folder);

        var result = await client.CreateAssetFoldersAsync(folderModel);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<AssetFoldersModel>(Folder, SharedTestJsonOptions.Default));
        capturedBody.ShouldMatchSerialized(folderModel);
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

        mock.Expect(new HttpMethod("PATCH"), $"{MockClientFactory.BaseUrl}/folders")
            .CaptureBody(out var capturedBody)
            .Respond("application/json", Folder);

        var result = await client.ModifyAssetFoldersAsync(changes);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<AssetFoldersModel>(Folder, SharedTestJsonOptions.Default));
        capturedBody.Value.Should().NotBeNull();
        // Heterogeneous polymorphic operation list: assert the converter-free, behaviourally meaningful part — the
        // ordered sequence of operation kinds (PATCH order matters), via each element's stable "op" discriminator.
        var sentOps = JsonNode.Parse(capturedBody.Value!)!.AsArray().Select(t => (string?)t!["op"]);
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
        new AssetFolderAddIntoPatchModel
        {
            Value = new AssetFolderHierarchy
            {
                ExternalId = "external-id",
                Name = "name",
                Codename = "codename"
            },
            Before = Reference.ByCodename("codename"),
            After = Reference.ById(Guid.NewGuid())
        },
        new AssetFolderRenamePatchModel
        {
            Value = "new folder name",
        },
        new AssetFolderRemovePatchModel()
    };
}

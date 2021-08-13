using Kentico.Kontent.Management.Models;
using Kentico.Kontent.Management.Models.Assets;
using Kentico.Kontent.Management.Models.Items;
using Kentico.Kontent.Management.Models.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Kentico.Kontent.Management.Tests.ManagementClientTests
{
    partial class ManagementClientTests
    {
        [Fact]
        [Trait("Category", "Asset")]
        public async Task ListAssets_ListsAssets()
        {
            var client = CreateManagementClient();

            var response = await client.ListAssetsAsync();
            Assert.NotNull(response);
            Assert.NotNull(response.FirstOrDefault());
        }

        [Fact]
        [Trait("Category", "Asset")]
        public async Task ListFolders_ListFolders()
        {
            var client = CreateManagementClient();

            var response = await client.GetAssetFoldersAsync();
            Assert.NotNull(response);
            Assert.True(response.Folders.Count() > 0);
        }

        [Fact]
        [Trait("Category", "Asset")]
        public async Task ListFolders_GetFolderLinkedTree()
        {
            var client = CreateManagementClient();

            var response = await client.GetAssetFoldersAsync();
            var linkedHierarchy = response.Folders.GetParentLinkedFolderHierarchy();

            Assert.NotNull(response);
            Assert.True(response.Folders.Count() > 0);
        }


        [Fact]
        [Trait("Category", "Asset")]
        public async Task ListFolders_GetFolderLinkedTreeSearchByFolderId()
        {
            var client = CreateManagementClient();

            var response = await client.GetAssetFoldersAsync();
            var linkedHierarchy = response.Folders.GetParentLinkedFolderHierarchy();
            var result = linkedHierarchy.GetParentLinkedFolderHierarchyById(ASSET_FOLDER_ID_1ST_LEVEL);
            var result2 = linkedHierarchy.GetParentLinkedFolderHierarchyById(ASSET_FOLDER_ID_2ND_LEVEL);
            var result3 = linkedHierarchy.GetParentLinkedFolderHierarchyById(ASSET_FOLDER_ID_3RD_LEVEL);
            var result4 = linkedHierarchy.GetParentLinkedFolderHierarchyById(ASSET_FOLDER_ID_4TH_LEVEL);

            Assert.NotNull(response);
            Assert.NotNull(result);
            Assert.NotNull(result2);
            Assert.NotNull(result3);
            Assert.NotNull(result4);
            Assert.Equal("TopFolder", result.Name);
            Assert.Equal("2ndFolder", result2.Name);
            Assert.Equal("3rdFolder", result3.Name);
            Assert.Equal("4thFolder", result4.Name);
        }

        [Fact]
        [Trait("Category", "Asset")]
        public async Task ListFolders_GetFolderHierarchy_NonExistingFolder()
        {
            var client = CreateManagementClient();

            var response = await client.GetAssetFoldersAsync();
            var nonExistingFolderId = "2ddaf2dc-8635-4b3f-b04d-5be69a0949e6";
            var result = response.Folders.GetFolderHierarchyById(nonExistingFolderId);

            Assert.Null(result);
        }

        [Fact()]
        [Trait("Category", "Asset")]
        public async Task ListFolders_GetFolderHierarchy_ExistingFolder()
        {
            var client = CreateManagementClient();

            var response = await client.GetAssetFoldersAsync();
            var result = response.Folders.GetFolderHierarchyById(ASSET_FOLDER_ID_4TH_LEVEL);

            Assert.NotNull(result);
            Assert.True(result.Name == "4thFolder");
        }

        [Fact]
        [Trait("Category", "Asset")]
        public async Task ListFolders_GetFolderPathString()
        {
            var client = CreateManagementClient();

            var response = await client.GetAssetFoldersAsync();
            var linkedHierarchy = response.Folders.GetParentLinkedFolderHierarchy();
            var result = linkedHierarchy.GetParentLinkedFolderHierarchyById("e2fe0a21-eb4c-5fba-8a28-697aeab81f83"); //Go three levels deep
            var pathString = result.GetFullFolderPath(); //Should be a folder path string TopFolder\2ndFolder\3rdFolder (3 levels deep)

            Assert.NotNull(response);
            Assert.NotNull(result);
            Assert.True(pathString == "TopFolder\\2ndFolder\\3rdFolder");
        }

        [Fact]
        [Trait("Category", "Asset")]
        public async Task ListAssets_WithContinuation_ListsAllAssets()
        {
            var client = CreateManagementClient();

            var response = await client.ListAssetsAsync();
            Assert.NotNull(response);

            while (true)
            {
                foreach (var asset in response)
                {
                    Assert.NotNull(asset);
                }

                if (!response.HasNextPage())
                {
                    break;
                }
                response = await response.GetNextPage();
                Assert.NotNull(response);

                break;
            }
        }

        [Fact]
        [Trait("Category", "Asset")]
        public async Task CreateAsset_WithStream_Uploads_CreatesAsset()
        {
            var client = CreateManagementClient();

            var content = $"Hello world from CM API .NET SDK test {nameof(CreateAsset_WithStream_Uploads_CreatesAsset)}! {"X".PadLeft((int)new Random().NextDouble() * 100, 'X')}";

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(content)))
            {
                var fileName = "Hello.txt";
                var contentType = "text/plain";

                var fileResult = await client.UploadFileAsync(new FileContentSource(stream, fileName, contentType));

                Assert.NotNull(fileResult);
                Assert.Equal(FileReferenceTypeEnum.Internal, fileResult.Type);

                Assert.True(Guid.TryParse(fileResult.Id, out Guid fileId));

                Assert.NotEqual(Guid.Empty, fileId);

                var asset = new AssetUpsertModel
                {
                    FileReference = fileResult,
                };

                var assetResult = await client.CreateAssetAsync(asset);

                Assert.NotNull(assetResult);
                Assert.Null(assetResult.ExternalId);
                Assert.Equal(contentType, assetResult.Type);
                Assert.Equal(content.Length, assetResult.Size);
                Assert.NotNull(assetResult.LastModified);
                Assert.Equal(fileName, assetResult.FileName);
                Assert.NotNull(assetResult.Descriptions);
                Assert.NotNull(assetResult.Url);

                // Cleanup
                await client.DeleteAssetAsync(AssetIdentifier.ById(assetResult.Id));
            }
        }

        [Fact]
        [Trait("Category", "Asset")]
        public async Task UpsertAssetByExternalId_WithByteArray_Uploads_CreatesAsset()
        {
            var client = CreateManagementClient();

            var content = $"Hello world from CM API .NET SDK test {nameof(UpsertAssetByExternalId_WithByteArray_Uploads_CreatesAsset)}! {"X".PadLeft((int)new Random().NextDouble() * 100, 'X')}";

            var fileName = "HelloExternal.txt";
            var contentType = "text/plain";

            var fileResult = await client.UploadFileAsync(new FileContentSource(Encoding.UTF8.GetBytes(content), fileName, contentType));

            Assert.NotNull(fileResult);
            Assert.Equal(FileReferenceTypeEnum.Internal, fileResult.Type);

            Assert.True(Guid.TryParse(fileResult.Id, out Guid fileId));

            Assert.NotEqual(Guid.Empty, fileId);

            var spanishDescription = "Spanish descriptión";
            var languageIdentifier = Reference.ById(EXISTING_LANGUAGE_ID);
            var assetDescription = new AssetDescription { Description = spanishDescription, Language = languageIdentifier };
            var descriptions = new[] { assetDescription };

            var asset = new AssetUpsertModel
            {
                FileReference = fileResult,
                Descriptions = descriptions
            };
            var externalId = "99877608d1f6448ebb35778f027c92f6";

            var assetResult = await client.UpsertAssetByExternalIdAsync(externalId, asset);

            Assert.NotNull(assetResult);
            Assert.Equal(externalId, assetResult.ExternalId);
            Assert.Equal(contentType, assetResult.Type);
            Assert.Equal(content.Length, assetResult.Size);
            Assert.NotNull(assetResult.LastModified);
            Assert.Equal(fileName, assetResult.FileName);
            Assert.Equal(spanishDescription, assetResult.Descriptions.FirstOrDefault(d => d.Language.Id == EXISTING_LANGUAGE_ID).Description);
            Assert.NotNull(assetResult.Url);

            // Cleanup
            await client.DeleteAssetAsync(AssetIdentifier.ByExternalId(externalId));
        }

        [Fact]
        [Trait("Category", "Asset")]
        public async Task CreateAsset_WithFile_Uploads_CreatesAsset()
        {
            var client = CreateManagementClient();

            var content = $"Hello world from CM API .NET SDK test {nameof(CreateAsset_WithFile_Uploads_CreatesAsset)}! {"X".PadLeft((int)new Random().NextDouble() * 100, 'X')}";

            var fileName = "Hello.txt";
            var contentType = "text/plain";

            var spanishDescription = "Spanish descriptión";
            var languageIdentifier = Reference.ById(EXISTING_LANGUAGE_ID);
            var assetDescription = new AssetDescription { Description = spanishDescription, Language = languageIdentifier };
            var descriptions = new[] { assetDescription };
            var title = "New title";

            var assetResult = await client.CreateAssetAsync(new FileContentSource(Encoding.UTF8.GetBytes(content), fileName, contentType), new AssetUpdateModel { Descriptions = descriptions, Title = title });

            Assert.NotNull(assetResult);
            Assert.Null(assetResult.ExternalId);
            Assert.Equal(contentType, assetResult.Type);
            Assert.Equal(content.Length, assetResult.Size);
            Assert.NotNull(assetResult.LastModified);
            Assert.Equal(fileName, assetResult.FileName);
            Assert.Equal(title, assetResult.Title);
            Assert.Equal(spanishDescription, assetResult.Descriptions.FirstOrDefault(d => d.Language.Id == EXISTING_LANGUAGE_ID).Description);
            Assert.NotNull(assetResult.Url);

            // Cleanup
            await client.DeleteAssetAsync(AssetIdentifier.ById(assetResult.Id));
        }

        [Fact]
        [Trait("Category", "Asset")]
        public async Task CreateAsset_FromFileSystem_Uploads_CreatesAsset()
        {
            var client = CreateManagementClient();

            var descriptions = new List<AssetDescription>();
            var title = "My new asset";

            var filePath = Path.Combine(Environment.CurrentDirectory, Path.Combine("Data", "kentico_rgb_bigger.png"));
            var contentType = "image/png";

            var assetResult = await client.CreateAssetAsync(new FileContentSource(filePath, contentType), new AssetUpdateModel { Descriptions = descriptions, Title = title });

            Assert.NotNull(assetResult);
            Assert.Null(assetResult.ExternalId);
            Assert.Equal(contentType, assetResult.Type);
            Assert.Equal(new FileInfo(filePath).Length, assetResult.Size);
            Assert.NotNull(assetResult.LastModified);
            Assert.Equal("kentico_rgb_bigger.png", assetResult.FileName);
            Assert.NotNull(assetResult.Descriptions);
            Assert.Equal(title, assetResult.Title);
            Assert.NotNull(assetResult.Url);

            // Cleanup
            await client.DeleteAssetAsync(AssetIdentifier.ById(assetResult.Id));
        }

        [Fact]
        [Trait("Category", "Asset")]
        public async Task UpsertAssetByExternalId_FromByteArray_Uploads_CreatesAsset()
        {
            var client = CreateManagementClient();

            var content = $"Hello world from CM API .NET SDK test {nameof(UpsertAssetByExternalId_FromByteArray_Uploads_CreatesAsset)}! {"X".PadLeft((int)new Random().NextDouble() * 100, 'X')}";

            var fileName = "HelloExternal.txt";
            var contentType = "text/plain";

            var externalId = "5bec7f21ad2e44bb8a3a1f4a6a5bf8ca";

            var spanishDescription = "Spanish descriptión";
            var languageIdentifier = Reference.ById(EXISTING_LANGUAGE_ID);
            var assetDescription = new AssetDescription { Description = spanishDescription, Language = languageIdentifier };
            var descriptions = new[] { assetDescription };
            var title = "New title";

            var assetResult = await client.UpsertAssetByExternalIdAsync(externalId, new FileContentSource(Encoding.UTF8.GetBytes(content), fileName, contentType), new AssetUpdateModel { Descriptions = descriptions, Title = title });

            Assert.NotNull(assetResult);
            Assert.Equal(externalId, assetResult.ExternalId);
            Assert.Equal(contentType, assetResult.Type);
            Assert.Equal(content.Length, assetResult.Size);
            Assert.NotNull(assetResult.LastModified);
            Assert.Equal(fileName, assetResult.FileName);
            Assert.Equal(title, assetResult.Title);
            Assert.Equal(spanishDescription, assetResult.Descriptions.FirstOrDefault(d => d.Language.Id == EXISTING_LANGUAGE_ID).Description);
            Assert.NotNull(assetResult.Url);

            // Cleanup
            await client.DeleteAssetAsync(AssetIdentifier.ByExternalId(externalId));
        }

        [Fact]
        [Trait("Category", "Asset")]
        public async Task UpdateAssetById_ReturnsUpdatedAsset()
        {
            var client = CreateManagementClient();

            var identifier = AssetIdentifier.ById(Guid.Parse("01647205-c8c4-4b41-b524-1a98a7b12750"));
            var title = "My super asset";
            var updatedDescription = new AssetDescription()
            {
                Language = Reference.ById(Guid.Empty),
                Description = "Dancing Goat Café - Los Angeles - UPDATED",
            };
            var update = new AssetUpdateModel() { Descriptions = new[] { updatedDescription }, Title = title };

            var assetResult = await client.UpdateAssetAsync(identifier, update);

            Assert.Equal(identifier.Id.ToString(), assetResult.Id.ToString());
            Assert.Equal(updatedDescription.Description, assetResult.Descriptions.FirstOrDefault(d => d.Language.Id == Guid.Empty).Description);
            Assert.Equal(title, assetResult.Title);
        }

        [Fact]
        [Trait("Category", "Asset")]
        public async Task GetAsset_WhenGivenAssetId_ReturnsGivenAsset()
        {
            var client = CreateManagementClient();

            var identifier = AssetIdentifier.ById(Guid.Parse("01647205-c8c4-4b41-b524-1a98a7b12750"));

            var response = await client.GetAssetAsync(identifier);

            Assert.Equal(identifier.Id, response.Id);
        }
    }
}

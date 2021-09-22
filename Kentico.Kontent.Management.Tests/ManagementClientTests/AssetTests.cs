using Kentico.Kontent.Management.Models.Assets;
using Kentico.Kontent.Management.Models.Assets.Patch;
using Kentico.Kontent.Management.Models.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using static Kentico.Kontent.Management.Tests.ManagementClientTests.Scenario;

namespace Kentico.Kontent.Management.Tests.ManagementClientTests
{
    [Trait("ManagementClient", "Asset")]
    public class AssetTests
    {
        private readonly ManagementClient _client;
        private readonly Scenario _scenario;

        public AssetTests(ITestOutputHelper output)
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
        public async Task ListAssets_ListsAssets()
        {
            

            var response = await _client.ListAssetsAsync();
            Assert.NotNull(response);
            Assert.NotNull(response.FirstOrDefault());
        }

        [Fact]
        public async Task ListFolders_ListFolders()
        {
            

            var response = await _client.GetAssetFoldersAsync();
            Assert.NotNull(response);
            Assert.True(response.Folders.Any());
        }

        [Fact]
        public async Task ListFolders_GetFolderLinkedTree()
        {
            

            var response = await _client.GetAssetFoldersAsync();
            response.Folders.GetParentLinkedFolderHierarchy();

            Assert.NotNull(response);
            Assert.True(response.Folders.Any());
        }


        [Fact]
        public async Task ListFolders_GetFolderLinkedTreeSearchByFolderId()
        {
            var response = await _client.GetAssetFoldersAsync();
            var linkedHierarchy = response.Folders.GetParentLinkedFolderHierarchy();
            var result = linkedHierarchy.GetParentLinkedFolderHierarchyByExternalId(ASSET_FOLDER_ID_1ST_LEVEL);
            var result2 = linkedHierarchy.GetParentLinkedFolderHierarchyByExternalId(ASSET_FOLDER_ID_2ND_LEVEL);
            var result3 = linkedHierarchy.GetParentLinkedFolderHierarchyByExternalId(ASSET_FOLDER_ID_3RD_LEVEL);
            var result4 = linkedHierarchy.GetParentLinkedFolderHierarchyByExternalId(ASSET_FOLDER_ID_4TH_LEVEL);

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
        public async Task ListFolders_GetFolderHierarchy_NonExistingFolder()
        {
            

            var response = await _client.GetAssetFoldersAsync();
            var nonExistingFolderId = "2ddaf2dc-8635-4b3f-b04d-5be69a0949e6";
            var result = response.Folders.GetFolderHierarchyById(nonExistingFolderId);

            Assert.Null(result);
        }

        [Fact()]
        public async Task ListFolders_GetFolderHierarchy_ExistingFolder()
        {
            

            var response = await _client.GetAssetFoldersAsync();
            var result = response.Folders.GetFolderHierarchyByExternalId(ASSET_FOLDER_ID_4TH_LEVEL);

            Assert.NotNull(result);
            Assert.True(result.Name == "4thFolder");
        }

        [Fact]
        public async Task ListFolders_GetFolderPathString()
        {
            

            var response = await _client.GetAssetFoldersAsync();
            var linkedHierarchy = response.Folders.GetParentLinkedFolderHierarchy();
            var result = linkedHierarchy.GetParentLinkedFolderHierarchyById("e2fe0a21-eb4c-5fba-8a28-697aeab81f83"); //Go three levels deep
            var pathString = result.GetFullFolderPath(); //Should be a folder path string TopFolder\2ndFolder\3rdFolder (3 levels deep)

            Assert.NotNull(response);
            Assert.NotNull(result);
            Assert.True(pathString == "TopFolder\\2ndFolder\\3rdFolder");
        }

        [Fact]
        public async Task ListAssets_WithContinuation_ListsAllAssets()
        {
            

            var response = await _client.ListAssetsAsync();
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
        public async Task CreateAsset_WithStream_Uploads_CreatesAsset()
        {
            

            var content = $"Hello world from CM API .NET SDK test {nameof(CreateAsset_WithStream_Uploads_CreatesAsset)}! {"X".PadLeft((int)new Random().NextDouble() * 100, 'X')}";

            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            var fileName = "Hello.txt";
            var contentType = "text/plain";

            var fileResult = await _client.UploadFileAsync(new FileContentSource(stream, fileName, contentType));

            Assert.NotNull(fileResult);
            Assert.Equal(FileReferenceTypeEnum.Internal, fileResult.Type);

            Assert.True(Guid.TryParse(fileResult.Id, out Guid fileId));

            Assert.NotEqual(Guid.Empty, fileId);

            var asset = new AssetUpsertModel
            {
                FileReference = fileResult,
            };

            var assetResult = await _client.CreateAssetAsync(asset);

            Assert.NotNull(assetResult);
            Assert.Null(assetResult.ExternalId);
            Assert.Equal(contentType, assetResult.Type);
            Assert.Equal(content.Length, assetResult.Size);
            Assert.NotNull(assetResult.LastModified);
            Assert.Equal(fileName, assetResult.FileName);
            Assert.NotNull(assetResult.Descriptions);
            Assert.NotNull(assetResult.Url);

            // Cleanup
            await _client.DeleteAssetAsync(AssetIdentifier.ById(assetResult.Id));
        }

        [Fact]
        public async Task UpsertAssetByExternalId_WithByteArray_Uploads_CreatesAsset()
        {
            

            var content = $"Hello world from CM API .NET SDK test {nameof(UpsertAssetByExternalId_WithByteArray_Uploads_CreatesAsset)}! {"X".PadLeft((int)new Random().NextDouble() * 100, 'X')}";

            var fileName = "HelloExternal.txt";
            var contentType = "text/plain";

            var fileResult = await _client.UploadFileAsync(new FileContentSource(Encoding.UTF8.GetBytes(content), fileName, contentType));

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

            var assetResult = await _client.UpsertAssetByExternalIdAsync(externalId, asset);

            Assert.NotNull(assetResult);
            Assert.Equal(externalId, assetResult.ExternalId);
            Assert.Equal(contentType, assetResult.Type);
            Assert.Equal(content.Length, assetResult.Size);
            Assert.NotNull(assetResult.LastModified);
            Assert.Equal(fileName, assetResult.FileName);
            Assert.Equal(spanishDescription, assetResult.Descriptions.FirstOrDefault(d => d.Language.Id == EXISTING_LANGUAGE_ID).Description);
            Assert.NotNull(assetResult.Url);

            // Cleanup
            await _client.DeleteAssetAsync(AssetIdentifier.ByExternalId(externalId));
        }

        [Fact]
        public async Task CreateAsset_WithFile_Uploads_CreatesAsset()
        {
            

            var content = $"Hello world from CM API .NET SDK test {nameof(CreateAsset_WithFile_Uploads_CreatesAsset)}! {"X".PadLeft((int)new Random().NextDouble() * 100, 'X')}";

            var fileName = "Hello.txt";
            var contentType = "text/plain";

            var spanishDescription = "Spanish descriptión";
            var languageIdentifier = Reference.ById(EXISTING_LANGUAGE_ID);
            var assetDescription = new AssetDescription { Description = spanishDescription, Language = languageIdentifier };
            var descriptions = new[] { assetDescription };
            var title = "New title";

            var assetResult = await _client.CreateAssetAsync(new FileContentSource(Encoding.UTF8.GetBytes(content), fileName, contentType), new AssetUpdateModel { Descriptions = descriptions, Title = title });

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
            await _client.DeleteAssetAsync(AssetIdentifier.ById(assetResult.Id));
        }

        [Fact]
        public async Task CreateAsset_FromFileSystem_Uploads_CreatesAsset()
        {
            

            var descriptions = new List<AssetDescription>();
            var title = "My new asset";

            var filePath = Path.Combine(Environment.CurrentDirectory, Path.Combine("Data", "kentico_rgb_bigger.png"));
            var contentType = "image/png";

            var assetResult = await _client.CreateAssetAsync(new FileContentSource(filePath, contentType), new AssetUpdateModel { Descriptions = descriptions, Title = title });

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
            await _client.DeleteAssetAsync(AssetIdentifier.ById(assetResult.Id));
        }

        [Fact]
        public async Task UpsertAssetByExternalId_FromByteArray_Uploads_CreatesAsset()
        {
            

            var content = $"Hello world from CM API .NET SDK test {nameof(UpsertAssetByExternalId_FromByteArray_Uploads_CreatesAsset)}! {"X".PadLeft((int)new Random().NextDouble() * 100, 'X')}";

            var fileName = "HelloExternal.txt";
            var contentType = "text/plain";

            var externalId = "5bec7f21ad2e44bb8a3a1f4a6a5bf8ca";

            var spanishDescription = "Spanish descriptión";
            var languageIdentifier = Reference.ById(EXISTING_LANGUAGE_ID);
            var assetDescription = new AssetDescription { Description = spanishDescription, Language = languageIdentifier };
            var descriptions = new[] { assetDescription };
            var title = "New title";

            var assetResult = await _client.UpsertAssetByExternalIdAsync(externalId, new FileContentSource(Encoding.UTF8.GetBytes(content), fileName, contentType), new AssetUpdateModel { Descriptions = descriptions, Title = title });

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
            await _client.DeleteAssetAsync(AssetIdentifier.ByExternalId(externalId));
        }

        [Fact]
        public async Task UpdateAssetById_ReturnsUpdatedAsset()
        {
            

            var identifier = AssetIdentifier.ById(Guid.Parse("01647205-c8c4-4b41-b524-1a98a7b12750"));
            var title = "My super asset";
            var updatedDescription = new AssetDescription()
            {
                Language = Reference.ById(Guid.Empty),
                Description = "Dancing Goat Café - Los Angeles - UPDATED",
            };
            var update = new AssetUpdateModel() { Descriptions = new[] { updatedDescription }, Title = title };

            var assetResult = await _client.UpdateAssetAsync(identifier, update);

            Assert.Equal(identifier.Id.ToString(), assetResult.Id.ToString());
            Assert.Equal(updatedDescription.Description, assetResult.Descriptions.FirstOrDefault(d => d.Language.Id == Guid.Empty).Description);
            Assert.Equal(title, assetResult.Title);
        }

        [Fact]
        public async Task GetAsset_WhenGivenAssetId_ReturnsGivenAsset()
        {
            

            var identifier = AssetIdentifier.ById(Guid.Parse("01647205-c8c4-4b41-b524-1a98a7b12750"));

            var response = await _client.GetAssetAsync(identifier);

            Assert.Equal(identifier.Id, response.Id);
        }

        [Fact]
        //todo this test might be flaky as it might delete folder structure during run of another test
        public async Task CreateFolders_CreatesFolders()
        {  
            //prepare - delete exisitng folders
            var folders = await RemoveFolderStructure();

            Assert.Empty(folders.Folders);

            //Act create it once again
            var newfolders = await CreateFolderStructure();

            Assert.Equal(ASSET_FOLDER_ID_1ST_LEVEL, newfolders.Folders.First().ExternalId);
        }

        [Fact]
        public async Task ModifyAssetFolder_AddInto_RemovesAssetFolder()
        {
            
            

            var change = new AssetFolderAddIntoModel
            {
                Reference = Reference.ByExternalId(ASSET_FOLDER_ID_2ND_LEVEL),
                Value = new AssetFolderHierarchy
                {
                    Folders = new List<AssetFolderHierarchy>(),
                    ExternalId = "externalId123",
                    Name = "NewFolder"
                },
                After = Reference.ByExternalId(ASSET_FOLDER_ID_3RD_LEVEL)
            };

            var response = await _client.ModifyAssetFoldersAsync(new[] { change });

            //we expect 2 folders on third level
            Assert.Equal(2, response.Folders.First().Folders.First().Folders.Count());

            //clean up 
            await RemoveFolderByExternalId("externalId123");
        }

        [Fact]
        public async Task ModifyAssetFolder_Remove_RemovesAssetFolder()
        {
            

            await AddIntoFolder("externalID167");

            //check that the folder exists
            var response = await _client.GetAssetFoldersAsync();
            var hierarchy = response.Folders.GetFolderHierarchyByExternalId("externalID167");
            Assert.NotNull(hierarchy);

            var change = new AssetFolderRemoveModel
            {
                Reference = Reference.ByExternalId("externalID167")
            };

            var removedResponse = await _client.ModifyAssetFoldersAsync(new[] { change });
            hierarchy = removedResponse.Folders.GetFolderHierarchyByExternalId("externalID167");

            //check that the folder does not exist
            Assert.Null(hierarchy);
        }

        [Fact]
        public async Task ModifyAssetFolder_Remame_RenamesAssetFolder()
        {
            

            await AddIntoFolder("externalID111");

            var change = new AssetFolderRenameModel
            {
                Reference = Reference.ByExternalId("externalID111"),
                Value = "My unique name"
            };

            var response = await _client.ModifyAssetFoldersAsync(new[] { change });
            var newFolder = response.Folders.GetFolderHierarchyByExternalId("externalID111");

            Assert.Equal("My unique name", newFolder.Name);

            await RemoveFolderByExternalId("externalID111");
        }

        private async Task<AssetFoldersModel> CreateFolderStructure()
        {
            var newFolderStructure = new AssetFolderCreateModel
            {
                Folders = new List<AssetFolderHierarchy>
                {
                    new AssetFolderHierarchy
                    {
                        Folders = new List<AssetFolderHierarchy>
                                  {
                                      new AssetFolderHierarchy
                                      {
                                          Folders = new List<AssetFolderHierarchy>
                                          {
                                              new AssetFolderHierarchy
                                              {
                                                  Folders = new List<AssetFolderHierarchy>
                                                  {
                                                      new AssetFolderHierarchy
                                                      {
                                                          Folders = new List<AssetFolderHierarchy>(),
                                                          ExternalId = ASSET_FOLDER_ID_4TH_LEVEL,
                                                          Name = "4thFolder"
                                                      }
                                                  },
                                                  ExternalId = ASSET_FOLDER_ID_3RD_LEVEL,
                                                  Name = "3rdFolder"
                                              }
                                          },
                                          ExternalId = ASSET_FOLDER_ID_2ND_LEVEL,
                                          Name = "2ndFolder"
                                      }
                                  },
                        ExternalId = ASSET_FOLDER_ID_1ST_LEVEL,
                        Name = "TopFolder"
                    }
                }
            };

            return await _client.CreateAssetFoldersAsync(newFolderStructure);
        }

        private async Task<AssetFoldersModel> RemoveFolderStructure()
        {
            var change = new AssetFolderRemoveModel
            {
                Reference = Reference.ByExternalId(ASSET_FOLDER_ID_1ST_LEVEL)
            };

            return await _client.ModifyAssetFoldersAsync(new[] { change });
        }

        private async Task RemoveFolderByExternalId(string externalId)
        {
            var change = new AssetFolderRemoveModel
            {
                Reference = Reference.ByExternalId(externalId)
            };

            await _client.ModifyAssetFoldersAsync(new[] { change });
        }

        private async Task<AssetFoldersModel> AddIntoFolder(string externalId)
        {
            var change = new AssetFolderAddIntoModel
            {
                Reference = Reference.ByExternalId(ASSET_FOLDER_ID_1ST_LEVEL),
                Value = new AssetFolderHierarchy
                {
                    Folders = new List<AssetFolderHierarchy>(),
                    ExternalId = externalId,
                    Name = externalId
                },
                After = Reference.ByExternalId(ASSET_FOLDER_ID_2ND_LEVEL)
            };

            return await _client.ModifyAssetFoldersAsync(new[] { change });
        }
    }
}

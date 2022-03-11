using Kentico.Kontent.Management.Models.Shared;
using Kentico.Kontent.Management.Tests.Unit.Base;
using Kentico.Kontent.Management.Tests.Unit.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Kentico.Kontent.Management.Extensions;
using Kentico.Kontent.Management.Models.Assets;
using Kentico.Kontent.Management.Models.StronglyTyped;

namespace Kentico.Kontent.Management.Tests.Unit.ManagementClientTests
{
    public class AssetTests : IClassFixture<FileSystemFixture>
    {
        private readonly FileSystemFixture _fileSystemFixture;

        public AssetTests(FileSystemFixture fileSystemFixture)
        {
            _fileSystemFixture = fileSystemFixture;
            _fileSystemFixture.SetSubFolder("Asset");
        }

        [Fact]
        public async Task ListAssetsAsync_DynamicallyTyped_WithMorePages_ListsAssets()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("AssetsPage1.json", "AssetsPage2.json", "AssetsPage3.json");

            var expected = new[] {
                "00000000-0000-0000-0000-000000000000",
                "10000000-0000-0000-0000-000000000000",
                "20000000-0000-0000-0000-000000000000"
            }.Select(x => GetExpectedDynamicAssetModel(assetId: x));

            var response = await client.ListAssetsAsync().GetAllAsync();

            response.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task ListAssetsAsync_StronglyTyped_WithMorePages_ListsAssets()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("AssetsPage1.json", "AssetsPage2.json", "AssetsPage3.json");

            var expected = new[] {
                "00000000-0000-0000-0000-000000000000",
                "10000000-0000-0000-0000-000000000000",
                "20000000-0000-0000-0000-000000000000"
            }.Select(x => GetExpectedStronglyTypedAssetModel(assetId: x));

            var response = await client.ListAssetsAsync<ComplexTestModel>().GetAllAsync();

            response.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetAssetAsync_StronglyTyped_ById_GetsAsset()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("Asset.json");

            var expected = GetExpectedStronglyTypedAssetModel();

            var response = await client.GetAssetAsync<ComplexTestModel>(Reference.ById(expected.Id));

            response.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetAssetAsync_StronglyTyped_ByCodename_Throws()
        {
            var client = _fileSystemFixture.CreateMockClientWithoutResponse();

            await client.Invoking(c => c.GetAssetAsync<ComplexTestModel>(Reference.ByCodename("codename")))
                .Should().ThrowExactlyAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task GetAssetAsync_StronglyTyped_ByExternalId_GetsAsset()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("Asset.json");

            var expected = GetExpectedStronglyTypedAssetModel();

            var response = await client.GetAssetAsync<ComplexTestModel>(Reference.ByExternalId(expected.ExternalId));

            response.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetAssetAsync_StronglyTyped_IdentifierIsNull_Throws()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("Asset.json");

            await client.Invoking(c => c.GetAssetAsync<ComplexTestModel>(null))
                .Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task GetAssetAsync_DynamicallyTyped_ById_GetsAsset()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("Asset.json");

            var expected = GetExpectedDynamicAssetModel();

            var response = await client.GetAssetAsync(Reference.ById(expected.Id));

            response.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetAssetAsync_DynamicallyTyped_ByCodename_Throws()
        {
            var client = _fileSystemFixture.CreateMockClientWithoutResponse();

            await client.Invoking(c => c.GetAssetAsync(Reference.ByCodename("codename")))
                .Should().ThrowExactlyAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task GetAssetAsync_DynamicallyTyped_ByExternalId_GetsAsset()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("Asset.json");

            var expected = GetExpectedDynamicAssetModel();

            var response = await client.GetAssetAsync(Reference.ByExternalId(expected.ExternalId));

            response.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetAssetAsync_DynamicallyTyped_IdentifierIsNull_Throws()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("Asset.json");

            await client.Invoking(c => c.GetAssetAsync(null))
                .Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task CreateAssetAsync_StronglyTyped_CreatesAsset()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("Asset.json");

            var expected = GetExpectedStronglyTypedAssetModel();

            var createModel = new AssetCreateModel<ComplexTestModel>
            {
                Title = expected.Title,
                ExternalId = expected.ExternalId,
                Elements = expected.Elements
            };

            var response = await client.CreateAssetAsync(createModel);

            response.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task CreateAssetAsync_StronglyTyped_IdentifierIsNull_Throws()
        {
            var client = _fileSystemFixture.CreateMockClientWithoutResponse();

            await client.Invoking(c => c.CreateAssetAsync<ComplexTestModel>(null))
                .Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task CreateAssetAsync_DynamicallyTyped_CreatesAsset()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("Asset.json");
        
            var expected = GetExpectedDynamicAssetModel();
        
            var createModel = new AssetCreateModel
            {
                Title = expected.Title,
                ExternalId = expected.ExternalId,
                Elements = expected.Elements
            };
        
            var response = await client.CreateAssetAsync(createModel);
        
            response.Should().BeEquivalentTo(expected);
        }
        
        [Fact]
        public async Task CreateAssetAsync_DynamicallyTyped_IdentifierIsNull_Throws()
        {
            var client = _fileSystemFixture.CreateMockClientWithoutResponse();

            await client.Invoking(c => c.CreateAssetAsync(null))
                .Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task UpsertAssetAsync_StronglyTyped_ById_UpsertsAsset()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("Asset.json");

            var expected = GetExpectedStronglyTypedAssetModel();

            var updateModel = new AssetUpsertModel<ComplexTestModel>
            {
                Title = expected.Title,
                Elements = expected.Elements
            };

            var response = await client.UpsertAssetAsync(Reference.ById(expected.Id), updateModel);

            response.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task UpsertAssetAsync_StronglyTyped_ByCodename_UpsertsAsset()
        {
            var client = _fileSystemFixture.CreateMockClientWithoutResponse();

            var updateModel = new AssetUpsertModel<ComplexTestModel> { Title = "xxx" };

            await client.Invoking(c => c.UpsertAssetAsync(Reference.ByCodename("c"), updateModel))
                .Should().ThrowExactlyAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task UpsertAssetAsync_StronglyTyped_ByExternalId_UpsertsAsset()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("Asset.json");

            var expected = GetExpectedStronglyTypedAssetModel();

            var updateModel = new AssetUpsertModel<ComplexTestModel>
            {
                Title = expected.Title,
                Elements = expected.Elements
            };

            var response = await client.UpsertAssetAsync(Reference.ByExternalId(expected.ExternalId), updateModel);

            response.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task UpsertAssetAsync_StronglyTyped_IdentifierIsNull_Throws()
        {
            var client = _fileSystemFixture.CreateMockClientWithoutResponse();

            var updateModel = new AssetUpsertModel<ComplexTestModel>
            {
                Title = "xxx"
            };

            await client.Invoking(c => c.UpsertAssetAsync(null, updateModel))
                .Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task UpsertAssetAsync_StronglyTyped_UpsertModelIsNull_Throws()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("Asset.json");

            await client.Invoking(c => c.UpsertAssetAsync<ComplexTestModel>(Reference.ByExternalId("ex"), null))
                .Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task UpsertAssetAsync_DynamicallyTyped_ById_UpsertsAsset()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("Asset.json");

            var expected = GetExpectedDynamicAssetModel();

            var updateModel = new AssetUpsertModel
            {
                Title = expected.Title,
                Elements = expected.Elements
            };

            var response = await client.UpsertAssetAsync(Reference.ById(expected.Id), updateModel);

            response.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task UpsertAssetAsync_DynamicallyTyped_ByCodename_UpsertsAsset()
        {
            var client = _fileSystemFixture.CreateMockClientWithoutResponse();
                
            var updateModel = new AssetUpsertModel{ Title = "xxx" };

            await client.Invoking(c => c.UpsertAssetAsync(Reference.ByCodename("c"), updateModel))
                .Should().ThrowExactlyAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task UpsertAssetAsync_DynamicallyTyped_ByExternalId_UpsertsAsset()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("Asset.json");

            var expected = GetExpectedDynamicAssetModel();

            var updateModel = new AssetUpsertModel
            {
                Title = expected.Title,
                Elements = expected.Elements
            };

            var response = await client.UpsertAssetAsync(Reference.ByExternalId(expected.ExternalId), updateModel);

            response.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task UpsertAssetAsync_DynamicallyTyped_IdentifierIsNull_Throws()
        {
            var client = _fileSystemFixture.CreateMockClientWithoutResponse();

            var updateModel = new AssetUpsertModel
            {
                Title = "xxx"
            };
            
            await client.Invoking(c => c.UpsertAssetAsync(null, updateModel))
                .Should().ThrowExactlyAsync<ArgumentNullException>();
        }
        
        [Fact]
        public async Task UpsertAssetAsync_DynamicallyTyped_UpsertModelIsNull_Throws()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("Asset.json");

            await client.Invoking(c => c.UpsertAssetAsync(Reference.ByExternalId("ex"), null))
                .Should().ThrowExactlyAsync<ArgumentNullException>();
        }
        
      

        private static AssetModel GetExpectedDynamicAssetModel(string assetId = "01647205-c8c4-4b41-b524-1a98a7b12750")
        {
            var stronglyTyped = GetExpectedStronglyTypedAssetModel(assetId);

            return new AssetModel
            {
                Id = stronglyTyped.Id, 
                ExternalId = stronglyTyped.ExternalId,
                FileName = stronglyTyped.FileName, 
                Title = stronglyTyped.Title, 
                Size = stronglyTyped.Size, 
                Type = stronglyTyped.Type, 
                Url = stronglyTyped.Url, 
                ImageWidth = stronglyTyped.ImageWidth, 
                ImageHeight = stronglyTyped.ImageHeight, 
                FileReference = stronglyTyped.FileReference, 
                LastModified = stronglyTyped.LastModified,
                Descriptions = stronglyTyped.Descriptions,
                Elements = ElementsData.GetExpectedDynamicElements(),
            };
        }

        private static AssetModel<ComplexTestModel> GetExpectedStronglyTypedAssetModel(string assetId = "01647205-c8c4-4b41-b524-1a98a7b12750") => new()
        {
            Id = Guid.Parse(assetId),
            ExternalId = "asset-1",
            FileName = "our-story.jpg",
            Title = "My super asset",
            Size = 69518,
            Type = "image/jpeg",
            Url = "https://assets-eu-01.kc-usercontent.com/a9931a80-9af4-010b-0590-ecb1273cf1b8/36f361fa-7f65-446f-b16e-170455766f3e/our-story.jpg",
            ImageWidth = 2160,
            ImageHeight = 1000,
            FileReference = new FileReference
            {
                Id = "36f361fa-7f65-446f-b16e-170455766f3e",
                Type = FileReferenceTypeEnum.Internal,
            },
            LastModified = DateTimeOffset.Parse("2021-11-06T13:57:51.3425375Z").UtcDateTime,
            Descriptions = new []
            {
                new AssetDescription
                {
                    Language = Reference.ById(Guid.Empty),
                    Description = "Dancing Goat Café - Los Angeles"
                },
                new AssetDescription
                {
                    Language = Reference.ById(Guid.Parse("78dbefe8-831b-457e-9352-f4c4eacd5024")),
                    Description = "Bolso de cafe en grano"
                }
            },
            Elements = ElementsData.GetExpectedStronglyTypedElementsModel(),
        };
    }
}

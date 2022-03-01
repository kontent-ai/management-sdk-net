using Kentico.Kontent.Management.Models.Shared;
using Kentico.Kontent.Management.Tests.Unit.Base;
using Kentico.Kontent.Management.Tests.Unit.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
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

        [Theory]
        [MemberData(nameof(AssetReferenceCombinations))]
        public async Task GetAssetAsync_ById_ReturnsStronglyTypedAssetModel(Reference assetReference)
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("Asset.json");

            var expected = GetExpectedStronglyTypedAssetModel();

            var response = await client.GetAssetAsync<ComplexTestModel>(assetReference);

            response.Should().BeEquivalentTo(expected);
        }
        
        [Theory]
        [MemberData(nameof(AssetReferenceCombinations))]
        public async Task GetAssetAsync_ById_ReturnsDynamicAssetModel(Reference assetReference)
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("Asset.json");

            var expected = GetExpectedDynamicAssetModel();

            var response = await client.GetAssetAsync(assetReference);

            response.Should().BeEquivalentTo(expected);
        }
        
        [Fact]
        public async Task ListAssetsAsync_ReturnsDynamicAssetModels()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("Assets.json");

            var expected = new List<AssetModel> { GetExpectedDynamicAssetModel() };

            var response = await client.ListAssetsAsync();

            response.ToList().Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task CreateAssetAsync_CreatesDynamicAssetModel()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("Asset.json");
        
            var expected = GetExpectedDynamicAssetModel();
        
            var createModel = new AssetCreateModel
            {
                Title = expected.Title,
                Elements = expected.Elements
            };
        
            var response = await client.CreateAssetAsync(createModel);
        
            response.Should().BeEquivalentTo(expected);
        }
        
        [Fact]
        public async Task CreateAssetAsync_CreatesStronglyTypedAssetModel()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("Asset.json");
        
            var expected = GetExpectedStronglyTypedAssetModel();
        
            var createModel = new AssetCreateModel<ComplexTestModel>
            {
                Title = expected.Title,
                Elements = expected.Elements
            };
        
            var response = await client.CreateAssetAsync(createModel);
        
            response.Should().BeEquivalentTo(expected);
        }
        
        [Theory]
        [MemberData(nameof(AssetReferenceCombinations))]
        public async Task UpdateAssetAsync_ByAssetReference_UpdatesDynamicAssetModel(Reference assetReference)
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("Asset.json");
        
            var expected = GetExpectedDynamicAssetModel();
        
            var updateModel = new AssetUpdateModel
            {
                Title = expected.Title,
                Elements = expected.Elements
            };
        
            var response = await client.UpdateAssetAsync(assetReference, updateModel);
        
            response.Should().BeEquivalentTo(expected);
        }
        
        [Theory]
        [MemberData(nameof(AssetReferenceCombinations))]
        public async Task UpdateAssetAsync_ByAssetReference_UpdatesStronglyTypedAssetModel(Reference assetReference)
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("Asset.json");
        
            var expected = GetExpectedStronglyTypedAssetModel();
        
            var updateModel = new AssetUpdateModel<ComplexTestModel>
            {
                Title = expected.Title,
                Elements = expected.Elements
            };
        
            var response = await client.UpdateAssetAsync(assetReference, updateModel);
        
            response.Should().BeEquivalentTo(expected);
        }
        
        
        [Fact]
        public async Task UpsertAssetByExternalIdAsync_ByExternalId_UpdatesDynamicAssetModel()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("Asset.json");
        
            var expected = GetExpectedDynamicAssetModel();
        
            var upsertModel = new AssetUpsertModel
            {
                Title = expected.Title,
                Elements = expected.Elements
            };
        
            var response = await client.UpsertAssetByExternalIdAsync("ext-asset-id", upsertModel);
        
            response.Should().BeEquivalentTo(expected);
        }
        
        [Fact]
        public async Task UpsertAssetByExternalIdAsync_ByExternalId_UpdatesStronglyTypedAssetModel()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("Asset.json");
        
            var expected = GetExpectedStronglyTypedAssetModel();
        
            var upsertModel = new AssetUpsertModel<ComplexTestModel>
            {
                Title = expected.Title,
                Elements = expected.Elements
            };
        
            var response = await client.UpsertAssetByExternalIdAsync("ext-asset-id", upsertModel);
        
            response.Should().BeEquivalentTo(expected);
        }

        private static AssetModel GetExpectedDynamicAssetModel()
        {
            var stronglyTyped = GetExpectedStronglyTypedAssetModel();

            return new AssetModel
            {
                Id = stronglyTyped.Id, 
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

        private static AssetModel<ComplexTestModel> GetExpectedStronglyTypedAssetModel() => new()
        {
            Id = Guid.Parse("01647205-c8c4-4b41-b524-1a98a7b12750"),
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
        
        public static IEnumerable<object[]> AssetReferenceCombinations
        {
            get
            {
                yield return new object [] { Reference.ById(Guid.Parse("01647205-c8c4-4b41-b524-1a98a7b12750")) };
                yield return new object [] { Reference.ByExternalId("asset-1") };
            }
        }
    }
}

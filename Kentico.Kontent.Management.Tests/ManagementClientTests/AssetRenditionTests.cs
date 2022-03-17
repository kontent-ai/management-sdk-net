using FluentAssertions;
using Kentico.Kontent.Management.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions.Execution;
using Kentico.Kontent.Management.Models.AssetRenditions;
using Xunit;
using Kentico.Kontent.Management.Tests.Base;

namespace Kentico.Kontent.Management.Tests.ManagementClientTests
{
    public class AssetRenditionTests : IClassFixture<FileSystemFixture>
    {
        private readonly FileSystemFixture _fileSystemFixture;

        public AssetRenditionTests(FileSystemFixture fileSystemFixture)
        {
            _fileSystemFixture = fileSystemFixture;
            _fileSystemFixture.SetSubFolder("AssetRendition");
        }

        [Theory]
        [MemberData(nameof(AssetReferenceRenditionReferenceCombinations))]
        public async Task GetRenditionAsync_ByCombinationOfInternalAndExternalIds_ReturnsRendition(Reference assetReference, Reference renditionReference)
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("AssetRendition.json");

            var expected = _fileSystemFixture.GetExpectedResponse<AssetRenditionModel>("AssetRendition.json");

            var identifier = new AssetRenditionIdentifier(assetReference, renditionReference);

            var response = await client.GetAssetRenditionAsync(identifier);

            response.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetRenditionAsync_IdentifierIsNull_Throws()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("AssetRendition.json");

            await client.Invoking(x => x.GetAssetRenditionAsync(null))
                .Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Theory]
        [MemberData(nameof(AssetReferenceCombinations))]
        public async Task ListRenditionsAsync_ByAssetReference_ReturnsRenditions(Reference assetReference)
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("AssetRenditions.json");

            var expected = _fileSystemFixture.GetItemsOfExpectedListingResponse<AssetRenditionModel>("AssetRenditions.json");

            var response = await client.ListAssetRenditionsAsync(assetReference);

            response.ToList().Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task ListRenditionsAsync_IdentifierIsNull_Throws()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("AssetRendition.json");

            await client.Invoking(x => x.ListAssetRenditionsAsync(null))
                .Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Theory]
        [MemberData(nameof(AssetReferenceCombinations))]
        public async Task CreateRenditionAsync_ByAssetReference_CreatesRenditions(Reference assetReference)
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("AssetRendition.json");

            var expected = _fileSystemFixture.GetExpectedResponse<AssetRenditionModel>("AssetRendition.json");

            var createModel = new AssetRenditionCreateModel
            {
                ExternalId = "rendition-1",
                Transformation = new RectangleResizeTransformation
                {
                    CustomWidth = 120,
                    CustomHeight = 240,
                    X = 300,
                    Y = 200,
                    Width = 360,
                    Height = 720,
                }
            };

            var response = await client.CreateAssetRenditionAsync(assetReference, createModel);

            using (new AssertionScope())
            {
                response.Transformation.Should().BeEquivalentTo(createModel.Transformation);
                response.ExternalId.Should().BeEquivalentTo(expected.ExternalId);
                response.AssetId.Should().Be(expected.AssetId);
                response.RenditionId.Should().Be(expected.RenditionId);
            }
        }

        [Fact]
        public async Task CreateRenditionAsync_IdentifierIsNull_Throws()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("AssetRendition.json");
            var createModel = new AssetRenditionCreateModel
            {
                ExternalId = "rendition-1",
                Transformation = new RectangleResizeTransformation
                {
                    CustomWidth = 120,
                    CustomHeight = 240,
                    X = 300,
                    Y = 200,
                    Width = 360,
                    Height = 720,
                }
            };

            await client.Invoking(x => x.CreateAssetRenditionAsync(null, createModel))
                .Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task CreateRenditionAsync_CreateModelIsNull_Throws()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("AssetRendition.json");

            await client.Invoking(x => x.CreateAssetRenditionAsync(Reference.ByExternalId("asset-1"), null))
                .Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Theory]
        [MemberData(nameof(AssetReferenceRenditionReferenceCombinations))]
        public async Task UpdateRenditionAsync_ByCombinationOfInternalAndExternalIds_UpdatesRenditions(Reference assetReference, Reference renditionReference)
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("AssetRendition.json");

            var expected = _fileSystemFixture.GetExpectedResponse<AssetRenditionModel>("AssetRendition.json");

            var updateModel = new AssetRenditionUpdateModel()
            {
                Transformation = new RectangleResizeTransformation
                {
                    CustomWidth = 120,
                    CustomHeight = 240,
                    X = 300,
                    Y = 200,
                    Width = 360,
                    Height = 720,
                }
            };

            var response = await client.UpdateAssetRenditionAsync(new AssetRenditionIdentifier(assetReference, renditionReference), updateModel);

            using (new AssertionScope())
            {
                response.Transformation.Should().BeEquivalentTo(updateModel.Transformation);
                response.ExternalId.Should().BeEquivalentTo(expected.ExternalId);
                response.AssetId.Should().Be(expected.AssetId);
                response.RenditionId.Should().Be(expected.RenditionId);
            }
        }

        [Fact]
        public async Task UpdateRenditionAsync_IdentifierIsNull_Throws()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("AssetRendition.json");
            var updateModel = new AssetRenditionUpdateModel
            {
                Transformation = new RectangleResizeTransformation
                {
                    CustomWidth = 120,
                    CustomHeight = 240,
                    X = 300,
                    Y = 200,
                    Width = 360,
                    Height = 720,
                }
            };

            await client.Invoking(x => x.UpdateAssetRenditionAsync(null, updateModel))
                .Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task UpdateRenditionAsync_CreateModelIsNull_Throws()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("AssetRendition.json");

            var identifier = new AssetRenditionIdentifier(Reference.ByExternalId("asset-1"), Reference.ByExternalId("rendition-1"));

            await client.Invoking(x => x.UpdateAssetRenditionAsync(identifier, null))
                .Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        public static IEnumerable<object[]> AssetReferenceRenditionReferenceCombinations
        {
            get
            {
                yield return new object[] { Reference.ById(Guid.NewGuid()), Reference.ById(Guid.NewGuid()) };
                yield return new object[] { Reference.ByExternalId("asset-1"), Reference.ById(Guid.NewGuid()) };
                yield return new object[] { Reference.ById(Guid.NewGuid()), Reference.ByExternalId("rendition-1") };
                yield return new object[] { Reference.ByExternalId("asset-1"), Reference.ByExternalId("rendition-1") };
            }
        }

        public static IEnumerable<object[]> AssetReferenceCombinations
        {
            get
            {
                yield return new object[] { Reference.ById(Guid.NewGuid()) };
                yield return new object[] { Reference.ByExternalId("asset-1") };
            }
        }
    }
}

using System;
using System.IO;
using Kentico.Kontent.Management.Models.Assets;
using Kentico.Kontent.Management.Models.LanguageVariants;
using Kentico.Kontent.Management.Models.LanguageVariants.Elements;
using Kentico.Kontent.Management.Models.Shared;
using Kentico.Kontent.Management.Tests.Unit.Base;
using Xunit;

namespace Kentico.Kontent.Management.Tests.Unit.CodeSamples
{
    public class ImportingAssets : IClassFixture<FileSystemFixture>
    {
        private FileSystemFixture _fileSystemFixture;

        public ImportingAssets(FileSystemFixture fileSystemFixture)
        {
            _fileSystemFixture = fileSystemFixture;
        }

        // DocSection: importing_assets_create_asset
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void CreateAsset()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("Empty.json");

            // Uses the file reference object obtained in step 1
            var createdAssetResponse = await client.UpsertAssetByExternalIdAsync("which-brewing-fits-you", new AssetUpsertModel
            {
                // 'fileReference' is only required when creating a new asset
                // To create a file reference, see the "Upload a binary file" endpoint
                FileReference = new FileReference
                {
                    Id = "8660e19c-7bbd-48a3-bb51-721934c7756c",
                    Type = FileReferenceTypeEnum.Internal
                },
                Title = "Brno Cafe",
                Descriptions = new AssetDescription[]
                {
                    new AssetDescription
                    {
                        Description = "Cafe in Brno",
                        Language = Reference.ByCodename("en-US")
                    },
                    new AssetDescription
                    {
                        Description = "Café en Brno",
                        Language = Reference.ByCodename("es-ES")
                    }
                }
            });
        }

        // DocSection: importing_assets_upload_file
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void UploadingFiles()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("Empty.json");

            string filePath = Path.Combine(Environment.CurrentDirectory, "Unit", "Data", "brno-cafe-1080px.jpg");
            string contentType = "image/jpg";

            // Binary file reference to be used when adding a new asset
            var response = await client.UploadFileAsync(new FileContentSource(filePath, contentType));
        }

        // DocSection: importing_assets_upload_file
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void UseAsset()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("Empty.json");

            var identifier = new LanguageVariantIdentifier(Reference.ByExternalId("ext-cafe-brno"), Reference.ByCodename("en-US"));

            var response = await client.UpsertLanguageVariantAsync(identifier, new LanguageVariantUpsertModel
            {
                Elements = new dynamic[]
                {
                    new AssetElement
                    {
                        Element = Reference.ByCodename("photo"),
                        Value = new[]
                        {
                            Reference.ByExternalId("brno-cafe-image")
                        }
                    }.ToDynamic()
                }
            });
        }

        // DocSection: importing_assets_upload_file
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void UseAssetRichText()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("Empty.json");

            var identifier = new LanguageVariantIdentifier(Reference.ByExternalId("new-cafes"), Reference.ByCodename("en-US"));

            var response = await client.UpsertLanguageVariantAsync(identifier, new LanguageVariantUpsertModel
            {
                Elements = new dynamic[]
                {
                    new RichTextElement
                    {
                        Element = Reference.ByCodename("body_copy"),
                        Value = "<p>...</p> <figure data-asset-external-id=\"brno-cafe-image\"></figure>",
                    }.ToDynamic()
                }
            });
        }
    }
}
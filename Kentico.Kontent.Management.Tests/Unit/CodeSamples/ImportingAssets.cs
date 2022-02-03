using System;
using System.IO;
using Kentico.Kontent.Management.Models.Assets;
using Kentico.Kontent.Management.Models.LanguageVariants;
using Kentico.Kontent.Management.Models.LanguageVariants.Elements;
using Kentico.Kontent.Management.Models.Shared;
using Kentico.Kontent.Management.Modules.ModelBuilders;
using Kentico.Kontent.Management.Tests.Unit.Base;
using Xunit;

namespace Kentico.Kontent.Management.Tests.Unit.CodeSamples
{
    /// <summary>
    /// Source for Code examples being store in https://github.com/KenticoDocs/kontent-docs-samples/tree/master/net/importing-assets
    /// </summary>
    public class ImportingAssets : IClassFixture<FileSystemFixture>
    {
        // IF YOU MAKE ANY CHANGE TO THIS FILE - ADJUST THE CODE SAMPLES

        private FileSystemFixture _fileSystemFixture;

        public ImportingAssets(FileSystemFixture fileSystemFixture)
        {
            _fileSystemFixture = fileSystemFixture;
            _fileSystemFixture.SetSubFolder("CodeSamples");
        }

        // DocSection: importing_assets_create_asset
        // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
        [Fact]
        public async void CreateAsset()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("Empty.json");

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
        // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
        [Fact]
        public async void UploadingFiles()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("Empty.json");

            string filePath = Path.Combine(Environment.CurrentDirectory, "Unit", "Data", "brno-cafe-1080px.jpg");
            string contentType = "image/jpg";

            // Binary file reference to be used when adding a new asset
            var response = await client.UploadFileAsync(new FileContentSource(filePath, contentType));
        }

        // DocSection: importing_assets_upload_file
        // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
        [Fact]
        public async void UseAsset()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("Empty.json");

            var identifier = new LanguageVariantIdentifier(Reference.ByExternalId("ext-cafe-brno"), Reference.ByCodename("en-US"));

            var response = await client.UpsertLanguageVariantAsync(identifier, new LanguageVariantUpsertModel
            {
                Elements = ElementBuilder.GetElementsAsDynamic(new BaseElement[]
                {
                    new AssetElement
                    {
                        Element = Reference.ByCodename("photo"),
                        Value = new[]
                        {
                            Reference.ByExternalId("brno-cafe-image")
                        }
                    }
                })
            });
        }

        // DocSection: importing_assets_upload_file
        // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
        [Fact]
        public async void UseAssetRichText()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("Empty.json");

            var identifier = new LanguageVariantIdentifier(Reference.ByExternalId("new-cafes"), Reference.ByCodename("en-US"));

            var response = await client.UpsertLanguageVariantAsync(identifier, new LanguageVariantUpsertModel
            {
                Elements = ElementBuilder.GetElementsAsDynamic(new BaseElement[]
                {
                    new RichTextElement
                    {
                        Element = Reference.ByCodename("body_copy"),
                        Value = "<p>...</p> <figure data-asset-external-id=\"brno-cafe-image\"></figure>",
                    }
                })
            });
        }
    }
}
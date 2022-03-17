using System.Linq;
using Kentico.Kontent.Management.Models.Shared;
using Kentico.Kontent.Management.Models.TaxonomyGroups;
using Kentico.Kontent.Management.Models.Types;
using Kentico.Kontent.Management.Models.Types.Elements;
using Kentico.Kontent.Management.Models.TypeSnippets;
using Kentico.Kontent.Management.Tests.Base;
using Xunit;

namespace Kentico.Kontent.Management.Tests.CodeSamples
{
    /// <summary>
    /// Source for Code examples being store in https://github.com/KenticoDocs/kontent-docs-samples/tree/master/net/importing-content-model
    /// </summary>
    public class ImportingContentModel : IClassFixture<FileSystemFixture>
    {
        // IF YOU MAKE ANY CHANGE TO THIS FILE - ADJUST THE CODE SAMPLES

        private FileSystemFixture _fileSystemFixture;

        public ImportingContentModel(FileSystemFixture fileSystemFixture)
        {
            _fileSystemFixture = fileSystemFixture;
            _fileSystemFixture.SetSubFolder("CodeSamples");
        }

        // DocSection: import_model_create_snippet
        // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
        [Fact]
        public async void CreateSnippet()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("Empty.json");

            var response = await client.CreateContentTypeSnippetAsync(new CreateContentSnippetCreateModel
            {
                Name = "Metadata",
                Codename = "metadata",
                Elements = new ElementMetadataBase[]
                {
                    new TextElementMetadataModel
                    {
                        Name = "Title",
                        Codename = "title",
                    },
                    new TextElementMetadataModel
                    {
                        Name = "Keywords",
                        Codename = "keywords",
                    },
                    new TextElementMetadataModel
                    {
                        Name = "Description",
                        Codename = "description",
                    },
                }
            });
        }

        // DocSection: import_model_create_taxonomy
        // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
        [Fact]
        public async void CreateTaxonomy()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("Empty.json");

            var response = await client.CreateTaxonomyGroupAsync(new TaxonomyGroupCreateModel
            {
                Name = "Blogpost topic",
                Codename = "blog_topic",
                Terms = new TaxonomyTermCreateModel[]
                    {
                        new TaxonomyTermCreateModel
                        {
                            Name = "Sport",
                            Codename = "sport",
                            Terms = new TaxonomyTermCreateModel[]
                            {
                                new TaxonomyTermCreateModel
                                {
                                    Name = "Soccer",
                                    ExternalId = "soccer",
                                    Terms = Enumerable.Empty<TaxonomyTermCreateModel>()
                                },
                                new TaxonomyTermCreateModel
                                {
                                    Name = "Ice hockey",
                                    ExternalId = "hockey",
                                    Terms = Enumerable.Empty<TaxonomyTermCreateModel>()
                                },
                                new TaxonomyTermCreateModel
                                {
                                    Name = "Rugby",
                                    ExternalId = "rugby",
                                    Terms = Enumerable.Empty<TaxonomyTermCreateModel>()
                                },
                            }
                        },
                        new TaxonomyTermCreateModel
                        {
                            Name = "Technology stack",
                            Codename = "tech",
                            Terms = new TaxonomyTermCreateModel[]
                            {
                                new TaxonomyTermCreateModel
                                {
                                    Name = "Javascript",
                                    ExternalId = "js",
                                    Terms = Enumerable.Empty<TaxonomyTermCreateModel>()
                                },
                                new TaxonomyTermCreateModel
                                {
                                    Name = "C#",
                                    ExternalId = "c",
                                    Terms = Enumerable.Empty<TaxonomyTermCreateModel>()
                                },
                                new TaxonomyTermCreateModel
                                {
                                    Name = "MVC",
                                    ExternalId = "mvc",
                                    Terms = Enumerable.Empty<TaxonomyTermCreateModel>()
                                },
                            }
                        },
                    }
            });
        }

        // DocSection: import_model_create_type
        // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
        [Fact]
        public async void CreateType()
        {
            var client = _fileSystemFixture.CreateMockClientWithResponse("Empty.json");

            var response = await client.CreateContentTypeAsync(new ContentTypeCreateModel
            {
                Name = "Blogpost",
                Codename = "blogpost",
                ContentGroups = new[]
                {
                    new ContentGroupModel
                    {
                        Name = "Content",
                        ExternalId = "content",
                    },
                    new ContentGroupModel
                    {
                        Name = "Metadata",
                        ExternalId = "metadata",
                    },
                    new ContentGroupModel
                    {
                        Name = "Topic",
                        ExternalId = "topic",
                    }
                },
                Elements = new ElementMetadataBase[]
                {
                    new TextElementMetadataModel
                    {
                        Name = "Title",
                        ContentGroup = Reference.ByExternalId("content")
                    },
                    new AssetElementMetadataModel
                    {
                        Name = "Image",
                        ContentGroup = Reference.ByExternalId("content")
                    },
                    new RichTextElementMetadataModel
                    {
                        Name = "Blog content",
                        ContentGroup = Reference.ByExternalId("content")
                    },
                    new ContentTypeSnippetElementMetadataModel
                    {
                        SnippetIdentifier = Reference.ByCodename("metadata"),
                        Codename = "metadata",
                        ContentGroup = Reference.ByExternalId("metadata")
                    },
                    new TaxonomyElementMetadataModel
                    {
                        TaxonomyGroup = Reference.ByExternalId("blog_topic"),
                        Codename = "taxonomy",
                        ContentGroup = Reference.ByExternalId("topic")
                    }
                }
            });
        }
    }
}
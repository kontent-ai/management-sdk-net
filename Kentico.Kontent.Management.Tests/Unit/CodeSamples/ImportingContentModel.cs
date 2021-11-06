using System.Linq;
using Kentico.Kontent.Management.Models.Shared;
using Kentico.Kontent.Management.Models.TaxonomyGroups;
using Kentico.Kontent.Management.Models.Types;
using Kentico.Kontent.Management.Models.Types.Elements;
using Kentico.Kontent.Management.Models.TypeSnippets;
using Kentico.Kontent.Management.Tests.Unit.Base;
using Xunit;

namespace Kentico.Kontent.Management.Tests.Unit.CodeSamples
{
    public class ImportingContentModel : IClassFixture<FileSystemFixture>
    {
        private FileSystemFixture _fileSystemFixture;

        public ImportingContentModel(FileSystemFixture fileSystemFixture)
        {
            _fileSystemFixture = fileSystemFixture;
        }

        // DocSection: import_model_create_snippet
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void CreateSnippet()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("Empty.json");

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
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void CreateTaxonomy()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("Empty.json");

            var response = await client.CreateTaxonomyGroupAsync(new TaxonomyGroupCreateModel
            {
                Name = "Blogpost topic",
                Codename = "blog_topic",
                Terms = new TaxonomyGroupCreateModel[]
                    {
                        new TaxonomyGroupCreateModel
                        {
                            Name = "Sport",
                            Codename = "sport",
                            Terms = new TaxonomyGroupCreateModel[]
                            {
                                new TaxonomyGroupCreateModel
                                {
                                    Name = "Soccer",
                                    ExternalId = "soccer",
                                    Terms = Enumerable.Empty<TaxonomyGroupCreateModel>()
                                },
                                new TaxonomyGroupCreateModel
                                {
                                    Name = "Ice hockey",
                                    ExternalId = "hockey",
                                    Terms = Enumerable.Empty<TaxonomyGroupCreateModel>()
                                },
                                new TaxonomyGroupCreateModel
                                {
                                    Name = "Rugby",
                                    ExternalId = "rugby",
                                    Terms = Enumerable.Empty<TaxonomyGroupCreateModel>()
                                },
                            }
                        },
                        new TaxonomyGroupCreateModel
                        {
                            Name = "Technology stack",
                            Codename = "tech",
                            Terms = new TaxonomyGroupCreateModel[]
                            {
                                new TaxonomyGroupCreateModel
                                {
                                    Name = "Javascript",
                                    ExternalId = "js",
                                    Terms = Enumerable.Empty<TaxonomyGroupCreateModel>()
                                },
                                new TaxonomyGroupCreateModel
                                {
                                    Name = "C#",
                                    ExternalId = "c",
                                    Terms = Enumerable.Empty<TaxonomyGroupCreateModel>()
                                },
                                new TaxonomyGroupCreateModel
                                {
                                    Name = "MVC",
                                    ExternalId = "mvc",
                                    Terms = Enumerable.Empty<TaxonomyGroupCreateModel>()
                                },
                            }
                        },
                    }
            });
        }

        // DocSection: import_model_create_type
        // Tip: Find more about .NET SDKs at https://docs.kontent.ai/net
        [Fact]
        public async void CreateType()
        {
            var client = _fileSystemFixture.CreateDefaultMockClientRespondingWithFilename("Empty.json");

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
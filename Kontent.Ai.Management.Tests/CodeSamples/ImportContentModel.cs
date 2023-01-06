using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.TaxonomyGroups;
using Kontent.Ai.Management.Models.Types;
using Kontent.Ai.Management.Models.Types.Elements;
using Kontent.Ai.Management.Models.Types.Elements.DefaultValues;
using Kontent.Ai.Management.Models.TypeSnippets;
using Kontent.Ai.Management.Tests.Base;
using System.Linq;
using Xunit;

namespace Kontent.Ai.Management.Tests.CodeSamples;

/// <summary>
/// Source for Code examples being store in https://github.com/Kontent-ai-Learn/kontent-ai-learn-code-samples/tree/master/net/import-content-model
/// </summary>
public class ImportContentModel : IClassFixture<FileSystemFixture>
{
    // IF YOU MAKE ANY CHANGE TO THIS FILE - ADJUST THE CODE SAMPLES

    private readonly FileSystemFixture _fileSystemFixture;

    public ImportContentModel(FileSystemFixture fileSystemFixture)
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

        var response = await client.CreateContentTypeSnippetAsync(new ContentTypeSnippetCreateModel
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
                    ContentGroup = Reference.ByExternalId("content"),
                    DefaultValue = new TextElementDefaultValueModel {
                        Global = new() {
                            Value = "This is the default value of the text element."
                        }
                    }
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

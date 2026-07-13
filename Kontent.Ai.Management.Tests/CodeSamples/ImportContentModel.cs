using Kontent.Ai.Management.Models.TaxonomyGroups;
using Kontent.Ai.Management.Models.Types;
using Kontent.Ai.Management.Models.Types.Elements;
using Kontent.Ai.Management.Models.Types.Elements.DefaultValues;
using Kontent.Ai.Management.Models.TypeSnippets;
using Kontent.Ai.Management.Tests.Base;

namespace Kontent.Ai.Management.Tests.CodeSamples;

/// <summary>
/// Source for Code examples being store in https://github.com/Kontent-ai-Learn/kontent-ai-learn-code-samples/tree/master/net/import-content-model
/// </summary>
public class ImportContentModel
{
    // IF YOU MAKE ANY CHANGE TO THIS FILE - ADJUST THE CODE SAMPLES

    private const string SampleFolder = "CodeSamples";

    // DocSection: import_model_create_snippet
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async Task CreateSnippet()
    {
        var client = MockClientFactory.CreateForSample(SampleFolder, "Empty.json");

        var response = await client.CreateContentTypeSnippetAsync(new ContentTypeSnippetCreateModel
        {
            Name = "Metadata",
            Codename = "metadata",
            Elements =
            [
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
            ]
        });
    }

    // DocSection: import_model_create_taxonomy
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async Task CreateTaxonomy()
    {
        var client = MockClientFactory.CreateForSample(SampleFolder, "Empty.json");

        var response = await client.CreateTaxonomyGroupAsync(new TaxonomyGroupCreateModel
        {
            Name = "Blogpost topic",
            Codename = "blog_topic",
            Terms =
            [
                new TaxonomyTermCreateModel
                {
                    Name = "Sport",
                    Codename = "sport",
                    Terms =
                    [
                        new TaxonomyTermCreateModel
                        {
                            Name = "Soccer",
                            ExternalId = "soccer",
                        },
                        new TaxonomyTermCreateModel
                        {
                            Name = "Ice hockey",
                            ExternalId = "hockey",
                        },
                        new TaxonomyTermCreateModel
                        {
                            Name = "Rugby",
                            ExternalId = "rugby",
                        },
                    ]
                },
                new TaxonomyTermCreateModel
                {
                    Name = "Technology stack",
                    Codename = "tech",
                    Terms =
                    [
                        new TaxonomyTermCreateModel
                        {
                            Name = "Javascript",
                            ExternalId = "js",
                        },
                        new TaxonomyTermCreateModel
                        {
                            Name = "C#",
                            ExternalId = "c",
                        },
                        new TaxonomyTermCreateModel
                        {
                            Name = "MVC",
                            ExternalId = "mvc",
                        },
                    ]
                },
            ]
        });
    }

    // DocSection: import_model_create_type
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async Task CreateType()
    {
        var client = MockClientFactory.CreateForSample(SampleFolder, "Empty.json");

        var response = await client.CreateContentTypeAsync(new ContentTypeCreateModel
        {
            Name = "Blogpost",
            Codename = "blogpost",
            ContentGroups =
            [
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
            ],
            Elements =
            [
                new TextElementMetadataModel
                {
                    Name = "Title",
                    ContentGroup = Reference.ByExternalId("content"),
                    DefaultValue = new TextElementDefaultValueModel("This is the default value of the text element.")
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
            ]
        });
    }
}

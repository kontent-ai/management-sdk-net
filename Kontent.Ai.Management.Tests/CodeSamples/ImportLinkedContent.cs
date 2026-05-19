using Kontent.Ai.Management.Models.Items;
using Kontent.Ai.Management.Models.LanguageVariants;
using Kontent.Ai.Management.Models.LanguageVariants.Elements;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Modules.ModelBuilders;
using Kontent.Ai.Management.Tests.Base;
using Xunit;

namespace Kontent.Ai.Management.Tests.CodeSamples;

/// <summary>
/// Source for Code examples being store in https://github.com/Kontent-ai-Learn/kontent-ai-learn-code-samples/tree/master/net/import-linked-content
/// </summary>
public class ImportLinkedContent
{
    // IF YOU MAKE ANY CHANGE TO THIS FILE - ADJUST THE CODE SAMPLES

    private const string SampleFolder = "CodeSamples";

    // DocSection: import_linked_create_item
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void CreateItem()
    {
        var client = MockClientFactory.CreateForSample(SampleFolder, "Empty.json");

        await client.UpsertContentItemAsync(
            Reference.ByExternalId("123"),
            new ContentItemUpsertModel { Name = "On Roasts", Type = Reference.ByCodename("article") });
    }

    // DocSection: import_linked_create_sec_item
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void CreateSecondItem()
    {
        var client = MockClientFactory.CreateForSample(SampleFolder, "Empty.json");

        await client.UpsertContentItemAsync(
            Reference.ByExternalId("456"),
            new ContentItemUpsertModel { Name = "Donate with us", Type = Reference.ByCodename("article") });
    }

    // DocSection: import_linked_upsert_Sec_variant
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void UpsertSecondVariant()
    {
        var client = MockClientFactory.CreateForSample(SampleFolder, "Empty.json");

        var identifier = new LanguageVariantIdentifier(Reference.ByExternalId("456"), Reference.ByCodename("en-US"));

        await client.UpsertLanguageVariantAsync(identifier, new LanguageVariantUpsertModel
        {
            Elements = ElementBuilder.GetElementsAsDynamic(new BaseElement[]
            {
                new TextElement
                {
                    Element = Reference.ByCodename("title"),
                    Value = "Donate with us"
                },
                new LinkedItemsElement
                {
                    Element = Reference.ByCodename("related_articles"),
                    Value = new []
                    {
                        Reference.ByExternalId("123"),
                    }
                }
            })
        });
    }

    // DocSection: import_linked_upsert_variant
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void UsertVariant()
    {
        var client = MockClientFactory.CreateForSample(SampleFolder, "Empty.json");

        var identifier = new LanguageVariantIdentifier(Reference.ByExternalId("123"), Reference.ByCodename("en-US"));

        var response = await client.UpsertLanguageVariantAsync(identifier, new LanguageVariantUpsertModel
        {
            Elements = ElementBuilder.GetElementsAsDynamic(new BaseElement[]
            {
                new TextElement
                {
                    Element = Reference.ByCodename("title"),
                    Value = "On Roasts"
                },
                new LinkedItemsElement
                {
                    Element = Reference.ByCodename("related_articles"),
                    Value = new []
                    {
                        Reference.ByExternalId("456"),
                    }
                }
            })
        });
    }

    // DocSection: import_linked_validate_content
    // Tip: Find more about .NET SDKs at https://kontent.ai/learn/net
    [Fact]
    public async void PostValidate()
    {
        var client = MockClientFactory.CreateForSample(SampleFolder, "Empty.json");

        var response = await client.ValidateEnvironmentAsync();
    }
}

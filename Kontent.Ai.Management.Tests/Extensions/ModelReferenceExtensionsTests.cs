using AwesomeAssertions;
using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.Assets;
using Kontent.Ai.Management.Models.Collections;
using Kontent.Ai.Management.Models.Items;
using Kontent.Ai.Management.Models.Languages;
using Kontent.Ai.Management.Models.Spaces;
using Kontent.Ai.Management.Models.TaxonomyGroups;
using Kontent.Ai.Management.Models.Types;
using Kontent.Ai.Management.Models.TypeSnippets;
using Kontent.Ai.Management.Models.Webhooks;
using Kontent.Ai.Management.Models.Workflow;
using Kontent.Ai.Management.Tests.Base;
using System.Text.Json;

namespace Kontent.Ai.Management.Tests.Extensions;

public class ModelReferenceExtensionsTests
{
    private static T Fixture<T>(string folder, string name) =>
        JsonSerializer.Deserialize<T>(
            File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Data", folder, name)),
            SharedTestJsonOptions.Default)!;

    [Fact]
    public void ToReference_ContentItem_ReferencesById()
    {
        var item = Fixture<ContentItemModel>("ContentItem", "ContentItem.json");
        item.ToReference().Should().Be(Reference.ById(item.Id));
    }

    [Fact]
    public void ToReference_Asset_ReferencesById()
    {
        var asset = Fixture<AssetModel>("Asset", "Asset.json");
        asset.ToReference().Should().Be(Reference.ById(asset.Id));
    }

    [Fact]
    public void ToReference_ContentType_ReferencesById()
    {
        var contentType = Fixture<ContentTypeModel>("ContentType", "ContentType.json");
        contentType.ToReference().Should().Be(Reference.ById(contentType.Id));
    }

    [Fact]
    public void ToReference_ContentTypeSnippet_ReferencesById()
    {
        var snippet = Fixture<ContentTypeSnippetModel>("ContentTypeSnippet", "Snippet.json");
        snippet.ToReference().Should().Be(Reference.ById(snippet.Id));
    }

    [Fact]
    public void ToReference_TaxonomyGroup_ReferencesById()
    {
        var taxonomyGroup = Fixture<TaxonomyGroupModel>("TaxonomyGroup", "TaxonomyGroup.json");
        taxonomyGroup.ToReference().Should().Be(Reference.ById(taxonomyGroup.Id));
    }

    [Fact]
    public void ToReference_Language_ReferencesById()
    {
        var language = Fixture<LanguageModel>("Language", "CreateLanguage_CreatesLanguage.json");
        language.ToReference().Should().Be(Reference.ById(language.Id));
    }

    [Fact]
    public void ToReference_Workflow_ReferencesById()
    {
        var workflow = Fixture<WorkflowModel>("Workflow", "Workflow.json");
        workflow.ToReference().Should().Be(Reference.ById(workflow.Id));
    }

    [Fact]
    public void ToReference_Collection_ReferencesById()
    {
        var collection = Fixture<CollectionsModel>("Collection", "Collections.json").Collections[0];
        collection.ToReference().Should().Be(Reference.ById(collection.Id));
    }

    [Fact]
    public void ToReference_Space_ReferencesById()
    {
        var space = Fixture<SpaceModel>("Space", "Space.json");
        space.ToReference().Should().Be(Reference.ById(space.Id));
    }

    [Fact]
    public void ToReference_Webhook_ReferencesById()
    {
        var webhook = Fixture<WebhookModel>("Webhook", "Webhook.json");
        webhook.ToReference().Should().Be(Reference.ById(webhook.Id));
    }
}

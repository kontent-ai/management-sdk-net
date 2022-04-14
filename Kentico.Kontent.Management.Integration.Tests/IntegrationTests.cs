using Kentico.Kontent.Management.Configuration;
using Kentico.Kontent.Management.Models.Environments;
using Kentico.Kontent.Management.Extensions;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using Kentico.Kontent.Management.Models.TaxonomyGroups;
using Kentico.Kontent.Management.Models.Types;
using Kentico.Kontent.Management.Models.Types.Elements;
using Kentico.Kontent.Management.Models.Shared;
using Kentico.Kontent.Management.Models.Items;
using System;
using Kentico.Kontent.Management.Models.LanguageVariants;
using Kentico.Kontent.Management.Modules.ModelBuilders;
using Kentico.Kontent.Management.Models.LanguageVariants.Elements;
using FluentAssertions;
using Kentico.Kontent.Management.Models.Workflow;
using Microsoft.Extensions.Configuration;

namespace Kentico.Kontent.Management.Integration.Tests;

public class IntegrationTests : IAsyncLifetime
{
    private ManagementClient _client;

    public async Task InitializeAsync()
    {
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<IntegrationTests>()
            .Build();

        var subscriptionKey = configuration["APIKey"];
        var subscriptionId = configuration["SubscriptionId"];

        var subscriptionClient = new ManagementClient(new ManagementOptions()
        {
            ApiKey = subscriptionKey,
            ProjectId = "349705c0-5a07-0153-13e5-867eecc3df5f",
            SubscriptionId = subscriptionId
        });

        var project = await subscriptionClient.GetProjectInformationAsync();
        var roles = await subscriptionClient.ListProjectRolesAsync();

        var environment = await subscriptionClient.CloneEnvironmentAsync(new EnvironmentCloneModel
        {
            Name = "Environment for sdk tests",
            RolesToActivate = roles.Roles.Select(x => x.Id).ToList()
        });

        var client = new ManagementClient(new ManagementOptions()
        {
            ApiKey = subscriptionKey,
            ProjectId = environment.Id.ToString(),
            SubscriptionId = subscriptionId
        });

        var state = await client.GetEnvironmentCloningStateAsync();

        while (state.CloningState != CloningState.Done)
        {
            await Task.Delay(5000);
            state = await client.GetEnvironmentCloningStateAsync();
        }

        _client = client;
    }

    public async Task DisposeAsync() => await _client.DeleteEnvironmentAsync();

    [Fact]
    public async Task Simple_Project_Creation()
    {
        var taxonomy = await _client.CreateTaxonomyGroupAsync(new TaxonomyGroupCreateModel
        {
            Codename = "pets",
            ExternalId = "pets_exId",
            Name = "Pets",
            Terms = new TaxonomyTermCreateModel[]
            {
                new TaxonomyTermCreateModel
                {
                    Codename = "dog",
                    Name = "Dog",
                    Terms = Array.Empty<TaxonomyTermCreateModel>()
                },
                new TaxonomyTermCreateModel
                {
                    Codename = "cat",
                    Name = "Cat",
                    Terms = Array.Empty<TaxonomyTermCreateModel>()
                },
            }
        });

        var type = await _client.CreateContentTypeAsync(new ContentTypeCreateModel
        {
            Codename = "article",
            Name = "Article",
            ExternalId = "article_exId",
            Elements = new ElementMetadataBase[]
            {
                new TextElementMetadataModel
                {
                    Name = "Summary",
                    IsRequired = true,
                    Guidelines = "Brief description of you",
                    Codename = "summary"
                },
                new TaxonomyElementMetadataModel
                {
                    Codename = "pets_tax",
                    IsRequired = true,
                    TaxonomyGroup = Reference.ByExternalId("pets_exId"),
                    Guidelines = "Are you dog or cat person"
                }
            }
        });

        var item = await _client.CreateContentItemAsync(new ContentItemCreateModel
        {
            Codename = "just_item",
            ExternalId = "just_item_exId",
            Type = Reference.ByExternalId("article_exId"),
            Name = "Article",
            Collection = Reference.ById(Guid.Empty)
        });

        var language = await _client.ListLanguagesAsync().GetAllAsync();

        var variantIdentifier = new LanguageVariantIdentifier(Reference.ByExternalId("just_item_exId"), Reference.ById(language.First().Id));

        var variant = await _client.UpsertLanguageVariantAsync(variantIdentifier, new LanguageVariantUpsertModel
        {
            Elements = ElementBuilder.GetElementsAsDynamic(new BaseElement[]
            {
                new TextElement
                {
                    Element = Reference.ByCodename("summary"),
                    Value = "I am a dog person"
                },
                new TaxonomyElement
                {
                    Element = Reference.ByCodename("pets_tax"),
                    Value = new Reference[] { Reference.ByCodename("dog")}
                }
            })
        });

        var variants = await _client.ListLanguageVariantsByItemAsync(Reference.ByExternalId("just_item_exId"));
        var taxonomies = await _client.ListTaxonomyGroupsAsync().GetAllAsync();
        var types = await _client.ListContentTypesAsync().GetAllAsync();

        //Assert
        types.Should().BeEquivalentTo(new[] { type });
        taxonomies.Should().BeEquivalentTo(new[] { taxonomy });
        variants.Should().BeEquivalentTo(new[] { variant });
    }
}

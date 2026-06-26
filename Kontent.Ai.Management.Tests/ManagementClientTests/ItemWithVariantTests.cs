using AwesomeAssertions;
using Kontent.Ai.Management.Models.ItemWithVariant;
using Kontent.Ai.Management.Models.VariantFilter;
using Kontent.Ai.Management.Tests.Base;
using RichardSzalay.MockHttp;

using static Kontent.Ai.Management.Tests.Base.PagedFixtures;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class ItemWithVariantTests
{
    private static string Fixture(string name)
        => File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Data", "ItemWithVariant", name));

    [Fact]
    public async Task ListItemsWithVariantsByFilterAsync_WithValidRequest_ReturnsFilterResults()
    {
        var (client, mock) = MockClientFactory.Create();
        mock.Expect(HttpMethod.Post, $"{MockClientFactory.BaseUrl}/items-with-variant/filter")
            .Respond("application/json", Fixture("FilterResponse.json"));

        var request = new ItemWithVariantFilterRequestModel
        {
            Filters = new VariantFilterFiltersModel
            {
                SearchPhrase = "test",
                Language = Reference.ByCodename("en-US"),
                ContentTypes = new List<Reference>
                {
                    Reference.ByCodename("article")
                },
                CompletionStatuses = new List<VariantFilterCompletionStatus> { VariantFilterCompletionStatus.Ready }
            },
            Order = new VariantFilterOrderModel
            {
                By = VariantFilterOrderColumn.Name,
                Direction = VariantFilterOrderDirection.Ascending
            }
        };

        var listResult = await client.ListItemsWithVariantsByFilterAsync(request);
        listResult.IsSuccess.Should().BeTrue();
        IReadOnlyList<ItemWithVariantFilterResultModel> items = listResult.Value;

        mock.VerifyNoOutstandingExpectation();
        items.Should().HaveCount(2);

        items[0].Item.Should().NotBeNull();
        items[0].Item.Id.Should().Be(new Guid("4b628214-e4fe-4fe0-b1ff-955df33e1515"));
        items[0].Language.Should().NotBeNull();
        items[0].Language.Id.Should().Be(Guid.Empty);

        items[1].Item.Should().NotBeNull();
        items[1].Item.Id.Should().Be(new Guid("6a8b4d04-7d3e-4d3c-8b9a-4c7e8f9a1b2c"));
        items[1].Language.Should().NotBeNull();
        items[1].Language.Id.Should().Be(new Guid("d1f95fde-af02-b3b5-bd9e-f232311ccab8"));
    }

    [Fact]
    public async Task ListItemsWithVariantsByFilterAsync_WithNullRequest_ThrowsArgumentNullException()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.ListItemsWithVariantsByFilterAsync(null!)).Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task ListItemsWithVariantsByFilterAsync_WithPagination_PagesThroughAllPages()
    {
        var (client, mock) = MockClientFactory.Create();
        var firstPage = Fixture("FilterResponseFirstPage.json");
        var lastPage = Fixture("FilterResponseLastPage.json");
        mock.Expect(HttpMethod.Post, $"{MockClientFactory.BaseUrl}/items-with-variant/filter")
            .Respond("application/json", firstPage);
        mock.Expect(HttpMethod.Post, $"{MockClientFactory.BaseUrl}/items-with-variant/filter")
            .Respond("application/json", lastPage);

        var request = new ItemWithVariantFilterRequestModel
        {
            Filters = new VariantFilterFiltersModel
            {
                Language = Reference.ByCodename("en-US")
            }
        };

        var listResult = await client.ListItemsWithVariantsByFilterAsync(request);
        listResult.IsSuccess.Should().BeTrue();
        IReadOnlyList<ItemWithVariantFilterResultModel> items = listResult.Value;

        mock.VerifyNoOutstandingExpectation();
        items.Should().BeEquivalentTo(ConcatPages<ItemWithVariantFilterResultModel>(firstPage, lastPage));
    }

    [Fact]
    public async Task EnumerateItemsWithVariantsByFilterPagesAsync_StreamsAllPages()
    {
        var (client, mock) = MockClientFactory.Create();
        var firstPage = Fixture("FilterResponseFirstPage.json");
        var lastPage = Fixture("FilterResponseLastPage.json");
        mock.Expect(HttpMethod.Post, $"{MockClientFactory.BaseUrl}/items-with-variant/filter")
            .Respond("application/json", firstPage);
        mock.Expect(HttpMethod.Post, $"{MockClientFactory.BaseUrl}/items-with-variant/filter")
            .Respond("application/json", lastPage);

        var request = new ItemWithVariantFilterRequestModel
        {
            Filters = new VariantFilterFiltersModel { Language = Reference.ByCodename("en-US") }
        };

        var items = new List<ItemWithVariantFilterResultModel>();
        await foreach (var page in client.EnumerateItemsWithVariantsByFilterPagesAsync(request))
        {
            page.IsSuccess.Should().BeTrue();
            items.AddRange(page.Value);
        }

        mock.VerifyNoOutstandingExpectation();
        items.Should().BeEquivalentTo(ConcatPages<ItemWithVariantFilterResultModel>(firstPage, lastPage));
    }

    [Fact]
    public void EnumerateItemsWithVariantsByFilterPagesAsync_WithNullRequest_ThrowsArgumentNullException()
    {
        // The null guard is eager (the method is not an iterator), so the call throws synchronously before enumeration.
        var (client, _) = MockClientFactory.Create();

        client.Invoking(x => x.EnumerateItemsWithVariantsByFilterPagesAsync(null!))
            .Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void EnumerateItemsWithVariantsByBulkGetPagesAsync_WithNullRequest_ThrowsArgumentNullException()
    {
        var (client, _) = MockClientFactory.Create();

        client.Invoking(x => x.EnumerateItemsWithVariantsByBulkGetPagesAsync(null!))
            .Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public async Task ListItemsWithVariantsByFilterAsync_LastPage_StopsAfterOnePage()
    {
        var (client, mock) = MockClientFactory.Create();
        mock.Expect(HttpMethod.Post, $"{MockClientFactory.BaseUrl}/items-with-variant/filter")
            .Respond("application/json", Fixture("FilterResponseLastPage.json"));

        var request = new ItemWithVariantFilterRequestModel
        {
            Filters = new VariantFilterFiltersModel
            {
                Language = Reference.ByCodename("en-US")
            }
        };

        var listResult = await client.ListItemsWithVariantsByFilterAsync(request);
        listResult.IsSuccess.Should().BeTrue();
        IReadOnlyList<ItemWithVariantFilterResultModel> items = listResult.Value;

        mock.VerifyNoOutstandingExpectation();
        items.Should().HaveCount(1);
    }

    [Fact]
    public async Task ListItemsWithVariantsByFilterAsync_WithComplexFilters_ReturnsResults()
    {
        var (client, mock) = MockClientFactory.Create();
        mock.Expect(HttpMethod.Post, $"{MockClientFactory.BaseUrl}/items-with-variant/filter")
            .Respond("application/json", Fixture("FilterResponse.json"));

        var request = new ItemWithVariantFilterRequestModel
        {
            Filters = new VariantFilterFiltersModel
            {
                Language = Reference.ByCodename("en-US"),
                ContentTypes = new List<Reference>
                {
                    Reference.ByCodename("article"),
                    Reference.ByCodename("blog_post")
                },
                Contributors = new List<UserIdentifier>
                {
                    UserIdentifier.ByEmail("user@example.com")
                },
                CompletionStatuses = new List<VariantFilterCompletionStatus>
                {
                    VariantFilterCompletionStatus.Ready,
                    VariantFilterCompletionStatus.Unfinished
                },
                WorkflowSteps = new List<VariantFilterWorkflowStepsModel>
                {
                    new VariantFilterWorkflowStepsModel
                    {
                        WorkflowReference = Reference.ByCodename("default"),
                        WorkflowStepReferences = new List<Reference>
                        {
                            Reference.ByCodename("draft")
                        }
                    }
                },
                TaxonomyGroups = new List<VariantFilterTaxonomyGroupModel>
                {
                    new VariantFilterTaxonomyGroupModel
                    {
                        TaxonomyReference = Reference.ByCodename("categories"),
                        TermReferences = new List<Reference>
                        {
                            Reference.ByCodename("tech")
                        },
                        IncludeUncategorized = false
                    }
                }
            },
            Order = new VariantFilterOrderModel
            {
                By = VariantFilterOrderColumn.LastModified,
                Direction = VariantFilterOrderDirection.Descending
            }
        };

        var listResult = await client.ListItemsWithVariantsByFilterAsync(request);
        listResult.IsSuccess.Should().BeTrue();
        IReadOnlyList<ItemWithVariantFilterResultModel> items = listResult.Value;

        mock.VerifyNoOutstandingExpectation();
        items.Should().HaveCount(2);
    }

    [Fact]
    public async Task ListItemsWithVariantsByFilterAsync_WithSpacesFilter_ReturnsResults()
    {
        var (client, mock) = MockClientFactory.Create();
        mock.Expect(HttpMethod.Post, $"{MockClientFactory.BaseUrl}/items-with-variant/filter")
            .Respond("application/json", Fixture("FilterResponse.json"));

        var request = new ItemWithVariantFilterRequestModel
        {
            Filters = new VariantFilterFiltersModel
            {
                Language = Reference.ByCodename("en-US"),
                Spaces = new List<Reference>
                {
                    Reference.ByCodename("default"),
                    Reference.ById(new Guid("4b628214-e4fe-4fe0-b1ff-955df33e1515"))
                }
            }
        };

        var listResult = await client.ListItemsWithVariantsByFilterAsync(request);
        listResult.IsSuccess.Should().BeTrue();
        IReadOnlyList<ItemWithVariantFilterResultModel> items = listResult.Value;

        mock.VerifyNoOutstandingExpectation();
        items.Should().HaveCount(2);
    }

    [Fact]
    public async Task ListItemsWithVariantsByFilterAsync_WithCollectionsFilter_ReturnsResults()
    {
        var (client, mock) = MockClientFactory.Create();
        mock.Expect(HttpMethod.Post, $"{MockClientFactory.BaseUrl}/items-with-variant/filter")
            .Respond("application/json", Fixture("FilterResponse.json"));

        var request = new ItemWithVariantFilterRequestModel
        {
            Filters = new VariantFilterFiltersModel
            {
                Language = Reference.ByCodename("en-US"),
                Collections = new List<Reference>
                {
                    Reference.ByCodename("default"),
                    Reference.ByExternalId("external-collection-1")
                }
            }
        };

        var listResult = await client.ListItemsWithVariantsByFilterAsync(request);
        listResult.IsSuccess.Should().BeTrue();
        IReadOnlyList<ItemWithVariantFilterResultModel> items = listResult.Value;

        mock.VerifyNoOutstandingExpectation();
        items.Should().HaveCount(2);
    }

    [Fact]
    public async Task ListItemsWithVariantsByFilterAsync_WithPublishingStatesFilter_ReturnsResults()
    {
        var (client, mock) = MockClientFactory.Create();
        mock.Expect(HttpMethod.Post, $"{MockClientFactory.BaseUrl}/items-with-variant/filter")
            .Respond("application/json", Fixture("FilterResponse.json"));

        var request = new ItemWithVariantFilterRequestModel
        {
            Filters = new VariantFilterFiltersModel
            {
                Language = Reference.ByCodename("en-US"),
                PublishingStates = new List<VariantFilterPublishingState>
                {
                    VariantFilterPublishingState.Published,
                    VariantFilterPublishingState.Unpublished
                }
            }
        };

        var listResult = await client.ListItemsWithVariantsByFilterAsync(request);
        listResult.IsSuccess.Should().BeTrue();
        IReadOnlyList<ItemWithVariantFilterResultModel> items = listResult.Value;

        mock.VerifyNoOutstandingExpectation();
        items.Should().HaveCount(2);
    }

    [Fact]
    public async Task ListItemsWithVariantsByFilterAsync_WithAllNewFilters_ReturnsResults()
    {
        var (client, mock) = MockClientFactory.Create();
        mock.Expect(HttpMethod.Post, $"{MockClientFactory.BaseUrl}/items-with-variant/filter")
            .Respond("application/json", Fixture("FilterResponse.json"));

        var request = new ItemWithVariantFilterRequestModel
        {
            Filters = new VariantFilterFiltersModel
            {
                Language = Reference.ByCodename("en-US"),
                Spaces = new List<Reference>
                {
                    Reference.ByCodename("default")
                },
                Collections = new List<Reference>
                {
                    Reference.ById(new Guid("4b628214-e4fe-4fe0-b1ff-955df33e1515"))
                },
                PublishingStates = new List<VariantFilterPublishingState>
                {
                    VariantFilterPublishingState.Published,
                    VariantFilterPublishingState.Unpublished,
                    VariantFilterPublishingState.NotPublishedYet
                }
            }
        };

        var listResult = await client.ListItemsWithVariantsByFilterAsync(request);
        listResult.IsSuccess.Should().BeTrue();
        IReadOnlyList<ItemWithVariantFilterResultModel> items = listResult.Value;

        mock.VerifyNoOutstandingExpectation();
        items.Should().HaveCount(2);
    }

    [Fact]
    public async Task ListItemsWithVariantsByFilterAsync_WithComponentTypesFilter_ReturnsResults()
    {
        var (client, mock) = MockClientFactory.Create();
        mock.Expect(HttpMethod.Post, $"{MockClientFactory.BaseUrl}/items-with-variant/filter")
            .Respond("application/json", Fixture("FilterResponse.json"));

        var request = new ItemWithVariantFilterRequestModel
        {
            Filters = new VariantFilterFiltersModel
            {
                Language = Reference.ByCodename("en-US"),
                ComponentTypes = new List<Reference>
                {
                    Reference.ByCodename("banner"),
                    Reference.ById(new Guid("4b628214-e4fe-4fe0-b1ff-955df33e1515"))
                }
            }
        };

        var listResult = await client.ListItemsWithVariantsByFilterAsync(request);
        listResult.IsSuccess.Should().BeTrue();
        IReadOnlyList<ItemWithVariantFilterResultModel> items = listResult.Value;

        mock.VerifyNoOutstandingExpectation();
        items.Should().HaveCount(2);
    }

    [Fact]
    public async Task ListItemsWithVariantsByBulkGetAsync_WithValidRequest_ReturnsItemsWithVariants()
    {
        var (client, mock) = MockClientFactory.Create();
        mock.Expect(HttpMethod.Post, $"{MockClientFactory.BaseUrl}/items-with-variant/bulk-get")
            .Respond("application/json", Fixture("BulkGetResponse.json"));

        var request = new ItemWithVariantBulkGetRequestModel
        {
            Variants = new List<VariantIdentifierModel>
            {
                new VariantIdentifierModel
                {
                    Item = Reference.ById(new Guid("4b628214-e4fe-4fe0-b1ff-955df33e1515")),
                    Language = Reference.ById(Guid.Empty)
                },
                new VariantIdentifierModel
                {
                    Item = Reference.ById(new Guid("6a8b4d04-7d3e-4d3c-8b9a-4c7e8f9a1b2c")),
                    Language = Reference.ByCodename("en-US")
                }
            }
        };

        var listResult = await client.ListItemsWithVariantsByBulkGetAsync(request);
        listResult.IsSuccess.Should().BeTrue();
        IReadOnlyList<ContentItemWithVariantModel> items = listResult.Value;

        mock.VerifyNoOutstandingExpectation();
        items.Should().HaveCount(2);

        items[0].Item.Should().NotBeNull();
        items[0].Item.Id.Should().Be(new Guid("4b628214-e4fe-4fe0-b1ff-955df33e1515"));
        items[0].Item.Name.Should().Be("Sample Article");
        items[0].Variant.Should().NotBeNull();
        items[0].Variant!.Language.Id.Should().Be(Guid.Empty);
        items[0].Variant!.Elements.Should().NotBeNull();

        items[1].Item.Should().NotBeNull();
        items[1].Item.Id.Should().Be(new Guid("6a8b4d04-7d3e-4d3c-8b9a-4c7e8f9a1b2c"));
        items[1].Item.Name.Should().Be("Another Article");
        items[1].Variant.Should().BeNull();
    }

    [Fact]
    public async Task ListItemsWithVariantsByBulkGetAsync_WithNullRequest_ThrowsArgumentNullException()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.ListItemsWithVariantsByBulkGetAsync(null!)).Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task ListItemsWithVariantsByBulkGetAsync_WithCodenames_ReturnsItemsWithVariants()
    {
        var (client, mock) = MockClientFactory.Create();
        mock.Expect(HttpMethod.Post, $"{MockClientFactory.BaseUrl}/items-with-variant/bulk-get")
            .Respond("application/json", Fixture("BulkGetResponse.json"));

        var request = new ItemWithVariantBulkGetRequestModel
        {
            Variants = new List<VariantIdentifierModel>
            {
                new VariantIdentifierModel
                {
                    Item = Reference.ByCodename("sample_article"),
                    Language = Reference.ByCodename("en-US")
                },
                new VariantIdentifierModel
                {
                    Item = Reference.ByCodename("another_article"),
                    Language = Reference.ByCodename("en-US")
                }
            }
        };

        var listResult = await client.ListItemsWithVariantsByBulkGetAsync(request);
        listResult.IsSuccess.Should().BeTrue();
        IReadOnlyList<ContentItemWithVariantModel> items = listResult.Value;

        mock.VerifyNoOutstandingExpectation();
        items.Should().HaveCount(2);
        items[0].Item.Should().NotBeNull();
        items[0].Item.Name.Should().Be("Sample Article");
        items[1].Item.Should().NotBeNull();
        items[1].Item.Name.Should().Be("Another Article");
    }
}

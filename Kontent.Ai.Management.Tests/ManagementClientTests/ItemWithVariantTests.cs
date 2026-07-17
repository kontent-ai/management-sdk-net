using AwesomeAssertions;
using Kontent.Ai.Management.Models.ItemWithVariant;
using Kontent.Ai.Management.Models.VariantFilter;
using Kontent.Ai.Management.Tests.Base;
using RichardSzalay.MockHttp;
using System.Text.Json.Nodes;

using static Kontent.Ai.Management.Tests.Base.PagedFixtures;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class ItemWithVariantTests
{
    // Must match "pagination.continuation_token" in the corresponding *FirstPage.json fixture.
    private const string FilterContinuationToken = "G5QAGBSh0hf0vP7kLAbXqbPOvADBBpwQJFRAPNkFQUYi2BGE4QfuHRQQGuwq";
    private const string BulkGetContinuationToken = "K9SBHDUj2jh2xR9nNCdZsdRQxCFDDrySLHTCROmHSWZk4DIG6ShwJSSSIwys";

    private static string Fixture(string name)
        => File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Data", "ItemWithVariant", name));

    [Fact]
    public async Task ListItemsWithVariantsByFilterAsync_WithAllFilterFacets_SendsFilterBodyAndReturnsResults()
    {
        var (client, mock) = MockClientFactory.Create();
        mock.Expect(HttpMethod.Post, $"{MockClientFactory.BaseUrl}/items-with-variant/filter")
            .CaptureBody(out var capturedBody)
            .Respond("application/json", Fixture("FilterResponse.json"));

        var request = new ItemWithVariantFilterRequestModel
        {
            Filters = new VariantFilterFiltersModel
            {
                SearchPhrase = "test",
                Language = Reference.ByCodename("en-US"),
                ContentTypes =
                [
                    Reference.ByCodename("article"),
                    Reference.ByCodename("blog_post")
                ],
                Contributors =
                [
                    UserIdentifier.ByEmail("user@example.com"),
                    UserIdentifier.ById("d94bc87a-c066-48a1-87ac-8dbb9f28ba86")
                ],
                HasNoContributors = false,
                CompletionStatuses =
                [
                    VariantFilterCompletionStatus.Unfinished,
                    VariantFilterCompletionStatus.Ready,
                    VariantFilterCompletionStatus.NotTranslated,
                    VariantFilterCompletionStatus.AllDone
                ],
                WorkflowSteps =
                [
                    new VariantFilterWorkflowStepsModel
                    {
                        Workflow = Reference.ByCodename("default"),
                        Steps = [Reference.ByCodename("draft")]
                    }
                ],
                TaxonomyGroups =
                [
                    new VariantFilterTaxonomyGroupModel
                    {
                        TaxonomyGroup = Reference.ByCodename("categories"),
                        Terms = [Reference.ByCodename("tech")],
                        IncludeUncategorized = true
                    }
                ],
                Spaces =
                [
                    Reference.ByCodename("default"),
                    Reference.ById(new Guid("4b628214-e4fe-4fe0-b1ff-955df33e1515"))
                ],
                Collections =
                [
                    Reference.ByCodename("default"),
                    Reference.ByExternalId("external-collection-1")
                ],
                PublishingStates =
                [
                    VariantFilterPublishingState.Published,
                    VariantFilterPublishingState.Unpublished,
                    VariantFilterPublishingState.NotPublishedYet
                ],
                ComponentTypes =
                [
                    Reference.ByCodename("banner"),
                    Reference.ById(new Guid("4b628214-e4fe-4fe0-b1ff-955df33e1515"))
                ]
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
        capturedBody.ShouldMatchSerialized(request);

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
    public async Task ListItemsWithVariantsByFilterAsync_WithMinimalFilter_OmitsUnsetFacetsFromBody()
    {
        var (client, mock) = MockClientFactory.Create();
        mock.Expect(HttpMethod.Post, $"{MockClientFactory.BaseUrl}/items-with-variant/filter")
            .CaptureBody(out var capturedBody)
            .Respond("application/json", Fixture("FilterResponse.json"));

        var request = new ItemWithVariantFilterRequestModel
        {
            Filters = new VariantFilterFiltersModel { Language = Reference.ByCodename("en-US") }
        };

        var listResult = await client.ListItemsWithVariantsByFilterAsync(request);
        listResult.IsSuccess.Should().BeTrue();

        mock.VerifyNoOutstandingExpectation();
        var body = JsonNode.Parse(capturedBody.Value!)!.AsObject();
        body.Select(p => p.Key).Should().Equal("filters");
        body["filters"]!.AsObject().Select(p => p.Key).Should().Equal("language");
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
            .WithHeaders("x-continuation", FilterContinuationToken)
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
            .WithHeaders("x-continuation", FilterContinuationToken)
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
    public async Task ListItemsWithVariantsByBulkGetAsync_WithValidRequest_SendsIdentifiersAndReturnsItemsWithVariants()
    {
        var (client, mock) = MockClientFactory.Create();
        mock.Expect(HttpMethod.Post, $"{MockClientFactory.BaseUrl}/items-with-variant/bulk-get")
            .CaptureBody(out var capturedBody)
            .Respond("application/json", Fixture("BulkGetResponse.json"));

        var request = new ItemWithVariantBulkGetRequestModel
        {
            Variants =
            [
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
            ]
        };

        var listResult = await client.ListItemsWithVariantsByBulkGetAsync(request);
        listResult.IsSuccess.Should().BeTrue();
        IReadOnlyList<ContentItemWithVariantModel> items = listResult.Value;

        mock.VerifyNoOutstandingExpectation();
        capturedBody.ShouldMatchSerialized(request);

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
            Variants =
            [
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
            ]
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

    [Fact]
    public async Task ListItemsWithVariantsByBulkGetAsync_WithPagination_PagesThroughAllPages()
    {
        var (client, mock) = MockClientFactory.Create();
        var firstPage = Fixture("BulkGetResponseFirstPage.json");
        var lastPage = Fixture("BulkGetResponseLastPage.json");
        mock.Expect(HttpMethod.Post, $"{MockClientFactory.BaseUrl}/items-with-variant/bulk-get")
            .Respond("application/json", firstPage);
        mock.Expect(HttpMethod.Post, $"{MockClientFactory.BaseUrl}/items-with-variant/bulk-get")
            .WithHeaders("x-continuation", BulkGetContinuationToken)
            .Respond("application/json", lastPage);

        var request = new ItemWithVariantBulkGetRequestModel
        {
            Variants =
            [
                new VariantIdentifierModel
                {
                    Item = Reference.ById(new Guid("4b628214-e4fe-4fe0-b1ff-955df33e1515")),
                    Language = Reference.ById(Guid.Empty)
                }
            ]
        };

        var listResult = await client.ListItemsWithVariantsByBulkGetAsync(request);
        listResult.IsSuccess.Should().BeTrue();
        IReadOnlyList<ContentItemWithVariantModel> items = listResult.Value;

        mock.VerifyNoOutstandingExpectation();
        items.ShouldEqualAsJson(ConcatPages<ContentItemWithVariantModel>(firstPage, lastPage));
    }

    [Fact]
    public async Task EnumerateItemsWithVariantsByBulkGetPagesAsync_StreamsAllPages()
    {
        var (client, mock) = MockClientFactory.Create();
        var firstPage = Fixture("BulkGetResponseFirstPage.json");
        var lastPage = Fixture("BulkGetResponseLastPage.json");
        mock.Expect(HttpMethod.Post, $"{MockClientFactory.BaseUrl}/items-with-variant/bulk-get")
            .Respond("application/json", firstPage);
        mock.Expect(HttpMethod.Post, $"{MockClientFactory.BaseUrl}/items-with-variant/bulk-get")
            .WithHeaders("x-continuation", BulkGetContinuationToken)
            .Respond("application/json", lastPage);

        var request = new ItemWithVariantBulkGetRequestModel
        {
            Variants =
            [
                new VariantIdentifierModel
                {
                    Item = Reference.ById(new Guid("4b628214-e4fe-4fe0-b1ff-955df33e1515")),
                    Language = Reference.ById(Guid.Empty)
                }
            ]
        };

        var items = new List<ContentItemWithVariantModel>();
        await foreach (var page in client.EnumerateItemsWithVariantsByBulkGetPagesAsync(request))
        {
            page.IsSuccess.Should().BeTrue();
            items.AddRange(page.Value);
        }

        mock.VerifyNoOutstandingExpectation();
        items.ShouldEqualAsJson(ConcatPages<ContentItemWithVariantModel>(firstPage, lastPage));
    }

    [Fact]
    public void EnumerateItemsWithVariantsByBulkGetPagesAsync_WithNullRequest_ThrowsArgumentNullException()
    {
        var (client, _) = MockClientFactory.Create();

        client.Invoking(x => x.EnumerateItemsWithVariantsByBulkGetPagesAsync(null!))
            .Should().ThrowExactly<ArgumentNullException>();
    }
}

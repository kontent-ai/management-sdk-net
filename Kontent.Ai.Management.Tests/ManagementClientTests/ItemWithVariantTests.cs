using FluentAssertions;
using Kontent.Ai.Management.Models.ItemWithVariant;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.VariantFilter;
using Kontent.Ai.Management.Tests.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class ItemWithVariantTests : IClassFixture<FileSystemFixture>
{
    private readonly FileSystemFixture _fileSystemFixture;

    public ItemWithVariantTests(FileSystemFixture fileSystemFixture)
    {
        _fileSystemFixture = fileSystemFixture;
        _fileSystemFixture.SetSubFolder("ItemWithVariant");
    }

    [Fact]
    public async Task FilterItemsWithVariantsAsync_WithValidRequest_ReturnsFilterResults()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("FilterResponse.json");

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
                By = "name",
                Direction = VariantFilterOrderDirection.Ascending
            }
        };

        var response = await client.FilterItemsWithVariantsAsync(request);

        response.Should().NotBeNull();
        response.Should().BeAssignableTo<IListingResponseModel<ItemWithVariantFilterResultModel>>();

        var items = response.ToList();
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
    public async Task FilterItemsWithVariantsAsync_WithNullRequest_ThrowsArgumentNullException()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("FilterResponse.json");

        await Assert.ThrowsAsync<ArgumentNullException>(async () => await client.FilterItemsWithVariantsAsync(null));
    }

    [Fact]
    public async Task FilterItemsWithVariantsAsync_WithPagination_HasNextPage()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("FilterResponse.json");

        var request = new ItemWithVariantFilterRequestModel
        {
            Filters = new VariantFilterFiltersModel
            {
                Language = Reference.ByCodename("en-US")
            }
        };

        var response = await client.FilterItemsWithVariantsAsync(request);

        response.Should().NotBeNull();
        response.HasNextPage().Should().BeTrue();
    }

    [Fact]
    public async Task FilterItemsWithVariantsAsync_LastPage_HasNoNextPage()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("FilterResponseLastPage.json");

        var request = new ItemWithVariantFilterRequestModel
        {
            Filters = new VariantFilterFiltersModel
            {
                Language = Reference.ByCodename("en-US")
            }
        };

        var response = await client.FilterItemsWithVariantsAsync(request);

        response.Should().NotBeNull();
        response.HasNextPage().Should().BeFalse();
    }

    [Fact]
    public async Task FilterItemsWithVariantsAsync_WithComplexFilters_ReturnsResults()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("FilterResponse.json");

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
                By = "last_modified",
                Direction = VariantFilterOrderDirection.Descending
            }
        };

        var response = await client.FilterItemsWithVariantsAsync(request);

        response.Should().NotBeNull();
        response.Should().BeAssignableTo<IListingResponseModel<ItemWithVariantFilterResultModel>>();
        response.ToList().Should().HaveCount(2);
    }

    [Fact]
    public async Task BulkGetItemsWithVariantsAsync_WithValidRequest_ReturnsItemsWithVariants()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("BulkGetResponse.json");

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

        var response = await client.BulkGetItemsWithVariantsAsync(request);

        response.Should().NotBeNull();
        response.Should().BeAssignableTo<IListingResponseModel<ContentItemWithVariantModel>>();

        var items = response.ToList();
        items.Should().HaveCount(2);

        items[0].Item.Should().NotBeNull();
        items[0].Item.Id.Should().Be(new Guid("4b628214-e4fe-4fe0-b1ff-955df33e1515"));
        items[0].Item.Name.Should().Be("Sample Article");
        items[0].Variant.Should().NotBeNull();
        items[0].Variant.Language.Id.Should().Be(Guid.Empty);
        items[0].Variant.Elements.Should().NotBeNull();

        items[1].Item.Should().NotBeNull();
        items[1].Item.Id.Should().Be(new Guid("6a8b4d04-7d3e-4d3c-8b9a-4c7e8f9a1b2c"));
        items[1].Item.Name.Should().Be("Another Article");
        items[1].Variant.Should().BeNull();
    }

    [Fact]
    public async Task BulkGetItemsWithVariantsAsync_WithNullRequest_ThrowsArgumentNullException()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("BulkGetResponse.json");

        await Assert.ThrowsAsync<ArgumentNullException>(async () => await client.BulkGetItemsWithVariantsAsync(null));
    }

    [Fact]
    public async Task BulkGetItemsWithVariantsAsync_WithCodenames_ReturnsItemsWithVariants()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("BulkGetResponse.json");

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

        var response = await client.BulkGetItemsWithVariantsAsync(request);

        response.Should().NotBeNull();
        response.Should().BeAssignableTo<IListingResponseModel<ContentItemWithVariantModel>>();

        var items = response.ToList();
        items.Should().HaveCount(2);
        items[0].Item.Should().NotBeNull();
        items[0].Item.Name.Should().Be("Sample Article");
        items[1].Item.Should().NotBeNull();
        items[1].Item.Name.Should().Be("Another Article");
    }
}

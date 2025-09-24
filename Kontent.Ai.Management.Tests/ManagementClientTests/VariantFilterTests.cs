using FluentAssertions;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.VariantFilter;
using Kontent.Ai.Management.Tests.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class VariantFilterTests : IClassFixture<FileSystemFixture>
{
    private readonly FileSystemFixture _fileSystemFixture;

    public VariantFilterTests(FileSystemFixture fileSystemFixture)
    {
        _fileSystemFixture = fileSystemFixture;
        _fileSystemFixture.SetSubFolder("VariantFilter");
    }

    [Fact]
    public async Task FilterVariantsAsync_WithValidRequest_ReturnsVariantFilterListingResponseServerModel()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("VariantFilter.json");

        var request = new VariantFilterRequestModel
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

        var response = await client.EarlyAccess.FilterVariantsAsync(request);

        response.Should().NotBeNull();
        response.Should().BeAssignableTo<IListingResponseModel<VariantFilterItemModel>>();
        
        var firstItem = response.ToList().First();
        firstItem.Item.Should().NotBeNull();
        firstItem.Item.Id.Should().NotBeEmpty();
        firstItem.Item.Name.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task FilterVariantsAsync_WithNullRequest_ThrowsArgumentNullException()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("VariantFilter.json");

        await Assert.ThrowsAsync<ArgumentNullException>(async () => await client.EarlyAccess.FilterVariantsAsync(null));
    }

    [Fact]
    public async Task FilterVariantsAsync_WithMinimalRequest_ReturnsVariantFilterListingResponseServerModel()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("VariantFilter.json");

        var request = new VariantFilterRequestModel
        {
            Filters = new VariantFilterFiltersModel
            {
                Language = Reference.ByCodename("en-US")
            }
        };

        var response = await client.EarlyAccess.FilterVariantsAsync(request);

        response.Should().NotBeNull();
        response.Should().BeAssignableTo<IListingResponseModel<VariantFilterItemModel>>();
        
        var firstItem = response.ToList().First();
        firstItem.Item.Should().NotBeNull();
        firstItem.Item.Id.Should().NotBeEmpty();
        firstItem.Item.Name.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task FilterVariantsAsync_WithComplexFilters_ReturnsVariantFilterListingResponseServerModel()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("VariantFilter.json");

        var request = new VariantFilterRequestModel
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
                CompletionStatuses = new List<VariantFilterCompletionStatus> { VariantFilterCompletionStatus.Ready, VariantFilterCompletionStatus.Unfinished },
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

        var response = await client.EarlyAccess.FilterVariantsAsync(request);

        response.Should().NotBeNull();
        response.Should().BeAssignableTo<IListingResponseModel<VariantFilterItemModel>>();
        
        var items = response.ToList();
        items.Should().HaveCount(2);
        
        var firstItem = items.First();
        firstItem.Item.Should().NotBeNull();
        firstItem.Item.Id.Should().NotBeEmpty();
        firstItem.Item.Name.Should().NotBeNullOrEmpty();
        
        // Check the second item that has variant data
        var secondItem = items.Skip(1).First();
        secondItem.Item.Should().NotBeNull();
        secondItem.Item.Id.Should().Be(new Guid("6a8b4d04-7d3e-4d3c-8b9a-4c7e8f9a1b2c"));
        secondItem.Item.Name.Should().Be("Another Article");
        secondItem.Item.Codename.Should().Be("another_article");
        secondItem.Item.ExternalId.Should().Be("ext_another_article");
        
        // Assert variant properties
        secondItem.Variant.Should().NotBeNull();
        secondItem.Variant.Language.Should().NotBeNull();
        secondItem.Variant.Language.Codename.Should().Be("en-US");
        secondItem.Variant.Item.Should().NotBeNull();
        secondItem.Variant.Item.Codename.Should().Be("another_article");
        secondItem.Variant.LastModified.Should().NotBeNull();
    }

    [Fact]
    public async Task FilterVariantsAsync_WithIncludeContentFalse_ReturnsVariantWithoutElements()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("VariantFilterWithoutElements.json");

        var request = new VariantFilterRequestModel
        {
            Filters = new VariantFilterFiltersModel
            {
                Language = Reference.ByCodename("en-US")
            },
            IncludeContent = false
        };

        var response = await client.EarlyAccess.FilterVariantsAsync(request);

        response.Should().NotBeNull();
        response.Should().BeAssignableTo<IListingResponseModel<VariantFilterItemModel>>();
        
        var firstItem = response.ToList().First();
        firstItem.Item.Should().NotBeNull();
        firstItem.Item.Id.Should().Be(new Guid("4b628214-e4fe-4fe0-b1ff-955df33e1515"));
        firstItem.Item.Name.Should().Be("Sample Article");
        
        // Variant should exist but without elements
        firstItem.Variant.Should().NotBeNull();
        firstItem.Variant.Language.Codename.Should().Be("en-US");
        firstItem.Variant.Elements.Should().BeNull();
    }

    [Fact]
    public async Task FilterVariantsAsync_WithIncludeContentTrue_ReturnsVariantWithElements()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("VariantFilterWithElements.json");

        var request = new VariantFilterRequestModel
        {
            Filters = new VariantFilterFiltersModel
            {
                Language = Reference.ByCodename("en-US"),
                ContentTypes = new List<Reference>
                {
                    Reference.ByCodename("article")
                }
            },
            IncludeContent = true
        };

        var response = await client.EarlyAccess.FilterVariantsAsync(request);

        response.Should().NotBeNull();
        response.Should().BeAssignableTo<IListingResponseModel<VariantFilterItemModel>>();
        
        var firstItem = response.ToList().First();
        firstItem.Item.Should().NotBeNull();
        firstItem.Item.Id.Should().Be(new Guid("6a8b4d04-7d3e-4d3c-8b9a-4c7e8f9a1b2c"));
        firstItem.Item.Name.Should().Be("Article with Content");
        firstItem.Item.ExternalId.Should().Be("ext_article_with_content");
        
        // Variant should exist with elements when include_content=true
        firstItem.Variant.Should().NotBeNull();
        firstItem.Variant.Language.Codename.Should().Be("en-US");
        firstItem.Variant.Elements.Should().NotBeNull();
        firstItem.Variant.Elements.Should().HaveCount(2);
        firstItem.Variant.Workflow.Should().NotBeNull();
        firstItem.Variant.Workflow.Step.Should().NotBeNull();
        firstItem.Variant.Workflow.Step.Codename.Should().Be("published");
    }

    [Fact]
    public async Task FilterVariantsAsync_WithEnums_UsesCorrectValues()
    {
        var client = _fileSystemFixture.CreateMockClientWithResponse("VariantFilter.json");

        var request = new VariantFilterRequestModel
        {
            Filters = new VariantFilterFiltersModel
            {
                Language = Reference.ByCodename("en-US"),
                CompletionStatuses = new List<VariantFilterCompletionStatus> 
                { 
                    VariantFilterCompletionStatus.Ready,
                    VariantFilterCompletionStatus.Unfinished,
                    VariantFilterCompletionStatus.NotTranslated,
                    VariantFilterCompletionStatus.AllDone
                }
            },
            Order = new VariantFilterOrderModel
            {
                By = "name",
                Direction = VariantFilterOrderDirection.Ascending
            }
        };

        var response = await client.EarlyAccess.FilterVariantsAsync(request);

        // Verify that enums are properly serialized and the request succeeds
        response.Should().NotBeNull();
        response.Should().BeAssignableTo<IListingResponseModel<VariantFilterItemModel>>();
        
        var firstItem = response.ToList().First();
        firstItem.Item.Should().NotBeNull();
        firstItem.Item.Id.Should().NotBeEmpty();
        firstItem.Item.Name.Should().NotBeNullOrEmpty();
    }
}
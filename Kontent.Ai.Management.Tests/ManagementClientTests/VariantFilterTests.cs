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
                CompletionStatuses = new List<string> { "completed" }
            },
            Order = new VariantFilterOrderModel
            {
                By = "name",
                Direction = "asc"
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
                CompletionStatuses = new List<string> { "completed", "unfinished" },
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
                Direction = "desc"
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
}
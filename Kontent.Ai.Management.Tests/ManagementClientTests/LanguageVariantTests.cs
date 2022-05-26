﻿using FluentAssertions;
using Kontent.Ai.Management.Extensions;
using Kontent.Ai.Management.Models.LanguageVariants;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.StronglyTyped;
using Kontent.Ai.Management.Models.Workflow;
using Kontent.Ai.Management.Tests.Base;
using Kontent.Ai.Management.Tests.Data;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class LanguageVariantTests : IClassFixture<FileSystemFixture>
{
    private readonly Scenario _scenario;
    
    public LanguageVariantTests()
    {
        _scenario = new Scenario(folder: "LanguageVariant");
    }

    [Fact]
    public async Task ListLanguageVariantsByItemAsync_StronglyTyped_ListsVariants()
    {
        var client = _scenario
            .WithResponses("LanguageVariants.json")
            .CreateManagementClient();

        var identifier = Reference.ById(Guid.Parse("4b628214-e4fe-4fe0-b1ff-955df33e1515"));
        var response = await client.ListLanguageVariantsByItemAsync<ComplexTestModel>(identifier);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Get)
            .Response(response, GetExpectedComplexTestModels("00000000-0000-0000-0000-000000000000", "10000000-0000-0000-0000-000000000000"))
            .Url($"{Endpoint}/projects/{PROJECT_ID}/items/{identifier.Id}/variants")
            .Validate();
    }
    
    [Fact]
    public async Task ListLanguageVariantsByItemAsync_StronglyTyped_IdentifierIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.ListLanguageVariantsByItemAsync<ComplexTestModel>(null))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    
    [Fact]
    public async Task ListLanguageVariantsByItemAsync_DynamicallyTyped_ListsVariants()
    {
        var client = _scenario
            .WithResponses("LanguageVariants.json")
            .CreateManagementClient();

        var identifier = Reference.ById(Guid.Parse("4b628214-e4fe-4fe0-b1ff-955df33e1515"));
        var response = await client.ListLanguageVariantsByItemAsync(identifier);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Get)
            .Response(response, GetExpectedLanguageVariantModels("00000000-0000-0000-0000-000000000000", "10000000-0000-0000-0000-000000000000"))
            .Url($"{Endpoint}/projects/{PROJECT_ID}/items/{identifier.Id}/variants")
            .Validate();
    }

    
    [Fact]
    public async Task ListLanguageVariantsByItemAsync_DynamicallyTyped_IdentifierIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.ListLanguageVariantsByItemAsync(null))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async void ListLanguageVariantsByTypeAsync_DynamicallyTyped_ListsVariants()
    {
        var client = _scenario
            .WithResponses("LanguageVariantsPage1.json", "LanguageVariantsPage2.json", "LanguageVariantsPage3.json")
            .CreateManagementClient();

        var expected = new[]
        {
            (itemId: "00000000-0000-0000-0000-000000000000", languageId: "00000000-0000-0000-0000-000000000000"),
            (itemId: "00000000-0000-0000-0000-000000000000", languageId: "10000000-0000-0000-0000-000000000000"),
            (itemId: "10000000-0000-0000-0000-000000000000", languageId: "00000000-0000-0000-0000-000000000000"),
            (itemId: "10000000-0000-0000-0000-000000000000", languageId: "10000000-0000-0000-0000-000000000000"),
            (itemId: "20000000-0000-0000-0000-000000000000", languageId: "00000000-0000-0000-0000-000000000000"),
            (itemId: "20000000-0000-0000-0000-000000000000", languageId: "10000000-0000-0000-0000-000000000000")
        }.Select(x => GetExpectedLanguageVariantModel(x.languageId, x.itemId));

        var identifier = Reference.ById(Guid.Parse("17ff8a28-ebe6-5c9d-95ea-18fe1ff86d2d"));
        var response = await client.ListLanguageVariantsByTypeAsync(identifier).GetAllAsync();

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Get)
            .Response(response, expected)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/types/{identifier.Id}/variants")
            .Validate();
    }
    
    [Fact]
    public async Task ListLanguageVariantsByTypeAsync_DynamicallyTyped_IdentifierIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.ListLanguageVariantsByTypeAsync(null))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    
    [Fact]
    public async Task ListLanguageVariantsOfContentTypeWithComponentsAsync_DynamicallyTyped_ListsVariants()
    {
        var client = _scenario
            .WithResponses("LanguageVariantsPage1.json", "LanguageVariantsPage2.json", "LanguageVariantsPage3.json")
            .CreateManagementClient();

        var expected = new[]
        {
            (itemId: "00000000-0000-0000-0000-000000000000", languageId: "00000000-0000-0000-0000-000000000000"),
            (itemId: "00000000-0000-0000-0000-000000000000", languageId: "10000000-0000-0000-0000-000000000000"),
            (itemId: "10000000-0000-0000-0000-000000000000", languageId: "00000000-0000-0000-0000-000000000000"),
            (itemId: "10000000-0000-0000-0000-000000000000", languageId: "10000000-0000-0000-0000-000000000000"),
            (itemId: "20000000-0000-0000-0000-000000000000", languageId: "00000000-0000-0000-0000-000000000000"),
            (itemId: "20000000-0000-0000-0000-000000000000", languageId: "10000000-0000-0000-0000-000000000000")
        }.Select(x => GetExpectedLanguageVariantModel(x.languageId, x.itemId));

        var identifier = Reference.ById(Guid.Parse("17ff8a28-ebe6-5c9d-95ea-18fe1ff86d2d"));
        var response = await client.ListLanguageVariantsOfContentTypeWithComponentsAsync(identifier).GetAllAsync();

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Get)
            .Response(response, expected)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/types/{identifier.Id}/components")
            .Validate();
    }
    
    [Fact]
    public async Task ListLanguageVariantsOfContentTypeWithComponentsAsync_DynamicallyTyped_IdentifierIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.ListLanguageVariantsOfContentTypeWithComponentsAsync(null))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }
    
    [Fact]
    public async void ListLanguageVariantsByCollectionAsync_DynamicallyTyped_ListsVariants()
    {
        var client = _scenario
            .WithResponses("LanguageVariantsPage1.json", "LanguageVariantsPage2.json", "LanguageVariantsPage3.json")
            .CreateManagementClient();

        var expected = new[]
        {
            (itemId: "00000000-0000-0000-0000-000000000000", languageId: "00000000-0000-0000-0000-000000000000"),
            (itemId: "00000000-0000-0000-0000-000000000000", languageId: "10000000-0000-0000-0000-000000000000"),
            (itemId: "10000000-0000-0000-0000-000000000000", languageId: "00000000-0000-0000-0000-000000000000"),
            (itemId: "10000000-0000-0000-0000-000000000000", languageId: "10000000-0000-0000-0000-000000000000"),
            (itemId: "20000000-0000-0000-0000-000000000000", languageId: "00000000-0000-0000-0000-000000000000"),
            (itemId: "20000000-0000-0000-0000-000000000000", languageId: "10000000-0000-0000-0000-000000000000")
        }.Select(x => GetExpectedLanguageVariantModel(x.languageId, x.itemId));

        var identifier = Reference.ById(Guid.Parse("17ff8a28-ebe6-5c9d-95ea-18fe1ff86d2d"));
        var response = await client.ListLanguageVariantsByCollectionAsync(identifier).GetAllAsync();

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Get)
            .Response(response, expected)
            .Url($"{Endpoint}/projects/{PROJECT_ID}/collections/{identifier.Id}/variants")
            .Validate();
    }
    
    [Fact]
    public async Task ListLanguageVariantsByCollectionAsync_DynamicallyTyped_IdentifierIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.ListLanguageVariantsByCollectionAsync(null))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Theory]
    [ClassData(typeof(CombinationOfIdentifiersAndUrl))]
    public async Task GetLanguageVariantAsync_StronglyTyped_GetsVariant(LanguageVariantIdentifier identifier, string expectedUrl)
    {
        var client = _scenario
            .WithResponses("LanguageVariant.json")
            .CreateManagementClient();

        var response = await client.GetLanguageVariantAsync<ComplexTestModel>(identifier);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Get)
            .Response(response, GetExpectedComplexTestModel())
            .Url(expectedUrl)
            .Validate();
    }
    
    [Fact]
    public async Task GetLanguageVariantAsync_StronglyTyped_IdentifierIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.GetLanguageVariantAsync<ComplexTestModel>(null))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Theory]
    [ClassData(typeof(CombinationOfIdentifiersAndUrl))]
    public async Task GetLanguageVariantAsync_DynamicallyTyped_GetsVariant(LanguageVariantIdentifier identifier, string expectedUrl)
    {
        var client = _scenario
            .WithResponses("LanguageVariant.json")
            .CreateManagementClient();

        var response = await client.GetLanguageVariantAsync(identifier);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Get)
            .Response(response, GetExpectedLanguageVariantModel())
            .Url(expectedUrl)
            .Validate();
    }

    [Fact]
    public async Task GetLanguageVariantAsync_DynamicallyTyped_IdentifierIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.GetLanguageVariantAsync(null))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Theory]
    [ClassData(typeof(CombinationOfIdentifiersAndUrl))]
    public async Task UpsertLanguageVariantAsync_StronglyTyped_UpsertsVariant(LanguageVariantIdentifier identifier, string expectedUrl)
    {
        var client = _scenario
            .WithResponses("LanguageVariant.json")
            .CreateManagementClient();

        var response = await client.UpsertLanguageVariantAsync(identifier, GetExpectedComplexTestModel().Elements);

        var expectedRequest = SerializeAndDeserialize(new LanguageVariantUpsertModel { Elements = GetExpectedLanguageVariantModel().Elements });

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Put)
            .RequestPayload(expectedRequest)
            .Response(response, GetExpectedComplexTestModel())
            .Url(expectedUrl)
            .Validate();
    }
   
    [Fact]
    public async Task UpsertLanguageVariantAsync_StronglyTyped_IdentifierIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.UpsertLanguageVariantAsync(null, new ComplexTestModel()))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Theory]
    [ClassData(typeof(CombinationOfIdentifiers))]
    public async Task UpsertLanguageVariantAsync_StronglyTyped_LanguageVariantUpsertModelIsNull_Throws(LanguageVariantIdentifier identifier)
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.UpsertLanguageVariantAsync(identifier, (ComplexTestModel)null))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }


    [Theory]
    [ClassData(typeof(CombinationOfIdentifiersAndUrl))]
    public async Task UpsertLanguageVariantAsync_DynamicallyTyped_ByLanguageVariantUpsertModel_UpsertsVariant(LanguageVariantIdentifier identifier, string expectedUrl)
    {
        var client = _scenario
            .WithResponses("LanguageVariant.json")
            .CreateManagementClient();

        var expected = GetExpectedLanguageVariantModel();

        var upsertModel = new LanguageVariantUpsertModel { Elements = expected.Elements };

        var response = await client.UpsertLanguageVariantAsync(identifier, upsertModel);

        var expectedRequest = SerializeAndDeserialize(upsertModel);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Put)
            .RequestPayload(expectedRequest)
            .Response(response, expected)
            .Url(expectedUrl)
            .Validate();
    }

    
    [Fact]
    public async Task UpsertLanguageVariantAsync_DynamicallyTyped_IdentifierIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.UpsertLanguageVariantAsync(null, new LanguageVariantUpsertModel()))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Theory]
    [ClassData(typeof(CombinationOfIdentifiers))]
    public async Task UpsertLanguageVariantAsync_DynamicallyTyped_LanguageVariantUpsertModelIsNull_Throws(LanguageVariantIdentifier identifier)
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.UpsertLanguageVariantAsync(identifier, (LanguageVariantUpsertModel)null))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Theory]
    [ClassData(typeof(CombinationOfIdentifiersAndUrl))]
    public async Task UpsertLanguageVariantAsync_DynamicallyTyped_ByLanguageVariantModel_UpsertsVariant(LanguageVariantIdentifier identifier, string expectedUrl)
    {
        var client = _scenario
            .WithResponses("LanguageVariant.json")
            .CreateManagementClient();

        var expected = GetExpectedLanguageVariantModel();

        var response = await client.UpsertLanguageVariantAsync(identifier, expected);

        var expectedRequest = SerializeAndDeserialize(new LanguageVariantUpsertModel(expected));

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Put)
            .RequestPayload(expectedRequest)
            .Response(response, expected)
            .Url(expectedUrl)
            .Validate();
    }

    
    [Fact]
    public async Task UpsertLanguageVariantAsync_DynamicallyTyped_ByLanguageVariantModel_IdentifierIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.UpsertLanguageVariantAsync(null, new LanguageVariantModel()))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Theory]
    [ClassData(typeof(CombinationOfIdentifiers))]
    public async Task UpsertLanguageVariantAsync_DynamicallyTyped_ByLanguageVariantModel_LanguageVariantModelIsNull_Throws(LanguageVariantIdentifier identifier)
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.UpsertLanguageVariantAsync(identifier, (LanguageVariantModel)null))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Theory]
    [ClassData(typeof(CombinationOfIdentifiersAndUrl))]
    public async Task DeleteLanguageVariantAsync_DeletesVariant(LanguageVariantIdentifier identifier, string expectedUrl)
    {
        var client = _scenario.CreateManagementClient();

        await client.DeleteLanguageVariantAsync(identifier);

        _scenario
            .CreateExpectations()
            .Url(expectedUrl)
            .HttpMethod(HttpMethod.Delete)
            .Validate();
    }
    
    private static List<LanguageVariantModel> GetExpectedLanguageVariantModels(params string[] languageIds)
    => languageIds.Select(x => GetExpectedLanguageVariantModel(x)).ToList();

    private static LanguageVariantModel GetExpectedLanguageVariantModel(
        string languageId = "78dbefe8-831b-457e-9352-f4c4eacd5024",
        string itemId = "4b628214-e4fe-4fe0-b1ff-955df33e1515") => new()
        {
            Item = Reference.ById(Guid.Parse(itemId)),
            Language = Reference.ById(Guid.Parse(languageId)),
            LastModified = DateTimeOffset.Parse("2021-11-06T13:57:26.7069564Z").UtcDateTime,
            Workflow = new WorkflowStepIdentifier(Reference.ById(Guid.Parse("00000000-0000-0000-0000-000000000000")), Reference.ById(Guid.Parse("eee6db3b-545a-4785-8e86-e3772c8756f9"))),
            Elements = ElementsData.GetExpectedDynamicElements(),
        };

    private static List<LanguageVariantModel<ComplexTestModel>> GetExpectedComplexTestModels(params string[] languageIds) 
        => languageIds.Select(GetExpectedComplexTestModel).ToList();

    private static LanguageVariantModel<ComplexTestModel> GetExpectedComplexTestModel(string languageId = "78dbefe8-831b-457e-9352-f4c4eacd5024") => new()
    {
        Item = Reference.ById(Guid.Parse("4b628214-e4fe-4fe0-b1ff-955df33e1515")),
        Language = Reference.ById(Guid.Parse(languageId)),
        LastModified = DateTimeOffset.Parse("2021-11-06T13:57:26.7069564Z").UtcDateTime,
        Workflow = new WorkflowStepIdentifier(Reference.ById(Guid.Parse("00000000-0000-0000-0000-000000000000")), Reference.ById(Guid.Parse("eee6db3b-545a-4785-8e86-e3772c8756f9"))),
        Elements = ElementsData.GetExpectedStronglyTypedElementsModel(),
    };

    private T SerializeAndDeserialize<T>(T @object)
    {
        var serialized = JsonConvert.SerializeObject(@object);
        return JsonConvert.DeserializeObject<T>(serialized);
    }

    private class CombinationOfIdentifiersAndUrl : IEnumerable<object[]>
    {

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<object[]> GetEnumerator()
        {
            foreach(var (Identifier, Url) in GetPermutation())
            {
                yield return new object[] { Identifier, Url };
            }
        }

        public IEnumerable<(LanguageVariantIdentifier Identifier, string Url)> GetPermutation()
        {
            var itemsIdentifiers = new[] { ById, ByCodename, ByExternalId };
            var languageIdentifiers = new[] { ById, ByCodename };

            foreach (var item in itemsIdentifiers)
            {
                foreach (var language in languageIdentifiers)
                {
                    var identifier = new LanguageVariantIdentifier(item.Identifier, language.Identifier);
                    var url = $"{Endpoint}/projects/{PROJECT_ID}/items/{item.UrlSegment}/variants/{language.UrlSegment}";
                    yield return (identifier, url);
                }
            }
        }

        protected (Reference Identifier, string UrlSegment) ById => (Reference.ById(Guid.Parse("4b628214-e4fe-4fe0-b1ff-955df33e1515")), "4b628214-e4fe-4fe0-b1ff-955df33e1515");
        protected (Reference Identifier, string UrlSegment) ByCodename => (Reference.ByCodename("codename"), "codename/codename");
        protected (Reference Identifier, string UrlSegment) ByExternalId => (Reference.ByExternalId("external-id"), "external-id/external-id");
    }

    private class CombinationOfIdentifiers : CombinationOfIdentifiersAndUrl, IEnumerable<object[]>
    {
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public new IEnumerator<object[]> GetEnumerator()
        {
            foreach (var (Identifier, Url) in GetPermutation())
            {
                yield return new object[] { Identifier };
            }
        }

    }
}

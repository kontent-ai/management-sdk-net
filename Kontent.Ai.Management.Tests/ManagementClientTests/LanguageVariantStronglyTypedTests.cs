using FluentAssertions;
using Kontent.Ai.Management.Models.LanguageVariants;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Tests.Base;
using MyProject.Models;
using Xunit;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

// Strongly-typed (generated-record) language-variant coverage. Scope here is the result-pattern + arg-guard
// behaviour reachable without HTTP. The success round-trip and HTTP-failure→result cases need the
// converter-injecting test scaffolding and land with the commit-5 MockHttp+Verify infra (this file extends
// there). The identifier→URL matrix stays covered by the non-generic *_DynamicallyTyped_* theories in
// LanguageVariantTests, which exercise the same ToUrlSegment + Refit endpoint the generics call.
public class LanguageVariantStronglyTypedTests
{
    private readonly Scenario _scenario = new(folder: "LanguageVariant");

    private static LanguageVariantIdentifier Identifier() =>
        new(Reference.ByCodename("my_article"), Reference.ByCodename("en-US"));

    [Fact]
    public async Task GetLanguageVariantAsync_StronglyTyped_IdentifierIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.GetLanguageVariantAsync<Article>(null!))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task UpsertLanguageVariantAsync_StronglyTyped_IdentifierIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.UpsertLanguageVariantAsync(null!, new Article()))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task UpsertLanguageVariantAsync_StronglyTyped_VariantIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.UpsertLanguageVariantAsync<Article>(Identifier(), null!))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task UpsertLanguageVariantAsync_StronglyTyped_ValidationFailure_ShortCircuitsWithoutHttp()
    {
        // [ExactElements(1)] on Article.Author — an empty collection violates it, so the validator fails the
        // precheck and the method returns before any HTTP call (phase-4 decision §9): no status, element-scoped error.
        var client = _scenario.CreateManagementClient();

        var result = await client.UpsertLanguageVariantAsync(Identifier(), new Article { Author = [] });

        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().BeNull();
        result.Errors.Should().Contain(e => e.ElementCodename == "author");
    }
}

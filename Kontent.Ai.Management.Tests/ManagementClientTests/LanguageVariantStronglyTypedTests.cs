using FluentAssertions;
using Kontent.Ai.Management.Conversion;
using Kontent.Ai.Management.Models.LanguageVariants;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Tests.Base;
using MyProject.Models;
using System.Net;
using Xunit;
// Models.Content carries its own `Reference`; alias the two types we need so it doesn't collide with
// Models.Shared.Reference used for the identifier.
using RichTextBuilder = Kontent.Ai.Management.Models.Content.RichTextBuilder;
using RichTextElement = Kontent.Ai.Management.Models.Content.RichTextElement;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

// Strongly-typed (generated-record) language-variant coverage on the existing Scenario infra: the result-pattern
// contract (success projection, validation short-circuit, HTTP-failure→result) plus arg guards. The identifier→URL
// matrix stays covered by the non-generic *_DynamicallyTyped_* theories in LanguageVariantTests, which exercise the
// same ToUrlSegment + Refit endpoint the generics call — deliberately not re-matrixed here.
public class LanguageVariantStronglyTypedTests
{
    private readonly Scenario _scenario = new(folder: "LanguageVariant");

    private static LanguageVariantIdentifier Identifier() =>
        new(Reference.ByCodename("my_article"), Reference.ByCodename("en-US"));

    // The client's owned converter auto-scans typeof(T).Assembly; in the test assembly that trips the deliberate
    // GeneratedStubs/MyProject codename collision. Inject a converter with just the types under test registered.
    private static ContentItemEnvelopeConverter Converter()
    {
        var registry = new ContentTypeRegistry();
        registry.Register(typeof(Callout));
        return new ContentItemEnvelopeConverter(registry);
    }

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

    [Fact]
    public async Task GetLanguageVariantAsync_StronglyTyped_Success_ProjectsValueAndStatus()
    {
        var client = _scenario
            .WithResponses("StronglyTypedCalloutVariant.json")
            .CreateManagementClient(Converter());

        var result = await client.GetLanguageVariantAsync<Callout>(Identifier());

        result.IsSuccess.Should().BeTrue();
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Errors.Should().BeEmpty();

        var callout = result.Value;
        callout.Type.Should().Equal(CalloutType.Warning);
        callout.Content!.Value.Should().StartWith("<p>outer</p>");

        // Rich-text component recursed through the client: bridge → converter → nested record.
        var nested = callout.Content.Components.Should().ContainSingle()
            .Which.Content.Should().BeOfType<Callout>().Subject;
        nested.Content!.Value.Should().Be("<p>inner</p>");
        nested.Type.Should().Equal(CalloutType.Info);
    }

    [Fact]
    public async Task UpsertLanguageVariantAsync_StronglyTyped_Success_SendsAndProjects()
    {
        var client = _scenario
            .WithResponses("StronglyTypedCalloutVariant.json")
            .CreateManagementClient(Converter());

        // RichTextBuilder → validator precheck (passes) → write-bridge → PUT → response read-bridge → projection.
        var rt = new RichTextBuilder();
        var callout = new Callout
        {
            Type = [CalloutType.Warning],
            Content = rt.Build($"<p>body</p>{rt.Component(new Callout { Type = [CalloutType.Info], Content = new RichTextElement { Value = "<p>inner</p>" } })}"),
        };

        var result = await client.UpsertLanguageVariantAsync(Identifier(), callout);

        result.IsSuccess.Should().BeTrue();
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Value.Type.Should().Equal(CalloutType.Warning);
    }

    [Fact]
    public async Task GetLanguageVariantAsync_StronglyTyped_HttpFailure_ReturnsFailureWithStatusAndErrors()
    {
        const string body = """
            { "message": "The requested content item was not found.",
              "validation_errors": [ { "message": "Item 'x' does not exist." } ] }
            """;
        var client = _scenario
            .WithResponse(HttpStatusCode.NotFound, body)
            .CreateManagementClient();

        var result = await client.GetLanguageVariantAsync<Callout>(Identifier());

        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        result.Errors.Should().Contain(e => e.Message.Contains("not found"))
            .And.Contain(e => e.Message.Contains("does not exist"));
    }
}

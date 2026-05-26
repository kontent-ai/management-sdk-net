using AwesomeAssertions;
using Kontent.Ai.Management.Conversion;
using Kontent.Ai.Management.Models.LanguageVariants;
using Kontent.Ai.Management.Tests.Base;
using MyProject.Models;
using RichardSzalay.MockHttp;
using System.Net;
// Models.Content carries its own `Reference`; alias the two types we need so it doesn't collide with
// Models.Shared.Reference used for the identifier.
using RichTextBuilder = Kontent.Ai.Management.Models.Content.RichTextBuilder;
using RichTextElement = Kontent.Ai.Management.Models.Content.RichTextElement;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

// Strongly-typed (generated-record) language-variant coverage: the result-pattern contract (success projection,
// validation short-circuit, HTTP-failure→result) plus arg guards. The identifier→URL matrix stays covered by the
// non-generic *_DynamicallyTyped_* theories in LanguageVariantTests, which exercise the same ToUrlSegment + Refit
// endpoint the generics call — deliberately not re-matrixed here.
public class LanguageVariantStronglyTypedTests
{
    private static string Fixture(string name)
        => File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Data", "LanguageVariant", name));

    private static LanguageVariantIdentifier Identifier() =>
        new(Reference.ByCodename("my_article"), Reference.ByCodename("en-US"));

    private static string VariantUrl =>
        $"{MockClientFactory.BaseUrl}/items/codename/my_article/variants/codename/en-US";

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
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.GetLanguageVariantAsync<Article>(null!))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task UpsertLanguageVariantAsync_StronglyTyped_IdentifierIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.UpsertLanguageVariantAsync(null!, new Article()))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task UpsertLanguageVariantAsync_StronglyTyped_VariantIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.UpsertLanguageVariantAsync<Article>(Identifier(), null!))
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task UpsertLanguageVariantAsync_StronglyTyped_ValidationFailure_ShortCircuitsWithoutHttp()
    {
        // [ExactElements(1)] on Article.Author — an empty collection violates it, so the validator fails the
        // precheck and the method returns before any HTTP call: no status, element-scoped error. No mock.Expect is
        // registered: MockHttp throws on any unmatched request, so an erroneous HTTP call would fail this test.
        var (client, _) = MockClientFactory.Create();

        var result = await client.UpsertLanguageVariantAsync(Identifier(), new Article { Author = [] });

        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().BeNull();
        result.ValidationErrors().Should().Contain(e => e.Path == "author");
    }

    [Fact]
    public async Task GetLanguageVariantAsync_StronglyTyped_Success_ProjectsValueAndStatus()
    {
        var (client, mock) = MockClientFactory.Create(Converter());
        mock.Expect(HttpMethod.Get, VariantUrl)
            .Respond("application/json", Fixture("StronglyTypedCalloutVariant.json"));

        var result = await client.GetLanguageVariantAsync<Callout>(Identifier());

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Error.Should().BeNull();

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
        var (client, mock) = MockClientFactory.Create(Converter());
        mock.Expect(HttpMethod.Put, VariantUrl)
            .Respond("application/json", Fixture("StronglyTypedCalloutVariant.json"));

        // RichTextBuilder → validator precheck (passes) → write-bridge → PUT → response read-bridge → projection.
        var rt = new RichTextBuilder();
        var callout = new Callout
        {
            Type = [CalloutType.Warning],
            Content = rt.Build($"<p>body</p>{rt.Component(new Callout { Type = [CalloutType.Info], Content = new RichTextElement { Value = "<p>inner</p>" } })}"),
        };

        var result = await client.UpsertLanguageVariantAsync(Identifier(), callout);

        mock.VerifyNoOutstandingExpectation();
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
        var (client, mock) = MockClientFactory.Create();
        mock.Expect(HttpMethod.Get, VariantUrl)
            .Respond(HttpStatusCode.NotFound, "application/json", body);

        var result = await client.GetLanguageVariantAsync<Callout>(Identifier());

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        result.Error!.Message.Should().Contain("not found");
        result.Error.ValidationErrors.Should().Contain(e => e.Message.Contains("does not exist"));
    }
}

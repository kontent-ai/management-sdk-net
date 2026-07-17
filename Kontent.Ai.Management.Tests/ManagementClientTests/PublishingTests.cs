using AwesomeAssertions;
using Kontent.Ai.Management.Models.LanguageVariants;
using Kontent.Ai.Management.Models.Publishing;
using Kontent.Ai.Management.Tests.Base;
using RichardSzalay.MockHttp;
using System.Collections;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class PublishingTests
{
    [Theory]
    [ClassData(typeof(CombinationOfVariantIdentifiersAndUrl))]
    public async Task ChangeLanguageVariantWorkflowAsync_ChangesWorkflow(LanguageVariantIdentifier variantIdentifier, string expectedUrl)
    {
        var (client, mock) = MockClientFactory.Create();
        var model = new ChangeLanguageVariantWorkflowModel
            (
                workflow: Reference.ById(Guid.NewGuid()),
                step: Reference.ById(Guid.NewGuid())
            );

        mock.Expect(HttpMethod.Put, $"{expectedUrl}/change-workflow")
            .CaptureBody(out var capturedBody)
            .Respond(System.Net.HttpStatusCode.OK);

        var result = await client.ChangeLanguageVariantWorkflowAsync(variantIdentifier, model);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        capturedBody.ShouldMatchSerialized(model);
    }

    [Fact]
    public async Task ChangeLanguageVariantWorkflowAsync_NoIdentifier_Throws()
    {
        var (client, _) = MockClientFactory.Create();
        var model = new ChangeLanguageVariantWorkflowModel
            (
                workflow: Reference.ById(Guid.NewGuid()),
                step: Reference.ById(Guid.NewGuid())
            );

        await client.Invoking(x => x.ChangeLanguageVariantWorkflowAsync(null!, model)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task ChangeLanguageVariantWorkflowAsync_PayloadIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();
        var identifier = new LanguageVariantIdentifier
            (
                itemIdentifier: Reference.ById(Guid.NewGuid()),
                languageIdentifier: Reference.ById(Guid.NewGuid())
            );

        await client.Invoking(x => x.ChangeLanguageVariantWorkflowAsync(identifier, null!)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Theory]
    [ClassData(typeof(CombinationOfVariantIdentifiersAndUrl))]
    public async Task PublishLanguageVariantAsync_PublishesVariant(LanguageVariantIdentifier variantIdentifier, string expectedUrl)
    {
        var (client, mock) = MockClientFactory.Create();
        mock.Expect(HttpMethod.Put, $"{expectedUrl}/publish")
            .Respond(System.Net.HttpStatusCode.OK);

        var result = await client.PublishLanguageVariantAsync(variantIdentifier);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task PublishLanguageVariantAsync_NoIdentifier_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.PublishLanguageVariantAsync(null!)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Theory]
    [ClassData(typeof(CombinationOfVariantIdentifiersAndUrl))]
    public async Task SchedulePublishingOfLanguageVariantAsync_SchedulesPublishingVariant(LanguageVariantIdentifier variantIdentifier, string expectedUrl)
    {
        var (client, mock) = MockClientFactory.Create();
        var schedule = new ScheduleModel
        {
            DisplayTimeZone = "prague",
            ScheduleTo = DateTimeOffset.UtcNow
        };

        mock.Expect(HttpMethod.Put, $"{expectedUrl}/publish")
            .CaptureBody(out var capturedBody)
            .Respond(System.Net.HttpStatusCode.OK);

        var result = await client.SchedulePublishingOfLanguageVariantAsync(variantIdentifier, schedule);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        capturedBody.ShouldMatchSerialized(schedule);
    }

    [Fact]
    public async Task SchedulePublishingOfLanguageVariantAsync_NoIdentifier_Throws()
    {
        var (client, _) = MockClientFactory.Create();
        var schedule = new ScheduleModel
        {
            DisplayTimeZone = "prague",
            ScheduleTo = DateTimeOffset.UtcNow
        };

        await client.Invoking(x => x.SchedulePublishingOfLanguageVariantAsync(null!, schedule)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Theory]
    [ClassData(typeof(CombinationOfVariantIdentifiersAndUrl))]
    public async Task SchedulePublishingAndUnpublishingOfLanguageVariantAsync_SchedulesVariant(LanguageVariantIdentifier variantIdentifier, string expectedUrl)
    {
        var (client, mock) = MockClientFactory.Create();
        var schedule = new SchedulePublishAndUnpublishModel()
        {
            PublishDisplayTimeZone = "prague",
            PublishScheduledTo = DateTimeOffset.UtcNow,
            UnpublishDisplayTimeZone = "prague",
            UnpublishScheduledTo = DateTimeOffset.UtcNow.AddDays(10)
        };

        mock.Expect(HttpMethod.Put, $"{expectedUrl}/schedule-publish-and-unpublish")
            .CaptureBody(out var capturedBody)
            .Respond(System.Net.HttpStatusCode.OK);

        var result = await client.SchedulePublishingAndUnpublishingOfLanguageVariantAsync(variantIdentifier, schedule);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        capturedBody.ShouldMatchSerialized(schedule);
    }

    [Fact]
    public async Task SchedulePublishingAndUnpublishingOfLanguageVariantAsync_NoIdentifier_Throws()
    {
        var (client, _) = MockClientFactory.Create();
        var schedule = new SchedulePublishAndUnpublishModel()
        {
            PublishDisplayTimeZone = "prague",
            PublishScheduledTo = DateTimeOffset.UtcNow,
            UnpublishDisplayTimeZone = "prague",
            UnpublishScheduledTo = DateTimeOffset.UtcNow.AddDays(10)
        };

        await client.Invoking(x => x.SchedulePublishingAndUnpublishingOfLanguageVariantAsync(null!, schedule)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task SchedulePublishingOfLanguageVariantAsync_ScheduleModelIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();
        var identifier = new LanguageVariantIdentifier
            (
                itemIdentifier: Reference.ById(Guid.NewGuid()),
                languageIdentifier: Reference.ById(Guid.NewGuid())
            );

        await client.Invoking(x => x.SchedulePublishingOfLanguageVariantAsync(identifier, null!)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Theory]
    [ClassData(typeof(CombinationOfVariantIdentifiersAndUrl))]
    public async Task CancelPublishingOfLanguageVariantAsync_CancelsScheduledPublish(LanguageVariantIdentifier variantIdentifier, string expectedUrl)
    {
        var (client, mock) = MockClientFactory.Create();
        mock.Expect(HttpMethod.Put, $"{expectedUrl}/cancel-scheduled-publish")
            .Respond(System.Net.HttpStatusCode.OK);

        var result = await client.CancelPublishingOfLanguageVariantAsync(variantIdentifier);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task CancelPublishingOfLanguageVariantAsync_NoIdentifier_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.CancelPublishingOfLanguageVariantAsync(null!)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Theory]
    [ClassData(typeof(CombinationOfVariantIdentifiersAndUrl))]
    public async Task UnpublishLanguageVariantAsync_UnpublishesAndArchivesVariant(LanguageVariantIdentifier variantIdentifier, string expectedUrl)
    {
        var (client, mock) = MockClientFactory.Create();
        mock.Expect(HttpMethod.Put, $"{expectedUrl}/unpublish-and-archive")
            .Respond(System.Net.HttpStatusCode.OK);

        var result = await client.UnpublishLanguageVariantAsync(variantIdentifier);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task UnpublishLanguageVariantAsync_NoIdentifier_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.UnpublishLanguageVariantAsync(null!)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Theory]
    [ClassData(typeof(CombinationOfVariantIdentifiersAndUrl))]
    public async Task CancelUnpublishingOfLanguageVariantAsync_CancelsScheduledUnpublish(LanguageVariantIdentifier variantIdentifier, string expectedUrl)
    {
        var (client, mock) = MockClientFactory.Create();
        mock.Expect(HttpMethod.Put, $"{expectedUrl}/cancel-scheduled-unpublish")
            .Respond(System.Net.HttpStatusCode.OK);

        var result = await client.CancelUnpublishingOfLanguageVariantAsync(variantIdentifier);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task CancelUnpublishingOfLanguageVariantAsync_NoIdentifier_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.CancelUnpublishingOfLanguageVariantAsync(null!)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Theory]
    [ClassData(typeof(CombinationOfVariantIdentifiersAndUrl))]
    public async Task ScheduleUnpublishingOfLanguageVariantAsync_SchedulesUnpublishingOfVariant(LanguageVariantIdentifier variantIdentifier, string expectedUrl)
    {
        var (client, mock) = MockClientFactory.Create();
        var schedule = new ScheduleModel
        {
            DisplayTimeZone = "prague",
            ScheduleTo = DateTimeOffset.UtcNow
        };

        mock.Expect(HttpMethod.Put, $"{expectedUrl}/unpublish-and-archive")
            .CaptureBody(out var capturedBody)
            .Respond(System.Net.HttpStatusCode.OK);

        var result = await client.ScheduleUnpublishingOfLanguageVariantAsync(variantIdentifier, schedule);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        capturedBody.ShouldMatchSerialized(schedule);
    }

    [Fact]
    public async Task ScheduleUnpublishingOfLanguageVariantAsync_NoIdentifier_Throws()
    {
        var (client, _) = MockClientFactory.Create();
        var schedule = new ScheduleModel
        {
            DisplayTimeZone = "prague",
            ScheduleTo = DateTimeOffset.UtcNow
        };

        await client.Invoking(x => x.ScheduleUnpublishingOfLanguageVariantAsync(null!, schedule)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task ScheduleUnpublishingOfLanguageVariantAsync_ScheduleModelIsNull_Throws()
    {
        var (client, _) = MockClientFactory.Create();
        var identifier = new LanguageVariantIdentifier
            (
                itemIdentifier: Reference.ById(Guid.NewGuid()),
                languageIdentifier: Reference.ById(Guid.NewGuid())
            );

        await client.Invoking(x => x.ScheduleUnpublishingOfLanguageVariantAsync(identifier, null!)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Theory]
    [ClassData(typeof(CombinationOfVariantIdentifiersAndUrl))]
    public async Task CreateNewVersionOfLanguageVariantAsync_CreatesNewVersion(LanguageVariantIdentifier variantIdentifier, string expectedUrl)
    {
        var (client, mock) = MockClientFactory.Create();
        mock.Expect(HttpMethod.Put, $"{expectedUrl}/new-version")
            .Respond(System.Net.HttpStatusCode.OK);

        var result = await client.CreateNewVersionOfLanguageVariantAsync(variantIdentifier);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task CreateNewVersionOfLanguageVariantAsync_NoIdentifier_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.CreateNewVersionOfLanguageVariantAsync(null!)).Should().ThrowAsync<ArgumentNullException>();
    }

    private class CombinationOfVariantIdentifiersAndUrl : IEnumerable<object[]>
    {
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<object[]> GetEnumerator()
        {
            foreach (var (Identifier, Url) in GetPermutation())
            {
                yield return new object[] { Identifier, Url };
            }
        }

        public static IEnumerable<(LanguageVariantIdentifier Identifier, string Url)> GetPermutation()
        {
            var items = new[] { IdentifierPermutations.ById, IdentifierPermutations.ByCodename, IdentifierPermutations.ByExternalId };
            var languages = new[] { IdentifierPermutations.ById, IdentifierPermutations.ByCodename };

            foreach (var (item, itemSegment, language, languageSegment) in IdentifierPermutations.Pairs(items, languages))
            {
                var identifier = new LanguageVariantIdentifier(item, language);
                var url = $"{MockClientFactory.BaseUrl}/items/{itemSegment}/variants/{languageSegment}";
                yield return (identifier, url);
            }
        }
    }
}

using System.Collections;
using FluentAssertions;
using Kontent.Ai.Management.Models.LanguageVariants;
using Kontent.Ai.Management.Models.Publishing;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Tests.Base;
using Newtonsoft.Json;
using RichardSzalay.MockHttp;
using Xunit;

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
                workflowIdentifier: Reference.ById(Guid.NewGuid()),
                stepIdentifier: Reference.ById(Guid.NewGuid())
            );

        string? capturedBody = null;
        mock.Expect(HttpMethod.Put, $"{expectedUrl}/change-workflow")
            .With(r =>
            {
                capturedBody = r.Content!.ReadAsStringAsync().GetAwaiter().GetResult();
                return true;
            })
            .Respond(System.Net.HttpStatusCode.OK);

        await client.ChangeLanguageVariantWorkflowAsync(variantIdentifier, model);

        mock.VerifyNoOutstandingExpectation();
        capturedBody.Should().NotBeNull();
        JsonConvert.DeserializeObject<ChangeLanguageVariantWorkflowModel>(capturedBody!)
            .Should().BeEquivalentTo(JsonConvert.DeserializeObject<ChangeLanguageVariantWorkflowModel>(JsonConvert.SerializeObject(model)));
    }

    [Fact]
    public async Task ChangeLanguageVariantWorkflowAsync_NoIdentifier_Throws()
    {
        var (client, _) = MockClientFactory.Create();
        var model = new ChangeLanguageVariantWorkflowModel
            (
                workflowIdentifier: Reference.ById(Guid.NewGuid()),
                stepIdentifier: Reference.ById(Guid.NewGuid())
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

        await client.PublishLanguageVariantAsync(variantIdentifier);

        mock.VerifyNoOutstandingExpectation();
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

        string? capturedBody = null;
        mock.Expect(HttpMethod.Put, $"{expectedUrl}/publish")
            .With(r =>
            {
                capturedBody = r.Content!.ReadAsStringAsync().GetAwaiter().GetResult();
                return true;
            })
            .Respond(System.Net.HttpStatusCode.OK);

        await client.SchedulePublishingOfLanguageVariantAsync(variantIdentifier, schedule);

        mock.VerifyNoOutstandingExpectation();
        capturedBody.Should().NotBeNull();
        JsonConvert.DeserializeObject<ScheduleModel>(capturedBody!)
            .Should().BeEquivalentTo(JsonConvert.DeserializeObject<ScheduleModel>(JsonConvert.SerializeObject(schedule)));
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

        string? capturedBody = null;
        mock.Expect(HttpMethod.Put, $"{expectedUrl}/schedule-publish-and-unpublish")
            .With(r =>
            {
                capturedBody = r.Content!.ReadAsStringAsync().GetAwaiter().GetResult();
                return true;
            })
            .Respond(System.Net.HttpStatusCode.OK);

        await client.SchedulePublishingAndUnpublishingOfLanguageVariantAsync(variantIdentifier, schedule);

        mock.VerifyNoOutstandingExpectation();
        capturedBody.Should().NotBeNull();
        JsonConvert.DeserializeObject<SchedulePublishAndUnpublishModel>(capturedBody!)
            .Should().BeEquivalentTo(JsonConvert.DeserializeObject<SchedulePublishAndUnpublishModel>(JsonConvert.SerializeObject(schedule)));
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
    public async Task CancelPublishingOfLanguageVariantAsync_SchedulesPublishingVariant(LanguageVariantIdentifier variantIdentifier, string expectedUrl)
    {
        var (client, mock) = MockClientFactory.Create();
        mock.Expect(HttpMethod.Put, $"{expectedUrl}/cancel-scheduled-publish")
            .Respond(System.Net.HttpStatusCode.OK);

        await client.CancelPublishingOfLanguageVariantAsync(variantIdentifier);

        mock.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task CancelPublishingOfLanguageVariantAsync_NoIdentifier_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.CancelPublishingOfLanguageVariantAsync(null!)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Theory]
    [ClassData(typeof(CombinationOfVariantIdentifiersAndUrl))]
    public async Task UnpublishLanguageVariantAsync_SchedulesPublishingVariant(LanguageVariantIdentifier variantIdentifier, string expectedUrl)
    {
        var (client, mock) = MockClientFactory.Create();
        mock.Expect(HttpMethod.Put, $"{expectedUrl}/unpublish-and-archive")
            .Respond(System.Net.HttpStatusCode.OK);

        await client.UnpublishLanguageVariantAsync(variantIdentifier);

        mock.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task UnpublishLanguageVariantAsync_NoIdentifier_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.UnpublishLanguageVariantAsync(null!)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Theory]
    [ClassData(typeof(CombinationOfVariantIdentifiersAndUrl))]
    public async Task CancelUnpublishingOfLanguageVariantAsync_SchedulesPublishingVariant(LanguageVariantIdentifier variantIdentifier, string expectedUrl)
    {
        var (client, mock) = MockClientFactory.Create();
        mock.Expect(HttpMethod.Put, $"{expectedUrl}/cancel-scheduled-unpublish")
            .Respond(System.Net.HttpStatusCode.OK);

        await client.CancelUnpublishingOfLanguageVariantAsync(variantIdentifier);

        mock.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task CancelUnpublishingOfLanguageVariantAsync_NoIdentifier_Throws()
    {
        var (client, _) = MockClientFactory.Create();

        await client.Invoking(x => x.CancelUnpublishingOfLanguageVariantAsync(null!)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Theory]
    [ClassData(typeof(CombinationOfVariantIdentifiersAndUrl))]
    public async Task ScheduleUnpublishingOfLanguageVariantAsync_SchedulesPublishingVariant(LanguageVariantIdentifier variantIdentifier, string expectedUrl)
    {
        var (client, mock) = MockClientFactory.Create();
        var schedule = new ScheduleModel
        {
            DisplayTimeZone = "prague",
            ScheduleTo = DateTimeOffset.UtcNow
        };

        string? capturedBody = null;
        mock.Expect(HttpMethod.Put, $"{expectedUrl}/unpublish-and-archive")
            .With(r =>
            {
                capturedBody = r.Content!.ReadAsStringAsync().GetAwaiter().GetResult();
                return true;
            })
            .Respond(System.Net.HttpStatusCode.OK);

        await client.ScheduleUnpublishingOfLanguageVariantAsync(variantIdentifier, schedule);

        mock.VerifyNoOutstandingExpectation();
        capturedBody.Should().NotBeNull();
        JsonConvert.DeserializeObject<ScheduleModel>(capturedBody!)
            .Should().BeEquivalentTo(JsonConvert.DeserializeObject<ScheduleModel>(JsonConvert.SerializeObject(schedule)));
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
    public async Task CreateNewVersionOfLanguageVariantAsync_SchedulesPublishingVariant(LanguageVariantIdentifier variantIdentifier, string expectedUrl)
    {
        var (client, mock) = MockClientFactory.Create();
        mock.Expect(HttpMethod.Put, $"{expectedUrl}/new-version")
            .Respond(System.Net.HttpStatusCode.OK);

        await client.CreateNewVersionOfLanguageVariantAsync(variantIdentifier);

        mock.VerifyNoOutstandingExpectation();
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
            var itemsIdentifiers = new[] { ById, ByCodename, ByExternalId };
            var languageIdentifiers = new[] { ById, ByCodename };

            foreach (var item in itemsIdentifiers)
            {
                foreach (var language in languageIdentifiers)
                {
                    var identifier = new LanguageVariantIdentifier(item.Identifier, language.Identifier);
                    var url = $"{MockClientFactory.BaseUrl}/items/{item.UrlSegment}/variants/{language.UrlSegment}";
                    yield return (identifier, url);
                }
            }
        }

        private static (Reference Identifier, string UrlSegment) ById => (Reference.ById(Guid.Parse("4b628214-e4fe-4fe0-b1ff-955df33e1515")), "4b628214-e4fe-4fe0-b1ff-955df33e1515");
        private static (Reference Identifier, string UrlSegment) ByCodename => (Reference.ByCodename("codename"), "codename/codename");
        private static (Reference Identifier, string UrlSegment) ByExternalId => (Reference.ByExternalId("external-id"), "external-id/external-id");
    }

    private class CombinationOfIdentifiers : CombinationOfVariantIdentifiersAndUrl, IEnumerable<object[]>
    {
        public new IEnumerator<object[]> GetEnumerator()
        {
            foreach (var (Identifier, Url) in GetPermutation())
            {
                yield return new object[] { Identifier };
            }
        }
    }
}

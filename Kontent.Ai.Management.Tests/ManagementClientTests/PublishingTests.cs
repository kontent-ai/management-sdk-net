﻿using FluentAssertions;
using Kontent.Ai.Management.Models.LanguageVariants;
using Kontent.Ai.Management.Models.Publishing;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Tests.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using Xunit;
using static Kontent.Ai.Management.Tests.Base.Scenario;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class PublishingTests
{
    private readonly Scenario _scenario;

    public PublishingTests()
    {
        _scenario = new Scenario(folder: "Workflow");
    }

    [Theory]
    [ClassData(typeof(CombinationOfVariantIdentifiersAndUrl))]
    public async void ChangeLanguageVariantWorkflowAsync_ChangesWorkflow(LanguageVariantIdentifier variantIdentifier, string expectedUrl)
    {
        var client = _scenario.CreateManagementClient();

        var model = new ChangeLanguageVariantWorkflowModel
            (
                workflowIdentifier: Reference.ById(Guid.NewGuid()),
                stepIdentifier: Reference.ById(Guid.NewGuid())
            );

        await client.ChangeLanguageVariantWorkflowAsync(variantIdentifier, model);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Put)
            .RequestPayload(model)
            .Url($"{expectedUrl}/change-workflow")
            .Validate();
    }

    [Fact]
    public async void ChangeLanguageVariantWorkflowAsync_NoIdentifier_Throws()
    {
        var client = _scenario.CreateManagementClient();

        var model = new ChangeLanguageVariantWorkflowModel
            (
                workflowIdentifier: Reference.ById(Guid.NewGuid()),
                stepIdentifier: Reference.ById(Guid.NewGuid())
            );

        await client.Invoking(x => x.ChangeLanguageVariantWorkflowAsync(null, model)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async void ChangeLanguageVariantWorkflowAsync_PayloadIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        var identifier = new LanguageVariantIdentifier
            (
                itemIdentifier: Reference.ById(Guid.NewGuid()),
                languageIdentifier: Reference.ById(Guid.NewGuid())
            );

        await client.Invoking(x => x.ChangeLanguageVariantWorkflowAsync(identifier, null)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Theory]
    [ClassData(typeof(CombinationOfVariantIdentifiersAndUrl))]
    public async void PublishLanguageVariantAsync_PublishesVariant(LanguageVariantIdentifier variantIdentifier, string expectedUrl)
    {
        var client = _scenario.CreateManagementClient();

        await client.PublishLanguageVariantAsync(variantIdentifier);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Put)
            .Url($"{expectedUrl}/publish")
            .Validate();
    }

    [Fact]
    public async void PublishLanguageVariantAsync_NoIdentifier_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.PublishLanguageVariantAsync(null)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Theory]
    [ClassData(typeof(CombinationOfVariantIdentifiersAndUrl))]
    public async void SchedulePublishingOfLanguageVariantAsync_SchedulesPublishingVariant(LanguageVariantIdentifier variantIdentifier, string expectedUrl)
    {
        var client = _scenario.CreateManagementClient();

        var schedule = new ScheduleModel
        {
            DisplayTimeZone = "prague",
            ScheduleTo = DateTimeOffset.UtcNow
        };

        await client.SchedulePublishingOfLanguageVariantAsync(variantIdentifier, schedule);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Put)
            .RequestPayload(schedule)
            .Url($"{expectedUrl}/publish")
            .Validate();
    }

    [Fact]
    public async void SchedulePublishingOfLanguageVariantAsync_NoIdentifier_Throws()
    {
        var client = _scenario.CreateManagementClient();

        var schedule = new ScheduleModel
        {
            DisplayTimeZone = "prague",
            ScheduleTo = DateTimeOffset.UtcNow
        };

        await client.Invoking(x => x.SchedulePublishingOfLanguageVariantAsync(null, schedule)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async void SchedulePublishingOfLanguageVariantAsync_ScheduleModelIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        var identifier = new LanguageVariantIdentifier
            (
                itemIdentifier: Reference.ById(Guid.NewGuid()),
                languageIdentifier: Reference.ById(Guid.NewGuid())
            );

        await client.Invoking(x => x.SchedulePublishingOfLanguageVariantAsync(identifier, null)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Theory]
    [ClassData(typeof(CombinationOfVariantIdentifiersAndUrl))]
    public async void CancelPublishingOfLanguageVariantAsync_SchedulesPublishingVariant(LanguageVariantIdentifier variantIdentifier, string expectedUrl)
    {
        var client = _scenario.CreateManagementClient();

        await client.CancelPublishingOfLanguageVariantAsync(variantIdentifier);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Put)
            .Url($"{expectedUrl}/cancel-scheduled-publish")
            .Validate();
    }

    [Fact]
    public async void CancelPublishingOfLanguageVariantAsync_NoIdentifier_Throws()
    {
        var client = _scenario.CreateManagementClient();


        await client.Invoking(x => x.CancelPublishingOfLanguageVariantAsync(null)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Theory]
    [ClassData(typeof(CombinationOfVariantIdentifiersAndUrl))]
    public async void UnpublishLanguageVariantAsync_SchedulesPublishingVariant(LanguageVariantIdentifier variantIdentifier, string expectedUrl)
    {
        var client = _scenario.CreateManagementClient();

        await client.UnpublishLanguageVariantAsync(variantIdentifier);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Put)
            .Url($"{expectedUrl}/unpublish-and-archive")
            .Validate();
    }

    [Fact]
    public async void UnpublishLanguageVariantAsync_NoIdentifier_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.UnpublishLanguageVariantAsync(null)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Theory]
    [ClassData(typeof(CombinationOfVariantIdentifiersAndUrl))]
    public async void CancelUnpublishingOfLanguageVariantAsync_SchedulesPublishingVariant(LanguageVariantIdentifier variantIdentifier, string expectedUrl)
    {
        var client = _scenario.CreateManagementClient();

        await client.CancelUnpublishingOfLanguageVariantAsync(variantIdentifier);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Put)
            .Url($"{expectedUrl}/cancel-scheduled-unpublish")
            .Validate();
    }

    [Fact]
    public async void CancelUnpublishingOfLanguageVariantAsync_NoIdentifier_Throws()
    {
        var client = _scenario.CreateManagementClient();

        await client.Invoking(x => x.CancelUnpublishingOfLanguageVariantAsync(null)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Theory]
    [ClassData(typeof(CombinationOfVariantIdentifiersAndUrl))]
    public async void ScheduleUnpublishingOfLanguageVariantAsync_SchedulesPublishingVariant(LanguageVariantIdentifier variantIdentifier, string expectedUrl)
    {
        var client = _scenario.CreateManagementClient();

        var schedule = new ScheduleModel
        {
            DisplayTimeZone = "prague",
            ScheduleTo = DateTimeOffset.UtcNow
        };

        await client.ScheduleUnpublishingOfLanguageVariantAsync(variantIdentifier, schedule);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Put)
            .RequestPayload(schedule)
            .Url($"{expectedUrl}/unpublish-and-archive")
            .Validate();
    }

    [Fact]
    public async void ScheduleUnpublishingOfLanguageVariantAsync_NoIdentifier_Throws()
    {
        var client = _scenario.CreateManagementClient();

        var schedule = new ScheduleModel
        {
            DisplayTimeZone = "prague",
            ScheduleTo = DateTimeOffset.UtcNow
        };

        await client.Invoking(x => x.ScheduleUnpublishingOfLanguageVariantAsync(null, schedule)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async void ScheduleUnpublishingOfLanguageVariantAsync_ScheduleModelIsNull_Throws()
    {
        var client = _scenario.CreateManagementClient();

        var identifier = new LanguageVariantIdentifier
            (
                itemIdentifier: Reference.ById(Guid.NewGuid()),
                languageIdentifier: Reference.ById(Guid.NewGuid())
            );

        await client.Invoking(x => x.ScheduleUnpublishingOfLanguageVariantAsync(identifier, null)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Theory]
    [ClassData(typeof(CombinationOfVariantIdentifiersAndUrl))]
    public async void CreateNewVersionOfLanguageVariantAsync_SchedulesPublishingVariant(LanguageVariantIdentifier variantIdentifier, string expectedUrl)
    {
        var client = _scenario.CreateManagementClient();

        await client.CreateNewVersionOfLanguageVariantAsync(variantIdentifier);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Put)
            .Url($"{expectedUrl}/new-version")
            .Validate();
    }

    [Fact]
    public async void CreateNewVersionOfLanguageVariantAsync_NoIdentifier_Throws()
    {
        var client = _scenario.CreateManagementClient();


        await client.Invoking(x => x.CreateNewVersionOfLanguageVariantAsync(null)).Should().ThrowAsync<ArgumentNullException>();
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
                    var url = $"{Endpoint}/projects/{ENVIRONMENT_ID}/items/{item.UrlSegment}/variants/{language.UrlSegment}";
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
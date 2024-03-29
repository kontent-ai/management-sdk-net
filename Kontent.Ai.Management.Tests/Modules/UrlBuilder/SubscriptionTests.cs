﻿using FluentAssertions;
using Kontent.Ai.Management.Models.Shared;
using System;
using Xunit;

namespace Kontent.Ai.Management.Tests.Modules.UrlBuilder;

public partial class EndpointUrlBuilderTests
{
    [Fact]
    public void BuildSubscriptionProjectsUrl_ReturnsCorrectUrl()
    {
        var actualUrl = _builder.BuildSubscriptionProjectsUrl();
        var expectedUrl = $"{ENDPOINT}/subscriptions/{SUBSCRIPTION_ID}/projects";

        Assert.Equal(expectedUrl, actualUrl);
    }

    [Fact]
    public void BuildSubscriptionUsersUrl_ReturnsCorrectUrl()
    {
        var actualUrl = _builder.BuildSubscriptionUsersUrl();
        var expectedUrl = $"{ENDPOINT}/subscriptions/{SUBSCRIPTION_ID}/users";

        Assert.Equal(expectedUrl, actualUrl);
    }

    [Fact]
    public void BuildSubscriptionUserUrl_ById_ReturnsCorrectUrl()
    {
        var userIdentifier = UserIdentifier.ById(Guid.NewGuid().ToString());

        var actualUrl = _builder.BuildSubscriptionUserUrl(userIdentifier);
        var expectedUrl = $"{ENDPOINT}/subscriptions/{SUBSCRIPTION_ID}/users/{userIdentifier.Id}";

        Assert.Equal(expectedUrl, actualUrl);
    }

    [Fact]
    public void BuildSubscriptionUserUrl_ByEmail_ReturnsCorrectUrl()
    {
        var userIdentifier = UserIdentifier.ByEmail("test@test.test");

        var actualUrl = _builder.BuildSubscriptionUserUrl(userIdentifier);
        var expectedUrl = $"{ENDPOINT}/subscriptions/{SUBSCRIPTION_ID}/users/email/{userIdentifier.Email}";

        Assert.Equal(expectedUrl, actualUrl);
    }

    [Fact]
    public void BuildSubscriptionUserUrl_MissingId_MissingEmail_ThrowsException()
    {
        _builder.Invoking(x => x.BuildSubscriptionUserUrl(UserIdentifier.ByEmail(null))).Should().ThrowExactly<ArgumentException>();
    }
}

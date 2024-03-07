﻿using Kontent.Ai.Management.Configuration;
using Kontent.Ai.Management.Modules.UrlBuilder;

namespace Kontent.Ai.Management.Tests.Modules.UrlBuilder;

public partial class EndpointUrlBuilderTests
{
    private const string ENVIRONMENT_ID = "bb6882a0-3088-405c-a6ac-4a0da46810b0";
    private const string SUBSCRIPTION_ID = "aaaaa2a0-3088-405c-a6ac-4a0da46810b0";
    private const string ENDPOINT = "https://manage.kontent.ai/v2";

    private static readonly ManagementOptions OPTIONS = new() { EnvironmentId = ENVIRONMENT_ID, SubscriptionId = SUBSCRIPTION_ID };

    private readonly EndpointUrlBuilder _builder;

    public EndpointUrlBuilderTests()
    {
        _builder = new EndpointUrlBuilder(OPTIONS);
    }
}

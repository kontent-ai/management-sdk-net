using FluentAssertions;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.WebSpotlight;
using Kontent.Ai.Management.Tests.Base;
using System;
using System.Net.Http;
using Xunit;
using static Kontent.Ai.Management.Tests.Base.Scenario;

#pragma warning disable CS0618 // Web Spotlight members are obsolete; these tests intentionally exercise them for backward compatibility.

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class WebSpotlightTests : IClassFixture<FileSystemFixture>
{
    private static readonly string WebSpotlightBaseUrl = $"{Endpoint}/projects/{ENVIRONMENT_ID}/web-spotlight";
    private readonly Scenario _scenario = new(folder: "WebSpotlight");

    [Fact]
    public async void ActivateWebSpotlight_Returns_DeprecatedStatus()
    {
        var client = _scenario
            .WithResponses("ActivationWebSpotlightResponse.json")
            .CreateManagementClient();

        var webSpotlightActivateModel = new WebSpotlightActivateModel { RootType = null };

        var response = await client
            .ActivateWebSpotlightAsync(webSpotlightActivateModel);

        response.Enabled.Should().BeFalse();
        response.RootTypeId.Should().BeNull();

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Put)
            .Response(response)
            .Url(WebSpotlightBaseUrl)
            .Validate();
    }

    [Fact]
    public async void ActivateWebSpotlight_WithProvidedValidRootTypeById_SerializesRequest()
    {
        var client = _scenario
            .WithResponses("ActivationWebSpotlightWithProvidedRootTypeIdResponse.json")
            .CreateManagementClient();

        var rootTypeId = Guid.Parse("3660e894-bae8-4dcd-9d3e-5fc9205c2ece");
        var reference = Reference.ById(rootTypeId);
        var webSpotlightActivateModel = new WebSpotlightActivateModel { RootType = reference };

        await client.ActivateWebSpotlightAsync(webSpotlightActivateModel);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Put)
            .RequestPayload(webSpotlightActivateModel)
            .Url(WebSpotlightBaseUrl)
            .Validate();
    }

    [Fact]
    public async void ActivateWebSpotlight_WithProvidedValidRootTypeByCodename_SerializesRequest()
    {
        var client = _scenario
            .WithResponses("ActivationWebSpotlightWithProvidedRootTypeIdResponse.json")
            .CreateManagementClient();

        var rootTypeCodename = "root_type_codename";
        var reference = Reference.ByCodename(rootTypeCodename);
        var webSpotlightActivateModel = new WebSpotlightActivateModel { RootType = reference };

        await client.ActivateWebSpotlightAsync(webSpotlightActivateModel);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Put)
            .RequestPayload(webSpotlightActivateModel)
            .Url(WebSpotlightBaseUrl)
            .Validate();
    }

    [Fact]
    public async void DeactivateWebSpotlight_Returns_DeprecatedStatus()
    {
        var client = _scenario
            .WithResponses("DeactivationWebSpotlightResponse.json")
            .CreateManagementClient();

        var response = await client
            .DeactivateWebSpotlightAsync();

        response.Enabled.Should().BeFalse();
        response.RootTypeId.Should().BeNull();

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Put)
            .Response(response)
            .Url(WebSpotlightBaseUrl)
            .Validate();
    }

    [Fact]
    public async void GetWebSpotlightStatus_Returns_DeprecatedStatus()
    {
        var client = _scenario
            .WithResponses("GetStatusWebSpotlightResponse.json")
            .CreateManagementClient();

        var response = await client
            .GetWebSpotlightStatusAsync();

        response.Enabled.Should().BeFalse();
        response.RootTypeId.Should().BeNull();

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Get)
            .Response(response)
            .Url(WebSpotlightBaseUrl)
            .Validate();
    }
}

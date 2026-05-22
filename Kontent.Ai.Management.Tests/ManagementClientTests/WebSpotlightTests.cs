using AwesomeAssertions;
using Kontent.Ai.Management.Models.WebSpotlight;
using Kontent.Ai.Management.Tests.Base;
using RichardSzalay.MockHttp;
using Xunit;
using System.Text.Json;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class WebSpotlightTests
{
    private static string WebSpotlightUrl => $"{MockClientFactory.BaseUrl}/web-spotlight";

    private static string ActivationWebSpotlightResponse => Fixture("ActivationWebSpotlightResponse.json");
    private static string ActivationWebSpotlightWithProvidedRootTypeIdResponse => Fixture("ActivationWebSpotlightWithProvidedRootTypeIdResponse.json");
    private static string DeactivationWebSpotlightResponse => Fixture("DeactivationWebSpotlightResponse.json");
    private static string GetStatusWebSpotlightResponse => Fixture("GetStatusWebSpotlightResponse.json");

    private static string Fixture(string name)
        => File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Data", "WebSpotlight", name));

    [Fact]
    public async Task ActivateWebSpotlight_Returns_EnabledStatusAndRootTypeId()
    {
        var (client, mock) = MockClientFactory.Create();
        var webSpotlightActivateModel = new WebSpotlightActivateModel { RootType = null };
        mock.Expect(HttpMethod.Put, WebSpotlightUrl)
            .Respond("application/json", ActivationWebSpotlightResponse);

        var result = await client.ActivateWebSpotlightAsync(webSpotlightActivateModel);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<WebSpotlightModel>(ActivationWebSpotlightResponse, SharedTestJsonOptions.Default));
    }

    [Fact]
    public async Task ActivateWebSpotlight_WithProvidedValidRootTypeById_Returns_EnabledStatusAndRootTypeId()
    {
        var (client, mock) = MockClientFactory.Create();
        var rootTypeId = Guid.Parse("3660e894-bae8-4dcd-9d3e-5fc9205c2ece");
        var webSpotlightActivateModel = new WebSpotlightActivateModel { RootType = Reference.ById(rootTypeId) };

        string? capturedBody = null;
        mock.Expect(HttpMethod.Put, WebSpotlightUrl)
            .With(r =>
            {
                capturedBody = r.Content!.ReadAsStringAsync().GetAwaiter().GetResult();
                return true;
            })
            .Respond("application/json", ActivationWebSpotlightWithProvidedRootTypeIdResponse);

        var result = await client.ActivateWebSpotlightAsync(webSpotlightActivateModel);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        capturedBody.Should().NotBeNull();
        JsonSerializer.Deserialize<WebSpotlightActivateModel>(capturedBody!, SharedTestJsonOptions.Default)
            .Should().BeEquivalentTo(JsonSerializer.Deserialize<WebSpotlightActivateModel>(JsonSerializer.Serialize(webSpotlightActivateModel, SharedTestJsonOptions.Default), SharedTestJsonOptions.Default));
    }

    [Fact]
    public async Task ActivateWebSpotlight_WithProvidedValidRootTypeByCodename_Returns_EnabledStatusAndRootTypeId()
    {
        var (client, mock) = MockClientFactory.Create();
        var webSpotlightActivateModel = new WebSpotlightActivateModel { RootType = Reference.ByCodename("root_type_codename") };

        string? capturedBody = null;
        mock.Expect(HttpMethod.Put, WebSpotlightUrl)
            .With(r =>
            {
                capturedBody = r.Content!.ReadAsStringAsync().GetAwaiter().GetResult();
                return true;
            })
            .Respond("application/json", ActivationWebSpotlightWithProvidedRootTypeIdResponse);

        var result = await client.ActivateWebSpotlightAsync(webSpotlightActivateModel);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        capturedBody.Should().NotBeNull();
        JsonSerializer.Deserialize<WebSpotlightActivateModel>(capturedBody!, SharedTestJsonOptions.Default)
            .Should().BeEquivalentTo(JsonSerializer.Deserialize<WebSpotlightActivateModel>(JsonSerializer.Serialize(webSpotlightActivateModel, SharedTestJsonOptions.Default), SharedTestJsonOptions.Default));
    }

    [Fact]
    public async Task DeactivateWebSpotlight_Returns_DisabledStatusAndRootTypeId()
    {
        var (client, mock) = MockClientFactory.Create();
        mock.Expect(HttpMethod.Put, WebSpotlightUrl)
            .Respond("application/json", DeactivationWebSpotlightResponse);

        var result = await client.DeactivateWebSpotlightAsync();

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<WebSpotlightModel>(DeactivationWebSpotlightResponse, SharedTestJsonOptions.Default));
    }

    [Fact]
    public async Task GetWebSpotlightStatus_Returns_StatusAndRootTypeId()
    {
        var (client, mock) = MockClientFactory.Create();
        mock.Expect(HttpMethod.Get, WebSpotlightUrl)
            .Respond("application/json", GetStatusWebSpotlightResponse);

        var result = await client.GetWebSpotlightStatusAsync();

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<WebSpotlightModel>(GetStatusWebSpotlightResponse, SharedTestJsonOptions.Default));
    }
}

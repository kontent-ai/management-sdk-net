using AwesomeAssertions;
using Kontent.Ai.Management.Models.PreviewConfiguration;
using Kontent.Ai.Management.Tests.Base;
using RichardSzalay.MockHttp;
using System.Text.Json;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class PreviewConfigurationTests
{
    private static string PreviewConfiguration => Fixture("PreviewConfiguration.json");

    private static string Fixture(string name)
        => File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Data", "PreviewConfiguration", name));

    [Fact]
    public async Task GetPreviewConfigurations_GetsPreviewConfiguration()
    {
        var (client, mock) = MockClientFactory.Create();
        mock.Expect(HttpMethod.Get, $"{MockClientFactory.BaseUrl}/preview-configuration")
            .Respond("application/json", PreviewConfiguration);

        var result = await client.GetPreviewConfigurationAsync();

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<PreviewConfigurationModel>(PreviewConfiguration, SharedTestJsonOptions.Default));
    }

    [Fact]
    public async Task ModifyPreviewConfiguration_ModifiesPreviewConfiguration()
    {
        var (client, mock) = MockClientFactory.Create();
        var request = new PreviewConfigurationModel
        {
            SpaceDomains = new List<SpaceDomainModel>
            {
                new()
                {
                    Domain = "www.mysite.com",
                    Space = Reference.ByCodename("my_space")
                }
            },
            PreviewUrlPatterns = new List<TypePreviewUrlPatternModel>
            {
                new()
                {
                    ContentType = Reference.ByCodename("article"),
                    UrlPatterns = new List<PreviewUrlPatternModel>
                    {
                        new()
                        {
                            Space = null,
                            UrlPattern = "https://www.globalsite.com/{URLSlug}"
                       },
                       new()
                       {
                           Space = Reference.ByCodename("my_space"),
                           UrlPattern = "https://{Space}/{URLSlug}/test"
                       },
                   }
               }
           }
        };

        string? capturedBody = null;
        mock.Expect(HttpMethod.Put, $"{MockClientFactory.BaseUrl}/preview-configuration")
            .With(r =>
            {
                capturedBody = r.Content!.ReadAsStringAsync().GetAwaiter().GetResult();
                return true;
            })
            .Respond("application/json", PreviewConfiguration);

        var result = await client.ModifyPreviewConfigurationAsync(request);

        mock.VerifyNoOutstandingExpectation();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(JsonSerializer.Deserialize<PreviewConfigurationModel>(PreviewConfiguration, SharedTestJsonOptions.Default));
        capturedBody.Should().NotBeNull();
        JsonSerializer.Deserialize<PreviewConfigurationModel>(capturedBody!, SharedTestJsonOptions.Default)
            .Should().BeEquivalentTo(JsonSerializer.Deserialize<PreviewConfigurationModel>(JsonSerializer.Serialize(request, SharedTestJsonOptions.Default), SharedTestJsonOptions.Default));
    }
}

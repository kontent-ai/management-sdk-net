using System.Collections.Generic;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.PreviewConfiguration;
using Kontent.Ai.Management.Tests.Base;
using Xunit;
using System.Net.Http;
using static Kontent.Ai.Management.Tests.Base.Scenario;

namespace Kontent.Ai.Management.Tests.ManagementClientTests;

public class PreviewConfigurationTests
{
    private readonly Scenario _scenario;

    public PreviewConfigurationTests()
    {
        _scenario = new Scenario(folder: "PreviewConfiguration");
    }

    [Fact]
    public async void GetPreviewConfigurations_GetsPreviewConfiguration()
    {
        var client = _scenario
            .WithResponses("PreviewConfiguration.json")
            .CreateManagementClient();

        var response = await client.GetPreviewConfigurationAsync();

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Get)
            .Response(response)
            .Url($"{Endpoint}/projects/{ENVIRONMENT_ID}/preview-configuration")
            .Validate();
    }

    [Fact]
    public async void ModifyPreviewConfiguration_ModifiesPreviewConfiguration()
    {
        var client = _scenario
            .WithResponses("PreviewConfiguration.json")
            .CreateManagementClient();

        var newPreviewConfiguration = new PreviewConfigurationModel
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

        var response = await client.ModifyPreviewConfigurationAsync(newPreviewConfiguration);

        _scenario
            .CreateExpectations()
            .HttpMethod(HttpMethod.Put)
            .RequestPayload(newPreviewConfiguration)
            .Response(response)
            .Url($"{Endpoint}/projects/{ENVIRONMENT_ID}/preview-configuration")
            .Validate();
    }

}

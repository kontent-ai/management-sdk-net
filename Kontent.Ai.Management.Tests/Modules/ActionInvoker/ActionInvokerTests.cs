using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Kontent.Ai.Management.Models.Assets;
using Xunit;
using System.Collections.Generic;
using System;
using _ActionInvoker = Kontent.Ai.Management.Modules.ActionInvoker.ActionInvoker;
using Kontent.Ai.Management.Models.Assets;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Modules.HttpClient;
using Kontent.Ai.Management.Models.LanguageVariants;
using Kontent.Ai.Management.Modules.ActionInvoker;

namespace Kontent.Ai.Management.Tests.Modules.ActionInvoker;

internal class FakeManagementHttpClient : IManagementHttpClient
{
    internal string _requestBody;

    public async Task<HttpResponseMessage> SendAsync(IMessageCreator messageCreator, string endpointUrl, HttpMethod method, HttpContent content = null, Dictionary<string, string> headers = null)
    {
        var message = messageCreator.CreateMessage(method, endpointUrl, content);
        _requestBody = await message.Content.ReadAsStringAsync();
        return new HttpResponseMessage(HttpStatusCode.Accepted)
        {
            Content = new StringContent(_requestBody)
        };
    }
}

[Trait("Test", "1")]
public class ActionInvokerTests
{
    [Theory]
    [Trait("Issue", "29")]
    [InlineData(0.0, "0")]
    [InlineData(29.0, "29.0")]
    public async Task ActionInvokerSerializeElementContainingZero_SerializedJsonContainsZero(decimal d, string s)
    {
        var httpClient = new FakeManagementHttpClient();
        var actionInvoker = new _ActionInvoker(httpClient, new MessageCreator("{api_key}"));

        var languageVariantUpsertModel = new LanguageVariantUpsertModel()
        {
            Elements = new List<dynamic>
            {
                new
                {
                    zero = d,
                    optZero = new decimal?(d),
                }
            },
        };

        await actionInvoker.InvokeMethodAsync<LanguageVariantUpsertModel, dynamic>("{endpoint_url}", HttpMethod.Get, languageVariantUpsertModel);
        Assert.Equal($"{{\"elements\":[{{\"zero\":{s},\"optZero\":{s}}}]}}", httpClient._requestBody);
    }

    [Fact]
    public async Task ActionInvokerSerializeEnum_EnumIsSerializedAsString()
    {
        var httpClient = new FakeManagementHttpClient();
        var actionInvoker = new _ActionInvoker(httpClient, new MessageCreator("{api_key}"));

        var assetUpsertModel = new AssetUpsertModel()
        {
            Title = "Asset",
            Descriptions = new[]
            {
                new AssetDescription()
                {
                    Description = "Description",
                    Language = Reference.ById(Guid.Empty)
                },
            },
            FileReference = new FileReference()
            {
                Id = "ab7bdf75-781b-4bf9-aed8-501048860402",
                Type = FileReferenceTypeEnum.Internal
            }
        };

        await actionInvoker.InvokeMethodAsync<AssetUpsertModel, dynamic>("{endpoint_url}", HttpMethod.Put, assetUpsertModel);

        Assert.Contains("\"type\":\"internal\"", httpClient._requestBody);
    }
}

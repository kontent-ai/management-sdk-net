using System.Net;
using FluentAssertions;
using Kontent.Ai.Management.Api;
using Kontent.Ai.Management.Configuration;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.Spaces;
using Kontent.Ai.Management.Models.Spaces.Patch;
using Xunit;

namespace Kontent.Ai.Management.Tests.Api;

public class IManagementApiSmokeTests
{
    private const string EnvironmentId = "00000000-0000-0000-0000-000000000001";
    private const string ApiKey = "dummy-api-key";
    private const string EnvPrefix = $"/v2/projects/{EnvironmentId}";

    private sealed class CapturingHandler(string responseBody) : HttpMessageHandler
    {
        public HttpRequestMessage? LastRequest { get; private set; }
        public string? LastRequestBody { get; private set; }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            LastRequest = request;
            if (request.Content is not null)
            {
                LastRequestBody = await request.Content.ReadAsStringAsync(cancellationToken);
            }

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(responseBody, System.Text.Encoding.UTF8, "application/json"),
            };
        }
    }

    private static (IManagementApi api, CapturingHandler handler) CreateApi(string responseBody = "{}")
    {
        var handler = new CapturingHandler(responseBody);
        var api = ManagementApiFactory.Create(new ManagementOptions { ApiKey = ApiKey, EnvironmentId = EnvironmentId }, handler);
        return (api, handler);
    }

    [Fact]
    public async Task ListSpaces_GetsSpacesCollection()
    {
        var (api, handler) = CreateApi("[]");
        await api.ListSpacesInternalAsync();
        handler.LastRequest!.Method.Should().Be(HttpMethod.Get);
        handler.LastRequest!.RequestUri!.AbsolutePath.Should().Be($"{EnvPrefix}/spaces");
    }

    [Fact]
    public async Task Request_CarriesBearerAuthAndSdkTrackingHeaders()
    {
        var (api, handler) = CreateApi("[]");
        await api.ListSpacesInternalAsync();

        handler.LastRequest!.Headers.Authorization.Should().BeEquivalentTo(new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", ApiKey));
        handler.LastRequest!.Headers.GetValues("X-KC-SDKID").Should().ContainSingle().Which.Should().Contain("Kontent.Ai.Management");
    }

    [Fact]
    public async Task GetSpace_ById_BuildsBareIdSegment()
    {
        var (api, handler) = CreateApi();
        var id = Guid.Parse("9a86a4c0-0e6a-4b3d-8b6a-000000000002");
        await api.GetSpaceInternalAsync(Reference.ById(id).ToUrlSegment());
        handler.LastRequest!.Method.Should().Be(HttpMethod.Get);
        handler.LastRequest!.RequestUri!.AbsolutePath.Should().Be($"{EnvPrefix}/spaces/{id}");
    }

    [Fact]
    public async Task GetSpace_ByCodename_KeepsCodenameSlashSegment()
    {
        var (api, handler) = CreateApi();
        await api.GetSpaceInternalAsync(Reference.ByCodename("my_space").ToUrlSegment());
        handler.LastRequest!.RequestUri!.AbsolutePath.Should().Be($"{EnvPrefix}/spaces/codename/my_space");
    }

    [Fact]
    public async Task CreateSpace_PostsToCollectionWithBody()
    {
        var (api, handler) = CreateApi();
        await api.CreateSpaceInternalAsync(new SpaceCreateModel { Name = "Marketing", Codename = "marketing" });
        handler.LastRequest!.Method.Should().Be(HttpMethod.Post);
        handler.LastRequest!.RequestUri!.AbsolutePath.Should().Be($"{EnvPrefix}/spaces");
        handler.LastRequestBody.Should().Contain("\"name\":\"Marketing\"").And.Contain("\"codename\":\"marketing\"");
    }

    [Fact]
    public async Task ModifySpace_PatchesIdentifiedSpaceWithOperationsBody()
    {
        var (api, handler) = CreateApi();
        await api.ModifySpaceInternalAsync(
            Reference.ByCodename("marketing").ToUrlSegment(),
            new[] { new SpaceOperationReplaceModel { PropertyName = PropertyName.Name, Value = "Marketing 2" } });
        handler.LastRequest!.Method.Should().Be(HttpMethod.Patch);
        handler.LastRequest!.RequestUri!.AbsolutePath.Should().Be($"{EnvPrefix}/spaces/codename/marketing");
        // the transitional Newtonsoft serializer must keep the legacy converter behaviour (string enums via [EnumMember])
        handler.LastRequestBody.Should().Contain("\"op\":\"replace\"").And.Contain("\"property_name\":\"name\"");
    }

    [Fact]
    public async Task DeleteSpace_DeletesIdentifiedSpace()
    {
        var (api, handler) = CreateApi();
        await api.DeleteSpaceInternalAsync(Reference.ById(Guid.Parse("9a86a4c0-0e6a-4b3d-8b6a-000000000002")).ToUrlSegment());
        handler.LastRequest!.Method.Should().Be(HttpMethod.Delete);
        handler.LastRequest!.RequestUri!.AbsolutePath.Should().Be($"{EnvPrefix}/spaces/9a86a4c0-0e6a-4b3d-8b6a-000000000002");
    }
}

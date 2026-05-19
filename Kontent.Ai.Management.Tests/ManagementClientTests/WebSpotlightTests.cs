using AwesomeAssertions;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.WebSpotlight;
using Kontent.Ai.Management.Tests.Base;
using Newtonsoft.Json;
using RichardSzalay.MockHttp;
using Xunit;

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

        var response = await client.ActivateWebSpotlightAsync(webSpotlightActivateModel);

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(JsonConvert.DeserializeObject<WebSpotlightModel>(ActivationWebSpotlightResponse));
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

        await client.ActivateWebSpotlightAsync(webSpotlightActivateModel);

        mock.VerifyNoOutstandingExpectation();
        capturedBody.Should().NotBeNull();
        JsonConvert.DeserializeObject<WebSpotlightActivateModel>(capturedBody!)
            .Should().BeEquivalentTo(JsonConvert.DeserializeObject<WebSpotlightActivateModel>(JsonConvert.SerializeObject(webSpotlightActivateModel)));
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

        await client.ActivateWebSpotlightAsync(webSpotlightActivateModel);

        mock.VerifyNoOutstandingExpectation();
        capturedBody.Should().NotBeNull();
        JsonConvert.DeserializeObject<WebSpotlightActivateModel>(capturedBody!)
            .Should().BeEquivalentTo(JsonConvert.DeserializeObject<WebSpotlightActivateModel>(JsonConvert.SerializeObject(webSpotlightActivateModel)));
    }

    [Fact]
    public async Task DeactivateWebSpotlight_Returns_DisabledStatusAndRootTypeId()
    {
        var (client, mock) = MockClientFactory.Create();
        mock.Expect(HttpMethod.Put, WebSpotlightUrl)
            .Respond("application/json", DeactivationWebSpotlightResponse);

        var response = await client.DeactivateWebSpotlightAsync();

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(JsonConvert.DeserializeObject<WebSpotlightModel>(DeactivationWebSpotlightResponse));
    }

    [Fact]
    public async Task GetWebSpotlightStatus_Returns_StatusAndRootTypeId()
    {
        var (client, mock) = MockClientFactory.Create();
        mock.Expect(HttpMethod.Get, WebSpotlightUrl)
            .Respond("application/json", GetStatusWebSpotlightResponse);

        var response = await client.GetWebSpotlightStatusAsync();

        mock.VerifyNoOutstandingExpectation();
        response.Should().BeEquivalentTo(JsonConvert.DeserializeObject<WebSpotlightModel>(GetStatusWebSpotlightResponse));
    }
}

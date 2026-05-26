using AwesomeAssertions;
using Kontent.Ai.Management.Modules.Extensions;

namespace Kontent.Ai.Management.Tests.Modules.Extensions;

public class HttpRequestHeadersExtensionsTests
{
    [Fact]
    public void AddSdkTrackingHeader_AddsRepositoryPackageIdAndStrippedVersion()
    {
        var sdkAssembly = typeof(ManagementClient).Assembly;
        var request = new HttpRequestMessage();

        request.Headers.AddSdkTrackingHeader();

        request.Headers.GetValues("X-KC-SDKID").Should()
            .ContainSingle().Which.Should()
            .Be($"nuget.org;{sdkAssembly.GetName().Name};{sdkAssembly.GetProductVersion()}");
    }

    [Fact]
    public void AddSourceTrackingHeader_AddsValueFromConsumingAssemblyAttribute()
    {
        // The test assembly carries [assembly: SourceTrackingHeader] (parameterless) via its .csproj.
        var consumingAssembly = typeof(HttpRequestHeadersExtensionsTests).Assembly;
        var request = new HttpRequestMessage();

        request.Headers.AddSourceTrackingHeader();

        request.Headers.GetValues("X-KC-SOURCE").Should()
            .ContainSingle().Which.Should()
            .Be($"{consumingAssembly.GetName().Name};{consumingAssembly.GetProductVersion()}");
    }
}

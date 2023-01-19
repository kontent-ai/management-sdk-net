using Kontent.Ai.Management.Modules.Extensions;
using Kontent.Ai.Management.Modules.HttpClient;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using Xunit;

namespace Kontent.Ai.Management.Tests.Modules.Extensions;

public class HttpRequestHeadersExtensionsTests
{
    [Fact]
    public void AddSdkTrackingHeader_CorrectSdkVersionHeaderAdded()
    {
        var assembly = typeof(ManagementHttpClient).Assembly;
        var fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
        var sdkVersion = fileVersionInfo.ProductVersion;
        var sdkPackageId = assembly.GetName().Name;
        var httpRequestMessage = new HttpRequestMessage();

        httpRequestMessage.Headers.AddSdkTrackingHeader();

        httpRequestMessage.Headers.TryGetValues("X-KC-SDKID", out var headerContent);

        Assert.True(httpRequestMessage.Headers.Contains("X-KC-SDKID"));
        Assert.Contains($"nuget.org;{sdkPackageId};{sdkVersion}", headerContent);
    }

    [Fact]
    public void SourceTrackingHeaderGeneratedFromAssembly()
    {
        var assembly = GetType().Assembly;
        var fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
        var sourceVersion = fileVersionInfo.ProductVersion;

        var request = new HttpRequestMessage();

        HttpRequestHeadersExtensions.AddSourceTrackingHeader(request.Headers);

        Assert.Equal($"{assembly.GetName().Name};{sourceVersion}", request.Headers.GetValues("X-KC-SOURCE").First());
    }
}

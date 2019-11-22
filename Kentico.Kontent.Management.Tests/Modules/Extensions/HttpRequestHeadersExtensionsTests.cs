using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;

using Kentico.Kontent.Management.Modules.HttpClient;
using Kentico.Kontent.Management.Modules.Extensions;

using Xunit;

namespace Kentico.Kontent.Management.Tests
{
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

            IEnumerable<string> headerContent = new List<string>();
            httpRequestMessage.Headers.TryGetValues("X-KC-SDKID", out headerContent);

            Assert.True(httpRequestMessage.Headers.Contains("X-KC-SDKID"));
            Assert.Contains($"nuget.org;{sdkPackageId};{sdkVersion}", headerContent);
        }
    }
}

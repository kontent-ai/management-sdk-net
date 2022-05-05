using System.Collections.Generic;
using System.Net.Http;

namespace Kentico.Kontent.Management.Tests.Base;

internal record HttpClientMockData(
    string Url,
    HttpMethod HttpMethod,
    string Payload,
    Dictionary<string, string> Headers);
using System.Collections.Generic;
using System.Net.Http;

namespace Kontent.Ai.Management.Tests.Base;

internal record HttpClientMockData(
    string Url,
    HttpMethod HttpMethod,
    string Payload,
    Dictionary<string, string> Headers);
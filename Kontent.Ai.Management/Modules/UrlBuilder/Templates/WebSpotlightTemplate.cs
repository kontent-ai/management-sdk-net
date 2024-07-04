using System;

namespace Kontent.Ai.Management.Modules.UrlBuilder.Templates;

internal class WebSpotlightTemplate : UrlTemplate
{
    public override string Url => "/web-spotlight";
    public override string UrlId => throw new InvalidOperationException("Web Spotlight does not have Id Url.");
    public override string UrlCodename => throw new InvalidOperationException("Web Spotlight does not have Codename Url.");
    public override string UrlExternalId => throw new InvalidOperationException("Web Spotlight does not have External Id Url.");
}
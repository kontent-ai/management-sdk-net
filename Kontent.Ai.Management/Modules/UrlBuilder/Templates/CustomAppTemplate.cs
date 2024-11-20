using System;

namespace Kontent.Ai.Management.Modules.UrlBuilder.Templates;

internal class CustomAppTemplate : UrlTemplate
{
    public override string Url => "/custom-apps";
    public override string UrlId => "/custom-apps/{0}";
    public override string UrlCodename => "/custom-apps/codename/{0}";
    public override string UrlExternalId  => throw new InvalidOperationException("Custom apps do not have external id url");
}
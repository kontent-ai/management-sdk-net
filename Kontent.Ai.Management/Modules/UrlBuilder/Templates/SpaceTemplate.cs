using System;

namespace Kontent.Ai.Management.Modules.UrlBuilder.Templates;

internal class SpaceTemplate : UrlTemplate
{
    public override string Url => "/spaces";
    public override string UrlId => "/spaces/{0}";
    public override string UrlCodename => "/spaces/codename/{0}";
    
    public override string UrlExternalId => throw new InvalidOperationException("Spaces do not have external id url");
}
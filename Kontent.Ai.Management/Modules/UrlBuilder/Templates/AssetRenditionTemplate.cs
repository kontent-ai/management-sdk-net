using System;

namespace Kontent.Ai.Management.Modules.UrlBuilder.Templates;

internal class AssetRenditionTemplate : UrlTemplate
{
    public override string Url => "/renditions";

    public override string UrlId => "/renditions/{0}";

    public override string UrlCodename => throw new InvalidOperationException("asset renditions do not have codename url");

    public override string UrlExternalId => "/renditions/external-id/{0}";
}
using System;

namespace Kentico.Kontent.Management.Modules.UrlBuilder.Templates;

internal class AssetTemplate : UrlTemplate
{
    public override string Url => "/assets";

    public override string UrlId => "/assets/{0}";

    public override string UrlCodename => throw new InvalidOperationException("assets do not have codename url");

    public override string UrlExternalId => "/assets/external-id/{0}";
}

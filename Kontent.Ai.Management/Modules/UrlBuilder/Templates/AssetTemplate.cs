namespace Kontent.Ai.Management.Modules.UrlBuilder.Templates;

internal class AssetTemplate : UrlTemplate
{
    public override string Url => "/assets";

    public override string UrlId => "/assets/{0}";

    public override string UrlCodename => "/assets/codename/{0}";

    public override string UrlExternalId => "/assets/external-id/{0}";
}

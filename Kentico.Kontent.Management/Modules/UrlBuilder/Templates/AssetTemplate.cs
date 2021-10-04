namespace Kentico.Kontent.Management.Modules.UrlBuilder.Templates
{
    internal class AssetTemplate : UrlTemplate
    {
        public override string Url => "/assets";

        public override string UrlId => "/assets/{0}";

        public override string UrlCodename => "/assets/external-id/{0}";

        public override string UrlExternalId => "/assets/external-id/{0}";
    }
}

namespace Kentico.Kontent.Management.Modules.UrlBuilder.Templates
{
    internal class ItemTemplate : UrlTemplate
    {
        public override string Url => "/items";

        public override string UrlId => "/items/{0}";

        public override string UrlCodename => "/items/codename/{0}";

        public override string UrlExternalId => "/items/external-id/{0}";
    }
}

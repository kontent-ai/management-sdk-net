using Kentico.Kontent.Management.Modules.UrlBuilder;

namespace Kentico.Kontent.Management.Modules.UrlBuilder.Templates
{
    internal class TypeTemplate : UrlTemplate
    {
        public override string Url => "/types";

        public override string UrlId => "/types/{0}";

        public override string UrlCodename => "/types/codename/{0}";

        public override string UrlExternalId => "/types/external-id/{0}";
    }
}

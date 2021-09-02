using System;

namespace Kentico.Kontent.Management.Modules.UrlBuilder.Templates
{
    internal class WebhookTemplate : UrlTemplate
    {
        public override string Url => "/webhooks";

        public override string UrlId => "/webhooks/{0}";

        public override string UrlCodename => throw new NotImplementedException("webhooks do not have codename url");

        public override string UrlExternalId => throw new NotImplementedException("webhooks do not have external id url");
    }
}

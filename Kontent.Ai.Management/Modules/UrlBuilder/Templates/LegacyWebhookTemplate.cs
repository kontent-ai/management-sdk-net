using System;

namespace Kontent.Ai.Management.Modules.UrlBuilder.Templates;

internal class LegacyWebhookTemplate : UrlTemplate
{
    public override string Url => "/webhooks";

    public override string UrlId => "/webhooks/{0}";

    public override string UrlCodename => throw new InvalidOperationException("webhooks do not have codename url");

    public override string UrlExternalId => throw new InvalidOperationException("webhooks do not have external id url");
}
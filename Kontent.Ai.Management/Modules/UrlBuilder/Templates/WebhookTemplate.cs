﻿using System;

namespace Kontent.Ai.Management.Modules.UrlBuilder.Templates;

internal class WebhookTemplate : UrlTemplate
{
    public override string Url => "/webhooks-vnext";
    public override string UrlId => "/webhooks-vnext/{0}";
    public override string UrlCodename => throw new InvalidOperationException("webhooks do not have codename url");
    public override string UrlExternalId => throw new InvalidOperationException("webhooks do not have external id url");
}
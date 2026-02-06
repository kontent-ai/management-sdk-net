using System;

namespace Kontent.Ai.Management.Modules.UrlBuilder.Templates;

internal class ItemWithVariantTemplate : UrlTemplate
{
    public override string Url => "/items-with-variant";
    public override string UrlId => throw new InvalidOperationException("ItemWithVariant does not have Id Url.");
    public override string UrlCodename => throw new InvalidOperationException("ItemWithVariant does not have Codename Url.");
    public override string UrlExternalId => throw new InvalidOperationException("ItemWithVariant does not have External Id Url.");
}

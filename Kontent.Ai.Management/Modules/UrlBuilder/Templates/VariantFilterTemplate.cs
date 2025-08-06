using System;

namespace Kontent.Ai.Management.Modules.UrlBuilder.Templates;

internal class VariantFilterTemplate : UrlTemplate
{
    public override string Url => "/early-access/variants/filter";
    public override string UrlId => throw new InvalidOperationException("Variant Filter does not have Id Url.");
    public override string UrlCodename => throw new InvalidOperationException("Variant Filter does not have Codename Url.");
    public override string UrlExternalId => throw new InvalidOperationException("Variant Filter does not have External Id Url.");
}
using System;

namespace Kontent.Ai.Management.Modules.UrlBuilder.Templates;

internal class VariantTemplate : UrlTemplate
{
    public override string Url => "/variants";

    public override string UrlId => "/variants/{0}";

    public override string UrlCodename => "/variants/codename/{0}";

    public override string UrlExternalId => throw new InvalidOperationException("variants do not have external Id url");
}

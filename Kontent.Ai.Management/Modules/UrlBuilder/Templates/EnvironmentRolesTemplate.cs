using System;

namespace Kontent.Ai.Management.Modules.UrlBuilder.Templates;

internal class EnvironmentRolesTemplate : UrlTemplate
{
    public override string Url => "/roles";
    public override string UrlId => "/roles/{0}";
    public override string UrlCodename => "/roles/codename/{0}";
    public override string UrlExternalId => throw new InvalidOperationException("roles do not have external id url");
}

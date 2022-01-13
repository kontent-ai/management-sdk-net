using System;

namespace Kentico.Kontent.Management.Modules.UrlBuilder.Templates
{
    internal class ProjectRolesTemplate : UrlTemplate
    {
        public override string Url => "/roles";
        public override string UrlId => "/roles/{0}";
        public override string UrlCodename => "/roles/codename/{0}";
        public override string UrlExternalId => throw new InvalidOperationException("roles do not have external id url");
    }
}

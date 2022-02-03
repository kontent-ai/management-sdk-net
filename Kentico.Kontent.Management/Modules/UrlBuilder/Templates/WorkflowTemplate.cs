using System;

namespace Kentico.Kontent.Management.Modules.UrlBuilder.Templates
{
    internal class WorkflowTemplate : UrlTemplate
    {
        public override string Url => "/workflow";

        public override string UrlId => "/workflow/{0}";

        public override string UrlCodename => "/workflow/codename/{0}";

        public override string UrlExternalId => throw new InvalidOperationException("workflows do not have external id url");
    }
}

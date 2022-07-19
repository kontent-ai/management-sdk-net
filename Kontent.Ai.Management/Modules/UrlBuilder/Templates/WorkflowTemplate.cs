using System;

namespace Kontent.Ai.Management.Modules.UrlBuilder.Templates;

internal class WorkflowTemplate : UrlTemplate
{
    public override string Url => "/workflows";

    public override string UrlId => "/workflows/{0}";

    public override string UrlCodename => "/workflows/codename/{0}";

    public override string UrlExternalId => throw new InvalidOperationException("Workflow does not have external id url");
}

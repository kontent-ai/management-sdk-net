namespace Kontent.Ai.Management.Modules.UrlBuilder.Templates;

internal class SnippetTemplate : UrlTemplate
{
    public override string Url => "/snippets";

    public override string UrlId => "/snippets/{0}";

    public override string UrlCodename => "/snippets/codename/{0}";

    public override string UrlExternalId => "/snippets/external-id/{0}";
}

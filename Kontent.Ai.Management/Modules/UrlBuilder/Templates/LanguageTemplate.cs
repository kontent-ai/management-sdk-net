namespace Kontent.Ai.Management.Modules.UrlBuilder.Templates;

internal class LanguageTemplate : UrlTemplate
{
    public override string Url => "/languages";

    public override string UrlId => "/languages/{0}";

    public override string UrlCodename => "/languages/codename/{0}";

    public override string UrlExternalId => "/languages/external-id/{0}";
}

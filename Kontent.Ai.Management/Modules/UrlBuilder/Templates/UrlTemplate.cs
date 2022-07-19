using System;

namespace Kontent.Ai.Management.Modules.UrlBuilder.Templates;

internal abstract class UrlTemplate
{
    public abstract string Url { get; }
    public abstract string UrlId { get; }
    public abstract string UrlCodename { get; }
    public abstract string UrlExternalId { get; }

    public virtual string UrlEmail => throw new InvalidOperationException("Only users have email url");
}

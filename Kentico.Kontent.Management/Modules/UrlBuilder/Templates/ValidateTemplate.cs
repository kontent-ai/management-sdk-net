using System;

namespace Kentico.Kontent.Management.Modules.UrlBuilder.Templates
{
    internal class ValidateTemplate : UrlTemplate
    {
        public override string Url => "/validate";

        public override string UrlId => throw new InvalidOperationException("validate does not have id url");

        public override string UrlCodename => throw new InvalidOperationException("validate does not have codename url");

        public override string UrlExternalId => throw new InvalidOperationException("validate does not have external id url");
    }
}

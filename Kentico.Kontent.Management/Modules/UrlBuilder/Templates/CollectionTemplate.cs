using System;

namespace Kentico.Kontent.Management.Modules.UrlBuilder.Templates
{
    internal class CollectionTemplate : UrlTemplate
    {
        public override string Url => "/collections";

        public override string UrlId => throw new NotImplementedException("collections do not have id url");

        public override string UrlCodename => throw new NotImplementedException("collections do not have codename url");

        public override string UrlExternalId => throw new NotImplementedException("collections do not have external id url");
    }
}

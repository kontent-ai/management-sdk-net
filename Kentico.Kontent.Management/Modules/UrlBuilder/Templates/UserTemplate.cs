using System;

namespace Kentico.Kontent.Management.Modules.UrlBuilder.Templates
{
    internal class UserTemplate : UrlTemplate
    {
        public override string Url => "/users";

        public override string UrlId => "/users/{0}";

        public override string UrlCodename => throw new InvalidOperationException("Users do not have codename url");

        public override string UrlExternalId => throw new InvalidOperationException("Users do not have external id url");

        public override string UrlEmail => "/users/email/{0}";
    }
}

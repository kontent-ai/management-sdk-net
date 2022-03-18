using Kentico.Kontent.Management.Models.Shared;
using Kentico.Kontent.Management.Modules.UrlBuilder.Templates;
using System;
using System.Net;

namespace Kentico.Kontent.Management.Modules.Extensions;

internal static class UrlTemplateExtensions
{
    internal static string GetIdentifierUrlSegment(this UrlTemplate template, UserIdentifier identifier)
    {
        if (identifier.Id != null)
        {
            return string.Format(template.UrlId, identifier.Id);
        }

        if (!string.IsNullOrEmpty(identifier.Email))
        {
            return string.Format(template.UrlEmail, identifier.Email);
        }

        throw new ArgumentException("You must provide user id or email");
    }

    internal static string GetIdentifierUrlSegment(this UrlTemplate template, Reference identifier)
    {
        if (identifier.Id != null)
        {
            return string.Format(template.UrlId, identifier.Id);
        }

        if (!string.IsNullOrEmpty(identifier.Codename))
        {
            return string.Format(template.UrlCodename, identifier.Codename);
        }

        if (!string.IsNullOrEmpty(identifier.ExternalId))
        {
            var escapedExternalId = WebUtility.UrlEncode(identifier.ExternalId);
            return string.Format(template.UrlExternalId, escapedExternalId);
        }

        throw new ArgumentException("You must provide id, codename or externalId");
    }
}

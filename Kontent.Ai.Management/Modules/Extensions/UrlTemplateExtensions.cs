using System;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Modules.UrlBuilder.Templates;

namespace Kontent.Ai.Management.Modules.Extensions;

internal static class UrlTemplateExtensions
{
    internal static string GetIdentifierUrlSegment(
        this UrlTemplate template,
        UserIdentifier identifier
    )
    {
        if (identifier.Id != null)
        {
            return string.Format(template.UrlId, ToSafePathSegment(identifier.Id));
        }

        if (!string.IsNullOrEmpty(identifier.Email))
        {
            return string.Format(template.UrlEmail, ToSafePathSegment(identifier.Email));
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
            return string.Format(template.UrlCodename, ToSafePathSegment(identifier.Codename));
        }

        if (!string.IsNullOrEmpty(identifier.ExternalId))
        {
            return string.Format(template.UrlExternalId, ToSafePathSegment(identifier.ExternalId));
        }

        throw new ArgumentException("You must provide id, codename or externalId");
    }

    /// <summary>
    /// Escapes and validates a caller-supplied identifier (codename, external id, email, or upload file name) so it
    /// stays a single URL path segment. Prevents path traversal attacks.
    /// </summary>
    internal static string ToSafePathSegment(string value) =>
        value is "." or ".."
            ? throw new ArgumentException($"'{value}' is not a valid identifier.", nameof(value))
            : Uri.EscapeDataString(value);
}

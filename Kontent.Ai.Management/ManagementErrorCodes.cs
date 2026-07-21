namespace Kontent.Ai.Management;

/// <summary>
/// A curated set of Management API <c>error_code</c> values (<see cref="IError.ErrorCode"/>) that callers commonly
/// branch on — variant workflow-state conflicts, duplicate external IDs, concurrency and throughput limits.
/// </summary>
/// <remarks>
/// This is a convenience snapshot, not the authoritative or exhaustive list: the Management API owns the full set and
/// may add or change codes. Codes are intentionally not unique — the API reuses some across unrelated conditions
/// (for example <see cref="PublishedOrScheduledVariantCannotBeUpdated"/> covers both states, and other codes differ by
/// endpoint), so a code does not by itself identify a single condition. When the distinction matters, inspect
/// <see cref="IError.Message"/> as well.
/// </remarks>
public static class ManagementErrorCodes
{
    /// <summary>The object was modified by another request between read and write (HTTP 409). Re-read and retry.</summary>
    public const int OptimisticConcurrencyConflict = 12;

    /// <summary>The uploaded file exceeds the maximum supported size.</summary>
    public const int FileSizeExceeded = 206;

    /// <summary>The API rate limit was exceeded (HTTP 429). Back off and retry.</summary>
    public const int RateLimitExceeded = 10000;

    /// <summary>The <c>x-continuation</c> header is invalid. Use the token from the response's <c>pagination</c> object.</summary>
    public const int InvalidContinuationToken = 1007;

    // Variant workflow-state conflicts — returned when a variant is not in an editable state.

    /// <summary>The variant is published or scheduled and cannot be updated; create a new version, or unpublish / cancel scheduling.</summary>
    public const int PublishedOrScheduledVariantCannotBeUpdated = 204;

    /// <summary>The variant cannot be unpublished because it is not published.</summary>
    public const int VariantNotPublishedCannotBeUnpublished = 213;

    /// <summary>A new version cannot be created because the variant is not published.</summary>
    public const int VariantNotPublishedCannotCreateNewVersion = 214;

    /// <summary>A new version of a published variant cannot be archived; unpublish it first.</summary>
    public const int NewVersionOfPublishedVariantCannotBeArchived = 222;

    /// <summary>The variant is archived and cannot be updated; change its workflow step first.</summary>
    public const int ArchivedVariantCannotBeUpdated = 223;

    // Duplicate external IDs — a created entity reused an external ID already present in the environment.

    /// <summary>An asset with the given external ID already exists.</summary>
    public const int DuplicateAssetExternalId = 208;

    /// <summary>A taxonomy group with the given external ID already exists.</summary>
    public const int DuplicateTaxonomyExternalId = 210;

    /// <summary>A content type with the given external ID already exists.</summary>
    public const int DuplicateContentTypeExternalId = 211;

    /// <summary>A content type snippet with the given external ID already exists.</summary>
    public const int DuplicateContentTypeSnippetExternalId = 217;

    /// <summary>A language with the given external ID already exists.</summary>
    public const int DuplicateLanguageExternalId = 219;

    /// <summary>An asset rendition with the given external ID already exists.</summary>
    public const int DuplicateAssetRenditionExternalId = 234;
}

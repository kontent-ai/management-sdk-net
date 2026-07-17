# Kontent.ai Management SDK for .NET 9.0.0-beta-4

Fourth beta of the modernized Management SDK — a **security** release. It hardens how caller-supplied identifiers are turned into request paths, closing a path-traversal issue. For the full overview of the rewrite (result pattern, Refit + `System.Text.Json` transport, materialized listings, DI + fluent builder, immutable strongly-typed models), see the [9.0.0-beta-1 release notes](release-notes-9.0.0-beta-1.md).

> [!WARNING]
> This is a **prerelease**. Install it with `--prerelease` — without that flag you get the stable `8.x` API, which these notes do **not** describe. Breaking changes may still land between prereleases until the first stable `9.x` ships; pin an exact version if you need stability during the beta. For production today, stay on the latest stable `8.x`.

## Security

- **Identifier values can no longer retarget a request to another endpoint.** A codename, external id, user id, or email is rendered into the request path through a catch-all route that treats `/` and the dot-segments `.` / `..` as path structure rather than data. An application that passed an untrusted string into `Reference.ByCodename(...)`, `Reference.ByExternalId(...)`, or `UserIdentifier.ByEmail(...)` could therefore have a value like `../../webhooks-vnext` silently retarget an intended read/delete to a different Management API endpoint under the same API key — a path-traversal / confused-deputy issue (CWE-22). These values are now validated at the URL boundary and rejected with an `ArgumentException` if they contain `/`, `\`, or are exactly `.` or `..`. Every other character continues to be percent-encoded as before, so normal identifiers are unaffected.

  The one behavior change to be aware of: an **external id that legitimately contains a `/`**. The `8.x` SDK percent-encoded external ids, so a slash round-tripped as `%2F`; the modernized transport cannot encode a slash inside this path segment without either double-encoding every other value or losing the traversal protection, so a slash-bearing external id is now rejected rather than silently mis-routed. Address such a resource by id or codename instead.

- **Upload file names get the same protection.** The `fileName` passed to `FileContentSource` (and from there to `UploadFileAsync`) travels as a URL path segment where dot-segments survive escaping — a file named `..` would have retargeted the upload `POST` to the environment root. `FileContentSource` now rejects file names containing `/` or `\`, or equal to `.` / `..`, at construction. For the file-path constructor, the validation applies to the derived file name (`Path.GetFileName`), so ordinary paths are unaffected.

- **Empty identifiers are rejected instead of silently hitting a different endpoint.** An empty codename, external id, user id, or email collapses a single-resource route onto its parent collection route — `GetSubscriptionUserAsync(UserIdentifier.ById(""))` would have called the *list users* endpoint instead of failing. `Reference.ByCodename` / `ByExternalId` and `UserIdentifier.ById` / `ByEmail` now throw an `ArgumentException` for null, empty, or whitespace-only values, and the URL boundary rejects them as defense in depth.

## Fixes

- **Retried requests no longer accumulate duplicate tracking headers.** The resilience layer re-dispatches the same request message on retry; the `X-KC-SDKID` / `X-KC-SOURCE` headers were appended per attempt, so a retried request carried repeated values. The headers are now set idempotently.
- **`Retry-After` in HTTP-date form is honored.** Previously only the delta-seconds form was; a date form fell back to the default exponential backoff.
- **HTTP responses are disposed after mapping to results**, instead of lingering until finalization.
- **`DynamicElement` round-trips `"value": null`.** A fetched unset element re-upserted through `DynamicElement` used to have its `value` property omitted (the serializer's omit-null default), silently turning an explicit null into an absent property. The `Value` property is now exempt from null omission.
- **Typed content-model reads match element ids case-insensitively.** Ids are GUID strings; a model attribute carrying an uppercase id previously failed to bind its element silently. Registration of typed models is also no longer racy on a concurrent first use.
- **Serializing an undefined enum value now throws** (`ArgumentException`) instead of writing the numeric string (e.g. `"99"`) to the wire.
- **`IManagementResult.RequestUrl`** now matches its documented contract: it carries the request URL for HTTP-backed results and is `null` only when no single request URL applies (aggregate operations).
- The convenience extensions on `IManagementClient` and the asset-folder hierarchy helpers now throw `ArgumentNullException` for null arguments instead of failing with a `NullReferenceException`.

## Installation

```bash
dotnet add package Kontent.Ai.Management --prerelease
```

## Requirements

- .NET 8.0
- A Kontent.ai environment with Management API v2 access (a Management API key)

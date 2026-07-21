# Kontent.ai Management SDK for .NET 9.0.0-beta-4

Fourth beta of the modernized Management SDK, and primarily a **security** release: it closes a path-traversal issue in how caller-supplied identifiers become request paths. It also fixes several correctness and reliability bugs and adds a small ergonomic improvement. For the full overview of the rewrite (result pattern, Refit + `System.Text.Json` transport, materialized listings, DI + fluent builder, immutable strongly-typed models), see the [9.0.0-beta-1 release notes](release-notes-9.0.0-beta-1.md).

> [!WARNING]
> This is a **prerelease**. Install it with `--prerelease` — without that flag you get the stable `8.x` API, which these notes do **not** describe. Breaking changes may still land between prereleases until the first stable `9.x` ships; pin an exact version if you need stability during the beta. For production today, stay on the latest stable `8.x`.

## Security

An untrusted string passed as an identifier could previously escape its path segment and retarget a request to a different Management API endpoint under the same API key — a path-traversal / confused-deputy issue (CWE-22). Identifiers are now validated at the URL boundary, so a value containing `/` or `\`, equal to `.` / `..`, or empty/whitespace-only is rejected with an `ArgumentException`. Normal identifiers are unaffected. This covers:

- **Codenames, external ids, user ids, and emails** — e.g. `Reference.ByCodename("../../webhooks-vnext")` or `UserIdentifier.ByEmail(...)`.
- **Upload file names** — the `fileName` given to `FileContentSource` (a `..` would have retargeted the upload to the environment root).
- **Empty identifiers** — an empty value used to collapse a single-resource call onto its parent collection route (e.g. `GetSubscriptionUserAsync(UserIdentifier.ById(""))` hit the *list users* endpoint); it now throws.

**One behavior change to note:** an **external id that legitimately contains a `/`** is now rejected rather than silently mis-routed. (The `8.x` SDK encoded it as `%2F`; the modernized transport can't do that without either double-encoding other values or losing the traversal protection.) Address such a resource by id or codename instead.

## Breaking changes

- **`ScheduleModel.ScheduleTo` → `ScheduledTo`.** The property now matches its wire field (`scheduled_to`) and the neighboring `SchedulePublishAndUnpublishModel.PublishScheduledTo` / `.UnpublishScheduledTo`. It's a `required` property, so the compiler flags every call site.

## Improvements

- **Configuration-based client registration now accepts the HTTP customization hooks.** The `AddManagementClient` overloads that bind from `IConfiguration` / `IConfigurationSection` take the same optional `configureHttpClient` / `configureResilience` / `configureRefit` parameters as the action-based overloads.

## Fixes

- **`DynamicElement` no longer drops explicit `null` values.** A fetched element with `"value": null` re-upserted through `DynamicElement` used to have its `value` silently omitted from the request, turning an explicit null into an absent property. It now round-trips.
- **Strongly-typed reads match element ids case-insensitively.** A model whose `[KontentElement]` id was not lowercase previously failed to bind that element with no error. First-time use of typed models from multiple threads is also no longer racy.
- **`Retry-After` response headers in HTTP-date form are honored**, not just the delta-seconds form.
- **Retried requests no longer send duplicate `X-KC-SDKID` / `X-KC-SOURCE` tracking headers.**
- **Serializing an out-of-range enum value throws** instead of sending an invalid token (e.g. `"99"`) to the API.
- **Null arguments to the client convenience extensions and asset-folder helpers throw `ArgumentNullException`** instead of a `NullReferenceException`.

## Installation

```bash
dotnet add package Kontent.Ai.Management --prerelease
```

## Requirements

- .NET 8.0
- A Kontent.ai environment with Management API v2 access (a Management API key)

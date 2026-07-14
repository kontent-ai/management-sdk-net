# Kontent.ai Management SDK for .NET 9.0.0-beta-3

Third beta of the modernized Management SDK — an **ergonomics-and-alignment** release. It rounds off the API surface with small, explicit conveniences drawn from real call-site friction, finishes the naming and collection-type conventions the rewrite started, and modernizes every doc sample to teach the patterns the SDK actually ships. For the full overview of the rewrite (result pattern, Refit + `System.Text.Json` transport, materialized listings, DI + fluent builder, immutable strongly-typed models), see the [9.0.0-beta-1 release notes](release-notes-9.0.0-beta-1.md).

> [!WARNING]
> This is a **prerelease**. Install it with `--prerelease` — without that flag you get the stable `8.x` API, which these notes do **not** describe. Breaking changes may still land between prereleases until the first stable `9.x` ships; pin an exact version if you need stability during the beta. For production today, stay on the latest stable `8.x`.

> [!IMPORTANT]
> **Upgrading from `8.x`?** Read the [upgrade guide](upgrade-guide.md) first — it covers every breaking change with before/after examples. If you're already on `9.0.0-beta-2`, see [API refinements](#api-refinements-breaking-vs-900-beta-2) below for the beta-to-beta changes.

## New

- **`ToReference()` on fetched models.** The models you list and act on (`ContentItemModel`, `AssetModel`, `ContentTypeModel`, `LanguageModel`, `WorkflowModel`, and five more) convert straight back into the `Reference` other calls expect: `client.DeleteContentItemAsync(item.ToReference())`. References by **id** deliberately — the docs on each method point you to `Reference.ByCodename(...)` for cross-environment scripts. *(Closes the last open ask from [#272](https://github.com/kontent-ai/management-sdk-net/issues/272); the proposed implicit conversion was declined in favor of this explicit form.)*
- **`ToIdentifier()` for variant workflows.** A fetched variant or an items-with-variants filter result feeds directly into publish/upsert/workflow calls — `client.PublishLanguageVariantAsync(found.ToIdentifier())` — and `ToVariantIdentifier()` chains filter results into a bulk get. No more manual `(item, language)` reassembly in filter → act loops.
- **`GetPublishedLanguageVariantAsync<T>`.** The typed projection of the published-variant endpoint, with parity to `GetLanguageVariantAsync<T>`.
- **Patch factories for every property-name domain.** `LanguagePatch`, `SpacePatch`, `TaxonomyGroupPatch`, and `CustomAppPatch` join `ContentTypePatch`: each names the property and takes the correctly-typed value, replacing the `object Value` guessing game — `LanguagePatch.FallbackLanguage(Reference.ByCodename("en-US"))`, `SpacePatch.RootItem(null)` to unset, `CustomAppPatch.AddAllowedRole(role)`.
- **Page streams for the variant listings.** `EnumerateLanguageVariantsByTypePagesAsync`, `…ByCollectionPagesAsync`, `…BySpacePagesAsync`, and `…OfContentTypeWithComponentsPagesAsync` stream the most unbounded listings (they scale as items × languages) one page at a time, like the existing asset/item streams.
- **`AsFailure<T>()` is public.** The primitive the SDK's own composite helpers use to propagate the first failure onto a different result type is now available for your own multi-step helpers (upload → create → link) — it preserves the error, status code, and request URL, and throws if misused on a successful result.
- **Element default values in one expression.** Each default-value model gained a convenience constructor: `DefaultValue = new TextElementDefaultValueModel("Untitled")` instead of the three-level `{ Global = new() { Value = … } }` initializer (which still works).

## Improvements

- **Less required ceremony.** Workflow step role lists (`RoleIds`, `CreateNewVersionRoleIds`, `UnpublishRoleIds`) default to empty — "no role restriction" no longer needs four explicit empty lists (the wire payload is unchanged; empty arrays still always serialize). `CollectionReplacePatchModel.PropertyName` defaults to `Name`, its only valid value today.
- **`Retry-After` honored everywhere.** The default resilience pipeline now respects a server-provided `Retry-After` delay on any retried response (e.g. `503`), not just `429`.
- **Content-model snapshot is explicitly experimental.** `ExportContentModelAsync` / `ContentModelSnapshot` now carry `[Experimental("KAIM001")]` — the feature and its JSON format are not yet a supported contract; suppress the diagnostic to opt in.
- **Doc samples teach the intended patterns.** The repository's code samples now use `EnsureSuccess()` instead of unchecked `.Value` unwrapping, the identifier factories, the patch facades instead of raw path strings, the one-call asset upload extensions, and modern collection expressions throughout.

## API refinements (breaking vs `9.0.0-beta-2`)

Beta-to-beta breaking changes — the compiler surfaces each:

- **All model collection properties are `IReadOnlyList<T>`.** The remaining `IEnumerable<T>` properties (69 declarations) converged on the convention the rest of the SDK already used, and `RichTextElementMetadataModel`'s six `ISet<…>` restriction properties did the same — so collection expressions now work everywhere: `AllowedBlocks = [RichTextBlockType.Text, RichTextBlockType.Images]`. If you assigned a deferred LINQ query directly, materialize it (`[.. items.Select(…)]`) — which also removes a subtle re-enumeration risk when retries re-serialize a request body.
- **`UpdateUserRolesAsync` takes a dedicated request model.** The body is now `UserRolesUpdateModel { CollectionGroups = … }` — previously it took the response-shaped `UserModel`, forcing a fake required `user_id` into a body the API identifies via the URL.
- **`UpsertLanguageVariantAsync(identifier, LanguageVariantModel)` moved to the extensions tier** (`Kontent.Ai.Management.Extensions`), joining its `UpsertContentItemAsync(Reference, ContentItemModel)` twin: the interface carries one method per API operation; fetched-model adapters are extensions. Call sites keep the same syntax — add the `using` if you don't have it.
- **Naming aligned, wire format untouched:**
  - Identifier-pair properties unified to the bare style: `WorkflowReference` / `WorkflowStepReference(s)` → `Workflow` / `Step(s)` (webhook workflow-transition triggers, variant filter), `TaxonomyReference` / `TermReferences` → `TaxonomyGroup` / `Terms`, `SnippetIdentifier` → `Snippet` (snippet element, URL-slug dependency).
  - `WorkflowStepColorModel` → `WorkflowStepColor` (an enum, not a model) and `RoleModel` → `UserRoleModel` (matching `EnvironmentRoleModel` / `SubscriptionUserRoleModel`).
  - Default-value models unified to `{Kind}ElementDefaultValueModel`, including `DateElementDefaultValueModel` → `DateTimeElementDefaultValueModel` to match `DateTimeElementMetadataModel`.
- **Asset-folder ids are `Guid`.** `AssetFolder.Id`, `AssetFolderHierarchy.Id`, and `AssetFolderLinkingHierarchy.Id` were `string`s holding GUIDs; they're typed `Guid` now (the zero GUID keeps meaning "outside any folder"), and the `AssetExtensions` folder-lookup helpers take `Guid` accordingly.

## Installation

```bash
dotnet add package Kontent.Ai.Management --prerelease
```

## Requirements

- .NET 8.0
- A Kontent.ai environment with Management API v2 access (a Management API key)

using Kontent.Ai.Management.Models.ContentModel.Patch;

namespace Kontent.Ai.Management.Models.TypeSnippets.Patch;

/// <summary>
/// Intent-revealing factories for content-type-snippet PATCH operations, ready to pass to
/// <c>ModifyContentTypeSnippetAsync</c>. Inherits every operation from <see cref="ContentModelPatchBase"/>; snippets
/// expose no content-group operations (they have no content groups, and their elements cannot reference one). The
/// server also rejects adding <c>url_slug</c>, <c>subpages</c>, or nested snippet elements into a snippet.
/// </summary>
public sealed class ContentTypeSnippetPatch : ContentModelPatchBase
{
    private ContentTypeSnippetPatch() { }
}

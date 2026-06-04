using Kontent.Ai.Management.Models.TaxonomyGroups;
using Kontent.Ai.Management.Models.Types;
using Kontent.Ai.Management.Models.TypeSnippets;

namespace Kontent.Ai.Management.Models.ContentModel;

/// <summary>
/// A complete, serializable snapshot of an environment's content model — its content types, snippets and taxonomy
/// groups. Produced by <see cref="Extensions.ContentModelExtensions.ExportContentModelAsync"/> as the input for
/// model generation and for committing as a content-model changelog.
/// </summary>
public sealed record ContentModelSnapshot
{
    /// <summary>All content types in the environment, ordered by codename.</summary>
    public required IReadOnlyList<ContentTypeModel> Types { get; init; }

    /// <summary>All content type snippets in the environment, ordered by codename.</summary>
    public required IReadOnlyList<ContentTypeSnippetModel> Snippets { get; init; }

    /// <summary>All taxonomy groups in the environment, ordered by codename.</summary>
    public required IReadOnlyList<TaxonomyGroupModel> Taxonomies { get; init; }
}

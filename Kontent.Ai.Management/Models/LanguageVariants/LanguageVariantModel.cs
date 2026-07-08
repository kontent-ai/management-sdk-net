using Kontent.Ai.Management.Models.LanguageVariants.Elements;
using Kontent.Ai.Management.Models.Publishing;
using Kontent.Ai.Management.Models.Workflow;

namespace Kontent.Ai.Management.Models.LanguageVariants;

/// <summary>
/// Everything about a language variant except its element values — the metadata shared by the untyped
/// <see cref="LanguageVariantModel"/> and the strongly-typed <see cref="LanguageVariantModel{TModel}"/>.
/// </summary>
public abstract record LanguageVariantMetadata
{
    /// <summary>
    /// Reference to the content item this variant belongs to.
    /// </summary>
    [JsonPropertyName("item")]
    public required Reference Item { get; init; }

    /// <summary>
    /// Reference to the language of this variant.
    /// </summary>
    [JsonPropertyName("language")]
    public required Reference Language { get; init; }

    /// <summary>
    /// ISO-8601 timestamp of the last change to the variant.
    /// </summary>
    [JsonPropertyName("last_modified")]
    public required DateTime LastModified { get; init; }

    /// <summary>
    /// Publishing and unpublishing schedule. The wrapper is always present; the individual timestamps may be null.
    /// </summary>
    [JsonPropertyName("schedule")]
    public required ScheduleResponseModel Schedule { get; init; }

    /// <summary>
    /// Workflow and step the variant currently sits in.
    /// </summary>
    [JsonPropertyName("workflow")]
    public required WorkflowStepIdentifier Workflow { get; init; }

    /// <summary>
    /// Due date. The wrapper is always present; its value may be null when no due date is set.
    /// </summary>
    [JsonPropertyName("due_date")]
    public required DueDateModel DueDate { get; init; }

    /// <summary>
    /// Free-form note attached to the variant.
    /// </summary>
    [JsonPropertyName("note")]
    public string? Note { get; init; }

    /// <summary>
    /// Users assigned as contributors. May be empty.
    /// </summary>
    [JsonPropertyName("contributors")]
    public required IReadOnlyList<UserIdentifier> Contributors { get; init; }
}

/// <summary>
/// A language variant of a content item (response shape), with untyped element values.
/// </summary>
public sealed record LanguageVariantModel : LanguageVariantMetadata
{
    /// <summary>
    /// Element values. Deserialized as <see cref="DynamicElement"/> — the wire carries no element-kind
    /// discriminator, so the value payload stays untyped while the envelope (element reference, sibling fields) is modeled.
    /// </summary>
    [JsonPropertyName("elements")]
    public required IReadOnlyList<BaseElement> Elements { get; init; }
}

/// <summary>
/// A language variant with strongly-typed elements — the typed counterpart of <see cref="LanguageVariantModel"/>.
/// Returned by the generic <c>GetLanguageVariantAsync&lt;T&gt;</c> / <c>UpsertLanguageVariantAsync&lt;T&gt;</c>: the variant's
/// element values are projected onto <typeparamref name="TModel"/>, while the metadata is preserved rather than discarded.
/// A client-side projection — not deserialized from the wire.
/// </summary>
/// <typeparam name="TModel">The generated content-type record modeling this variant's elements.</typeparam>
public sealed record LanguageVariantModel<TModel> : LanguageVariantMetadata where TModel : IElementsModel
{
    /// <summary>The strongly-typed element values.</summary>
    public required TModel Elements { get; init; }
}

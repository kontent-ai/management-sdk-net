using Kontent.Ai.Management.Models.LanguageVariants.Elements;

namespace Kontent.Ai.Management.Modules.ModelBuilders;

// TODO: revisit before this surface stabilizes. The typed element value shapes here overlap the modern Models/Content
// value wrappers (DateTimeValue / UrlSlugValue / CustomValue / RichTextElement) and the per-kind wire logic in
// ContentItemEnvelopeConverter — per-element wire shapes are now defined in more than one place. Decide whether this
// builder tier should share a single source of truth for the wire with the converter rather than re-declaring it.

/// <summary>
/// Builds the loosely-typed element collection a language-variant upsert expects from typed
/// <see cref="BaseElement"/> values — the typed-without-codegen way to author a variant's elements, sitting between the
/// raw anonymous payload and a fully generated content-type record.
/// </summary>
public static class ElementBuilder
{
    /// <summary>
    /// Returns the given typed elements as the element collection expected by
    /// <see cref="Models.LanguageVariants.LanguageVariantUpsertModel.Elements"/>. Each element serializes by its runtime
    /// type through the client's configured options.
    /// </summary>
    /// <param name="elements">The typed elements to author.</param>
    public static IEnumerable<object> GetElements(params BaseElement[] elements)
    {
        ArgumentNullException.ThrowIfNull(elements);
        return elements;
    }
}

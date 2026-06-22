namespace Kontent.Ai.Management.Models.Content;

/// <summary>
/// Value of a custom element on a generated content-type record: the opaque payload plus the plaintext companion
/// the CMS uses for search and filtering. The envelope converter flattens this to a <c>value</c> + sibling
/// <c>searchable_value</c> on the wire.
/// </summary>
/// <remarks>Use this to set a custom element on a generated content-type record. To set the same element by hand in the untyped element array instead, use <see cref="LanguageVariants.Elements.CustomElement"/>.</remarks>
public sealed record CustomValue
{
    /// <summary>The opaque element value, as produced by the custom element; often a JSON string.</summary>
    [JsonPropertyName("value")]
    public string? Value { get; init; }

    /// <summary>Plaintext companion used for search and filtering; null when unset.</summary>
    [JsonPropertyName("searchable_value")]
    public string? SearchableValue { get; init; }

    /// <summary>Keeps the common "just a value" authoring path a one-liner.</summary>
    public static implicit operator CustomValue(string value) => new() { Value = value };
}

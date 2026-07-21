namespace Kontent.Ai.Management.Models.Types.Elements.DefaultValues;

/// <summary>
/// Base shape for an element's default value. Subclasses specialize <typeparamref name="TValue"/> for the element kind.
/// </summary>
public abstract record ElementDefaultValue<TValue>
{
    /// <summary>
    /// Non-language-specific default. Required when the caller configures any default on the element.
    /// </summary>
    [JsonPropertyName("global")]
    public required ElementDefaultValueEnvelope<TValue> Global { get; init; }
}

/// <summary>
/// Wraps a single element default value. The wire format is <c>{ "value": ... }</c>.
/// </summary>
public sealed record ElementDefaultValueEnvelope<TValue>
{
    /// <summary>
    /// The default value carried by this container. The API rejects null and empty-array values — leave the parent default-value object null to express "no default configured".
    /// </summary>
    [JsonPropertyName("value")]
    public required TValue Value { get; init; }
}

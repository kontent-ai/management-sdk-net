using Newtonsoft.Json;

namespace Kontent.Ai.Management.Models.Types.Elements.DefaultValues;

/// <summary>
/// Base class for the element's default value definition
/// </summary>
public record ElementDefaultValue<TContainer, TValue> where TContainer : TypeValue<TValue>, new()
{
    /// <summary>
    /// Non-language specific default value
    /// </summary>
    [JsonProperty("global")]
    public TContainer Global { get; init; } = new();
}

/// <summary>
/// Container for the element's default value
/// </summary>
/// <typeparam name="TValue"></typeparam>
public record TypeValue<TValue>
{
    /// <summary>
    /// Default value
    /// </summary>
    [JsonProperty("value")]
    public TValue Value { get; init; }
}
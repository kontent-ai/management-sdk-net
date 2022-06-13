using Newtonsoft.Json;

namespace Kentico.Kontent.Management.Models.Types.Elements;

/// <summary>
/// Base class for the element's default value definition
/// </summary>
public class ElementDefaultValue<TContainer, TValue> where TContainer : TypeValue<TValue>, new()
{
    /// <summary>
    /// Non-language specific default value
    /// </summary>
    [JsonProperty("global")]
    public TContainer Global { get; set; } = new();
}

/// <summary>
/// Container for the element's default value
/// </summary>
/// <typeparam name="TValue"></typeparam>
public class TypeValue<TValue>
{
    /// <summary>
    /// Default value
    /// </summary>
    [JsonProperty("value")]
    public TValue Value { get; set; }
}
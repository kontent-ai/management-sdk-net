using Kontent.Ai.Management.Models.Shared;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Kontent.Ai.Management.Models.LanguageVariants.Elements;

/// <summary>
/// Represents the strongly typed assets element.
/// </summary>
public class AssetElement : BaseElement
{
    /// <summary>
    /// Gets or sets the value of the asset element.
    /// </summary>
    [JsonProperty("value")]
    public IEnumerable<AssetWithRenditionsReference> Value { get; set; }

    /// <summary>
    /// Transforms the asset element to the dynamic object.
    /// </summary>
    public override dynamic ToDynamic() => new {
        element = Element.ToDynamic(),
        value = Value.Select(v => v.ToDynamic())
    };
}

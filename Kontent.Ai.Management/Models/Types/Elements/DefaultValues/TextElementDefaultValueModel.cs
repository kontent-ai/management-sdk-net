using System.Diagnostics.CodeAnalysis;

namespace Kontent.Ai.Management.Models.Types.Elements.DefaultValues;

/// <summary>
/// Default value for the text element.
/// </summary>
public sealed record TextElementDefaultValueModel : ElementDefaultValue<string>
{
    /// <summary>Creates an empty instance for object-initializer construction.</summary>
    public TextElementDefaultValueModel() { }

    /// <summary>Creates a default of <paramref name="value"/>.</summary>
    [SetsRequiredMembers]
    public TextElementDefaultValueModel(string value) => Global = new() { Value = value };
}

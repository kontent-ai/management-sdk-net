using System.Diagnostics.CodeAnalysis;

namespace Kontent.Ai.Management.Models.Types.Elements.DefaultValues;

/// <summary>
/// Default value for the number element.
/// </summary>
public sealed record NumberElementDefaultValueModel : ElementDefaultValue<decimal>
{
    /// <summary>Creates an empty instance for object-initializer construction.</summary>
    public NumberElementDefaultValueModel() { }

    /// <summary>Creates a default of <paramref name="value"/>.</summary>
    [SetsRequiredMembers]
    public NumberElementDefaultValueModel(decimal value) => Global = new() { Value = value };
}

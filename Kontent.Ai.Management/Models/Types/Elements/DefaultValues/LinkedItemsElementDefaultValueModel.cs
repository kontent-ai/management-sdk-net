using System.Diagnostics.CodeAnalysis;

namespace Kontent.Ai.Management.Models.Types.Elements.DefaultValues;

/// <summary>
/// Default value for the linked items element.
/// </summary>
public sealed record LinkedItemsElementDefaultValueModel : ElementDefaultValue<IReadOnlyList<Reference>>
{
    /// <summary>Creates an empty instance for object-initializer construction.</summary>
    public LinkedItemsElementDefaultValueModel() { }

    /// <summary>Creates a default of the given linked items.</summary>
    [SetsRequiredMembers]
    public LinkedItemsElementDefaultValueModel(params Reference[] values) => Global = new() { Value = values };
}

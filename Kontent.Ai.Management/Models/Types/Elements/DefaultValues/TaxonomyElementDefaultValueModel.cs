using System.Diagnostics.CodeAnalysis;

namespace Kontent.Ai.Management.Models.Types.Elements.DefaultValues;

/// <summary>
/// Default value for the taxonomy element.
/// </summary>
public sealed record TaxonomyElementDefaultValueModel : ElementDefaultValue<IReadOnlyList<Reference>>
{
    /// <summary>Creates an empty instance for object-initializer construction.</summary>
    public TaxonomyElementDefaultValueModel() { }

    /// <summary>Creates a default of the given terms.</summary>
    [SetsRequiredMembers]
    public TaxonomyElementDefaultValueModel(params Reference[] values) => Global = new() { Value = values };
}

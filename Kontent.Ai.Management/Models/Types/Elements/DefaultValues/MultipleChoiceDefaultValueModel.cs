using System.Diagnostics.CodeAnalysis;

namespace Kontent.Ai.Management.Models.Types.Elements.DefaultValues;

/// <summary>
/// Default value for the multiple-choice element.
/// </summary>
public sealed record MultipleChoiceDefaultValueModel : ElementDefaultValue<IEnumerable<Reference>>
{
    /// <summary>Creates an empty instance for object-initializer construction.</summary>
    public MultipleChoiceDefaultValueModel() { }

    /// <summary>Creates a default of the given options.</summary>
    [SetsRequiredMembers]
    public MultipleChoiceDefaultValueModel(params Reference[] values) => Global = new() { Value = values };
}

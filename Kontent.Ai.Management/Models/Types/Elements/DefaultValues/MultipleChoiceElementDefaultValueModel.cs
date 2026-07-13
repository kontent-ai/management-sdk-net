using System.Diagnostics.CodeAnalysis;

namespace Kontent.Ai.Management.Models.Types.Elements.DefaultValues;

/// <summary>
/// Default value for the multiple-choice element.
/// </summary>
public sealed record MultipleChoiceElementDefaultValueModel : ElementDefaultValue<IReadOnlyList<Reference>>
{
    /// <summary>Creates an empty instance for object-initializer construction.</summary>
    public MultipleChoiceElementDefaultValueModel() { }

    /// <summary>Creates a default of the given options.</summary>
    [SetsRequiredMembers]
    public MultipleChoiceElementDefaultValueModel(params Reference[] values) => Global = new() { Value = values };
}

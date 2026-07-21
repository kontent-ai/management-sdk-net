using System.Diagnostics.CodeAnalysis;

namespace Kontent.Ai.Management.Models.Types.Elements.DefaultValues;

/// <summary>
/// Default value for the date &amp; time element.
/// </summary>
public sealed record DateTimeElementDefaultValueModel : ElementDefaultValue<DateTimeOffset>
{
    /// <summary>Creates an empty instance for object-initializer construction.</summary>
    public DateTimeElementDefaultValueModel() { }

    /// <summary>Creates a default of <paramref name="value"/>.</summary>
    [SetsRequiredMembers]
    public DateTimeElementDefaultValueModel(DateTimeOffset value) => Global = new() { Value = value };
}

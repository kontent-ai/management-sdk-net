using Kontent.Ai.Management.Models.Content;

using System.Diagnostics.CodeAnalysis;

namespace Kontent.Ai.Management.Models.Types.Elements.DefaultValues;

/// <summary>
/// Default value for the asset element.
/// </summary>
public sealed record AssetDefaultValueModel : ElementDefaultValue<IEnumerable<AssetReference>>
{
    /// <summary>Creates an empty instance for object-initializer construction.</summary>
    public AssetDefaultValueModel() { }

    /// <summary>Creates a default of the given assets.</summary>
    [SetsRequiredMembers]
    public AssetDefaultValueModel(params AssetReference[] values) => Global = new() { Value = values };
}

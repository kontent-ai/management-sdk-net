using Kontent.Ai.Management.Models.Content;

using System.Diagnostics.CodeAnalysis;

namespace Kontent.Ai.Management.Models.Types.Elements.DefaultValues;

/// <summary>
/// Default value for the asset element.
/// </summary>
public sealed record AssetElementDefaultValueModel : ElementDefaultValue<IReadOnlyList<AssetReference>>
{
    /// <summary>Creates an empty instance for object-initializer construction.</summary>
    public AssetElementDefaultValueModel() { }

    /// <summary>Creates a default of the given assets.</summary>
    [SetsRequiredMembers]
    public AssetElementDefaultValueModel(params AssetReference[] values) => Global = new() { Value = values };
}

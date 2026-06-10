using Kontent.Ai.Management.Models.Content;

namespace Kontent.Ai.Management.Models.Types.Elements.DefaultValues;

/// <summary>
/// Default value for the asset element.
/// </summary>
public sealed record AssetDefaultValueModel : ElementDefaultValue<IEnumerable<AssetReference>> { }

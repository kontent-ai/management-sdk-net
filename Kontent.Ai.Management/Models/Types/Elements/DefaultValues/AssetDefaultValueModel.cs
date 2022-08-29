using Kontent.Ai.Management.Models.Shared;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.Types.Elements.DefaultValues;

/// <summary>
/// The default value model for the asset element
/// </summary>
public class AssetDefaultValueModel : ElementDefaultValue<TypeValue<IEnumerable<AssetWithRenditionsReference>>, IEnumerable<AssetWithRenditionsReference>> { }

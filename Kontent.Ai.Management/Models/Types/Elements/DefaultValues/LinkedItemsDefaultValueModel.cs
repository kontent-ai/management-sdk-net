using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.Types.Elements.DefaultValues;

/// <summary>
/// The default value model for the linked item element
/// </summary>
public sealed record LinkedItemsDefaultValueModel : ElementDefaultValue<TypeValue<IEnumerable<Reference>>, IEnumerable<Reference>> { }

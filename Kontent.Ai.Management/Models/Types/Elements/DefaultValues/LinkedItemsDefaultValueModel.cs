using Kontent.Ai.Management.Models.Shared;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.Types.Elements.DefaultValues;

/// <summary>
/// The default value model for the linked item element
/// </summary>
public class LinkedItemsDefaultValueModel: ElementDefaultValue<TypeValue<IEnumerable<Reference>>, IEnumerable<Reference>> { }

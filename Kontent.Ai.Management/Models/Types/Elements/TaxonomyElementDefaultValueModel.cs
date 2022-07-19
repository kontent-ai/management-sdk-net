using Kontent.Ai.Management.Models.Shared;
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.Types.Elements;

/// <summary>
/// The default value model for the taxonomy element
/// </summary>
public class TaxonomyElementDefaultValueModel : ElementDefaultValue<TypeValue<List<Reference>>, List<Reference>> { }
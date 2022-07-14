using Kentico.Kontent.Management.Models.Shared;
using System.Collections.Generic;

namespace Kentico.Kontent.Management.Models.Types.Elements;

/// <summary>
/// The default value model for the taxonomy element
/// </summary>
public class TaxonomyElementDefaultValueModel : ElementDefaultValue<TypeValue<List<Reference>>, List<Reference>> { }
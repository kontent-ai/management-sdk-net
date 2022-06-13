using Kentico.Kontent.Management.Models.Shared;
using System.Collections.Generic;

namespace Kentico.Kontent.Management.Models.Types.Elements;

/// <summary>
/// The default value model for the multiple choice element
/// </summary>
public class MultipleChoiceDefaultValueModel : ElementDefaultValue<TypeValue<List<Reference>>, List<Reference>> { }
using System.Collections.Generic;

namespace Kontent.Ai.Management.Models.Types.Elements.DefaultValues;

/// <summary>
/// The default value model for the multiple choice element
/// </summary>
public sealed record MultipleChoiceDefaultValueModel : ElementDefaultValue<TypeValue<List<Reference>>, List<Reference>> { }
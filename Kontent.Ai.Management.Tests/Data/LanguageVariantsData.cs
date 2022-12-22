using Kontent.Ai.Management.Models.LanguageVariants;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.StronglyTyped;
using Kontent.Ai.Management.Models.Workflow;
using System;

namespace Kontent.Ai.Management.Tests.Data;

/// <summary>
/// Represents language variants with dynamic and strongly typed objects of element data located in './ElementsData/ElementsData.json'.
/// </summary>
internal class LanguageVariantsData
{
    public static LanguageVariantModel GetExpectedLanguageVariantModel(
        string languageId = "78dbefe8-831b-457e-9352-f4c4eacd5024",
        string itemId = "4b628214-e4fe-4fe0-b1ff-955df33e1515") => new()
        {
            Item = Reference.ById(Guid.Parse(itemId)),
            Language = Reference.ById(Guid.Parse(languageId)),
            LastModified = DateTimeOffset.Parse("2021-11-06T13:57:26.7069564Z").UtcDateTime,
            Workflow = new WorkflowStepIdentifier(Reference.ById(Guid.Parse("00000000-0000-0000-0000-000000000000")), Reference.ById(Guid.Parse("eee6db3b-545a-4785-8e86-e3772c8756f9"))),
            Elements = ElementsData.GetExpectedDynamicElements(),
        };

    public static LanguageVariantModel<ComplexTestModel> GetExpectedComplexTestModel(string languageId = "78dbefe8-831b-457e-9352-f4c4eacd5024") => new()
    {
        Item = Reference.ById(Guid.Parse("4b628214-e4fe-4fe0-b1ff-955df33e1515")),
        Language = Reference.ById(Guid.Parse(languageId)),
        LastModified = DateTimeOffset.Parse("2021-11-06T13:57:26.7069564Z").UtcDateTime,
        Workflow = new WorkflowStepIdentifier(Reference.ById(Guid.Parse("00000000-0000-0000-0000-000000000000")), Reference.ById(Guid.Parse("eee6db3b-545a-4785-8e86-e3772c8756f9"))),
        Elements = ElementsData.GetExpectedStronglyTypedElementsModel(),
    };
}

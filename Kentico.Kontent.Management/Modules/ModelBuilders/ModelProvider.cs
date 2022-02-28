using Kentico.Kontent.Management.Models.LanguageVariants;
using Kentico.Kontent.Management.Models.StronglyTyped;

namespace Kentico.Kontent.Management.Modules.ModelBuilders
{
    internal class ModelProvider : IModelProvider
    {
        private readonly IElementModelProvider _elementModelProvider;

        internal ModelProvider()
        {
            _elementModelProvider = new ElementModelProvider();
        }

        public LanguageVariantModel<T> GetLanguageVariantModel<T>(LanguageVariantModel variant) where T : new()
        {
            var result = new LanguageVariantModel<T>
            {
                Item = variant.Item,
                Language = variant.Language,
                LastModified = variant.LastModified,
                WorkflowStep = variant.WorkflowStep,
                Elements = _elementModelProvider.GetStronglyTypedElements<T>(variant.Elements)
            };

            return result;
        }

        public LanguageVariantUpsertModel GetLanguageVariantUpsertModel<T>(T variantElements) where T : new()
        {
            return new LanguageVariantUpsertModel
            {
                Elements = _elementModelProvider.GetDynamicElements(variantElements)
            };
        }
    }
}

using Kentico.Kontent.Management.Models.Items;
using Kentico.Kontent.Management.Models.StronglyTyped;

namespace Kentico.Kontent.Management.Modules.ModelBuilders
{
    /// <summary>
    /// Defines the contract for mapping content items to strongly typed models.
    /// </summary>
    public interface IModelProvider
    {
        /// <summary>
        /// Ensures mapping between Kentico Kontent content item fields and model properties.
        /// </summary>
        IPropertyMapper PropertyMapper { get; set; }

        /// <summary>
        /// Builds a strongly typed model from non-generic model.
        /// </summary>
        /// <typeparam name="T">Strongly typed content item model.</typeparam>
        /// <param name="variant">Content item variant data.</param>
        /// <returns>Strongly typed model of the generic type.</returns>
        ContentItemVariantModel<T> GetContentItemVariantModel<T>(ContentItemVariantModel variant) where T : new();

        /// <summary>
        /// Converts generic upsert model to non-generic model.
        /// </summary>
        /// <typeparam name="T">Strongly typed content item model.</typeparam>
        /// <param name="variantElements">Strongly typed content item variant data.</param>
        /// <returns>Non-generic model.</returns>
        ContentItemVariantUpsertModel GetContentItemVariantUpsertModel<T>(T variantElements) where T : new();
    }
}
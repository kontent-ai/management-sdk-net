using KenticoCloud.ContentManagement.Models.Items;
using KenticoCloud.ContentManagement.Models.StronglyTyped;

namespace KenticoCloud.ContentManagement.Modules.ModelBuilders
{
    /// <summary>
    /// Defines the contract for mapping content items to strongly typed models.
    /// </summary>
    public interface IModelProvider
    {
        /// <summary>
        /// Ensures mapping between Kentico Cloud content item fields and model properties.
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
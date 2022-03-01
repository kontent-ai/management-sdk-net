using Kentico.Kontent.Management.Models.Assets;
using Kentico.Kontent.Management.Models.LanguageVariants;
using Kentico.Kontent.Management.Models.StronglyTyped;

namespace Kentico.Kontent.Management.Modules.ModelBuilders
{
    /// <summary>
    /// Defines the contract for mapping content items and assets to strongly typed models.
    /// </summary>
    public interface IModelProvider
    {
        /// <summary>
        /// Builds a strongly typed language variant model from non-generic model.
        /// </summary>
        /// <typeparam name="T">Strongly typed elements model.</typeparam>
        /// <param name="variant">Language variant data.</param>
        /// <returns>Strongly typed language variant model of the generic type.</returns>
        LanguageVariantModel<T> GetLanguageVariantModel<T>(LanguageVariantModel variant) where T : new();

        /// <summary>
        /// Converts generic language variant upsert model to non-generic model.
        /// </summary>
        /// <typeparam name="T">Strongly typed elements model.</typeparam>
        /// <param name="variantElements">Strongly typed language variant elements data.</param>
        /// <returns>Non-generic language variant model.</returns>
        LanguageVariantUpsertModel GetLanguageVariantUpsertModel<T>(T variantElements) where T : new();

        /// <summary>
        /// Builds a strongly typed asset model from non-generic model.
        /// </summary>
        /// <typeparam name="T">Asset data</typeparam>
        /// <param name="asset">Strongly typed elements model.</param>
        /// <returns>Strongly typed asset model of the generic type.</returns>
        AssetModel<T> GetAssetModel<T>(AssetModel asset) where T : new();
        
        /// <summary>
        /// Converts generic asset create model to non-generic model.
        /// </summary>
        /// <typeparam name="T">Strongly typed elements model.</typeparam>
        /// <param name="asset">Strongly typed asset create model with elements data.</param>
        /// <returns>Non-generic asset create model.</returns>
        AssetCreateModel GetAssetCreateModel<T>(AssetCreateModel<T> asset) where T : new();

        /// <summary>
        /// Converts generic asset update model to non-generic model.
        /// </summary>
        /// <typeparam name="T">Strongly typed elements model.</typeparam>
        /// <param name="asset">Strongly typed asset update model with elements data.</param>
        /// <returns>Non-generic asset update model.</returns>
        AssetUpdateModel GetAssetUpdateModel<T>(AssetUpdateModel<T> asset) where T : new();
        
        /// <summary>
        /// Converts generic asset upsert model to non-generic model.
        /// </summary>
        /// <typeparam name="T">Strongly typed elements model.</typeparam>
        /// <param name="asset">Strongly typed asset upsert model with elements data.</param>
        /// <returns>Non-generic asset upsert model.</returns>
        AssetUpsertModel GetAssetUpsertModel<T>(AssetUpsertModel<T> asset) where T : new();
    }
}
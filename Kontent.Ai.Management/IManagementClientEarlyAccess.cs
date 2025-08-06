using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.VariantFilter;
using System.Threading.Tasks;

namespace Kontent.Ai.Management;

/// <summary>
/// Represents a set of early access Content Management API requests.
/// These features are experimental and may change or be removed in future versions.
/// </summary>
public interface IManagementClientEarlyAccess
{
    /// <summary>
    /// Returns listing of filtered variants.
    /// This is an early access feature that may change or be removed in future versions.
    /// </summary>
    /// <param name="variantFilterRequest">The variant filter request containing filters and ordering options.</param>
    /// <returns>The <see cref="IListingResponseModel{VariantFilterItemModel}"/> instance representing the filtered variants.</returns>
    Task<IListingResponseModel<VariantFilterItemModel>> FilterVariantsAsync(VariantFilterRequestModel variantFilterRequest);
}
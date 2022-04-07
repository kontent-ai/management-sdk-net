using System.Collections.Generic;

namespace Kentico.Kontent.Management.Models.Shared;

internal interface IListingResponse<T> : IEnumerable<T>
{
    PaginationResponseModel Pagination { get; set; }
}

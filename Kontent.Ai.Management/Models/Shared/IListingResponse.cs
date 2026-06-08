namespace Kontent.Ai.Management.Models.Shared;

// T is a marker the ListingResponse converter reads to discover the item type; the response is never enumerated directly.
internal interface IListingResponse<T>
{
    PaginationResponseModel Pagination { get; init; }
}

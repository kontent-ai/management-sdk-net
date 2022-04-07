using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kentico.Kontent.Management.Models.Shared;

internal class ListingResponseModel<T> : IListingResponseModel<T>
{
    private readonly IEnumerable<T> _result;

    private readonly string _continuationToken;
    private readonly string _url;
    private readonly Func<string, string, Task<IListingResponse<T>>> _nextPageRetriever;

    public ListingResponseModel(Func<string, string, Task<IListingResponse<T>>> retriever, string continuationToken, string url, IEnumerable<T> result)
    {
        _nextPageRetriever = retriever;
        _continuationToken = continuationToken;
        _url = url;
        _result = result;
    }

    public async Task<IListingResponseModel<T>> GetNextPage()
    {
        if (_continuationToken == null)
        {
            throw new InvalidOperationException("Next page not available.");
        }

        var nextPage = await _nextPageRetriever(_continuationToken, _url);
        return new ListingResponseModel<T>(_nextPageRetriever, nextPage.Pagination?.Token, _url, nextPage);
    }

    public bool HasNextPage() => 
        _continuationToken != null;

    IEnumerator IEnumerable.GetEnumerator() => 
        GetEnumerator();

    public IEnumerator<T> GetEnumerator() => 
        _result.GetEnumerator();
}

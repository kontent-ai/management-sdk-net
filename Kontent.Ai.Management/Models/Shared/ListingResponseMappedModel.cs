using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kontent.Ai.Management.Models.Shared;

internal class ListingResponseMappedModel<TRaw, T> : IListingResponseModel<T>
{
    private readonly IEnumerable<TRaw> _rawResult;

    private readonly string _continuationToken;
    private readonly string _url;
    private readonly Func<string, string, Task<IListingResponse<TRaw>>> _nextPageRetriever;
    private readonly Func<TRaw, T> _mapModel;

    public ListingResponseMappedModel(
        Func<string, string, Task<IListingResponse<TRaw>>> retriever,
        string continuationToken,
        string url,
        IEnumerable<TRaw> result,
        Func<TRaw, T> mapModel)
    {
        _nextPageRetriever = retriever;
        _continuationToken = continuationToken;
        _url = url;
        _rawResult = result;
        _mapModel = mapModel;
    }

    public async Task<IListingResponseModel<T>> GetNextPage()
    {
        if (_continuationToken == null)
        {
            throw new InvalidOperationException("Next page not available.");
        }

        var nextPage = await _nextPageRetriever(_continuationToken, _url);
        return new ListingResponseMappedModel<TRaw, T>(_nextPageRetriever, nextPage.Pagination?.Token, _url, nextPage, _mapModel);
    }

    public bool HasNextPage() =>
        _continuationToken != null;

    IEnumerator IEnumerable.GetEnumerator() =>
        GetEnumerator();

    public IEnumerator<T> GetEnumerator() =>
        _rawResult.Select(_mapModel).GetEnumerator();
}

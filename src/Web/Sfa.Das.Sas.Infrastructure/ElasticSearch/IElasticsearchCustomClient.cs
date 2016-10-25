using System;
using System.Runtime.CompilerServices;
using Nest;

namespace Sfa.Das.Sas.Infrastructure.Elasticsearch
{
    public interface IElasticsearchCustomClient
    {
        ISearchResponse<T> Search<T>(Func<SearchDescriptor<T>, ISearchRequest> selector, [CallerMemberName] string callerName = "")
            where T : class;

        ISearchResponse<T> Scroll<T>(string time, string scrollId)
            where T : class;
    }
}
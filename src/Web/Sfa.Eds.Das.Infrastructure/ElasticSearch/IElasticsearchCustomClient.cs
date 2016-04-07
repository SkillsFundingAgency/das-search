using System;
using Nest;

namespace Sfa.Eds.Das.Infrastructure.Elasticsearch
{
    public interface IElasticsearchCustomClient
    {
        ISearchResponse<T> Search<T>(Func<SearchDescriptor<T>, ISearchRequest> selector)
            where T : class;
    }
}
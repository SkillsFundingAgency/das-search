using System;
using Nest;

namespace Sfa.Eds.Das.Infrastructure.Elasticsearch
{
    using System.Runtime.CompilerServices;

    public interface IElasticsearchCustomClient
    {
        ISearchResponse<T> Search<T>(Func<SearchDescriptor<T>, ISearchRequest> selector, [CallerMemberName] string callerName = "")
            where T : class;
    }
}
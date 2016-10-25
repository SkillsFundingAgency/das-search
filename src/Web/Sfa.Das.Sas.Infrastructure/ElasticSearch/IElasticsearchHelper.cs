using System.Collections.Generic;

namespace Sfa.Das.Sas.Infrastructure.Elasticsearch
{
    public interface IElasticsearchHelper
    {
        List<T> GetAllDocumentsFromIndex<T>(string indexAlias, string type)
            where T : class;
    }
}
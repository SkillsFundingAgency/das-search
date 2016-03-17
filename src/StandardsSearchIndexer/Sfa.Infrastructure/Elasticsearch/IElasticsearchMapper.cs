namespace Sfa.Infrastructure.Elasticsearch
{
    using Sfa.Eds.Das.Indexer.Core.Models;
    using Sfa.Eds.Das.Indexer.Core.Models.Framework;

    public interface IElasticsearchMapper
    {
        StandardDocument CreateStandardDocument(MetaDataItem standard);

        FrameworkDocument CreateFrameworkDocument(FrameworkMetaData frameworkMetaData);
    }
}
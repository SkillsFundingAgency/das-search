using System;

namespace Sfa.Infrastructure.Elasticsearch
{
    using Eds.Das.Indexer.Core.Models.Provider;
    using Sfa.Eds.Das.Indexer.Core.Models;
    using Sfa.Eds.Das.Indexer.Core.Models.Framework;
    using Sfa.Infrastructure.Elasticsearch.Models;

    public interface IElasticsearchMapper
    {
        StandardDocument CreateStandardDocument(StandardMetaData standard);

        FrameworkDocument CreateFrameworkDocument(FrameworkMetaData frameworkMetaData);

        int MapLevelProgType(int level);
        
        StandardProvider CreateStandardProviderDocument(Provider provider, StandardInformation standardInformation, DeliveryInformation deliveryInformation);

        FrameworkProvider CreateFrameworkProviderDocument(Provider provider, FrameworkInformation standardInformation, DeliveryInformation deliveryInformation);
    }
}
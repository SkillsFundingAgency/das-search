using Sfa.Das.Sas.Indexer.Core.Models;
using Sfa.Das.Sas.Indexer.Core.Models.Framework;
using Sfa.Das.Sas.Indexer.Core.Models.Provider;
using Sfa.Das.Sas.Indexer.Infrastructure.Elasticsearch.Models;

namespace Sfa.Das.Sas.Indexer.Infrastructure.Elasticsearch
{
    using System.Collections.Generic;

    public interface IElasticsearchMapper
    {
        StandardDocument CreateStandardDocument(StandardMetaData standard);

        FrameworkDocument CreateFrameworkDocument(FrameworkMetaData frameworkMetaData);

        int MapToLevelFromProgType(int level);

        StandardProvider CreateStandardProviderDocument(Provider provider, StandardInformation standardInformation, IEnumerable<DeliveryInformation> deliveryInformation);

        StandardProvider CreateStandardProviderDocument(Provider provider, StandardInformation standardInformation, DeliveryInformation deliveryInformation);

        FrameworkProvider CreateFrameworkProviderDocument(Provider provider, FrameworkInformation standardInformation, IEnumerable<DeliveryInformation> deliveryInformation);

        FrameworkProvider CreateFrameworkProviderDocument(Provider provider, FrameworkInformation standardInformation, DeliveryInformation deliveryInformation);

        ProviderDocument CreateProviderDocument(Provider provider);
    }
}
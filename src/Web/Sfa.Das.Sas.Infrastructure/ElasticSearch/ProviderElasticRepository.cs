﻿using System;
using System.Collections.Generic;
using System.Linq;
using Nest;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Core.Configuration;
using Sfa.Das.Sas.Core.Domain.Model;
using Sfa.Das.Sas.Core.Domain.Services;
using Sfa.Das.Sas.Core.Logging;
using Sfa.Das.Sas.Infrastructure.Mapping;

namespace Sfa.Das.Sas.Infrastructure.Elasticsearch
{
    public sealed class ProviderElasticRepository : IGetProviders
    {
        private readonly IElasticsearchCustomClient _elasticsearchCustomClient;
        private readonly ILog _applicationLogger;
        private readonly IConfigurationSettings _applicationSettings;

        public ProviderElasticRepository(
            IElasticsearchCustomClient elasticsearchCustomClient,
            ILog applicationLogger,
            IConfigurationSettings applicationSettings)
        {
            _elasticsearchCustomClient = elasticsearchCustomClient;
            _applicationLogger = applicationLogger;
            _applicationSettings = applicationSettings;
        }

        public long GetProvidersAmount()
        {
            var results =
                   _elasticsearchCustomClient.Search<ProviderSearchResultItem>(
                       s =>
                       s.Index(_applicationSettings.ProviderIndexAlias)
                           .Type(Types.Parse("providerdocument"))
                           .From(0)
                           .MatchAll());
            return results.HitsMetaData.Total;
        }
    }
}
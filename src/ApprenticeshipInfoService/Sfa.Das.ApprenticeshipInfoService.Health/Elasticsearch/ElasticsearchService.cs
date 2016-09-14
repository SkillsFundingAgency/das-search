namespace Sfa.Das.ApprenticeshipInfoService.Health.Elasticsearch
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using global::Elasticsearch.Net;

    using Nest;

    using Core.Logging;
    using Models;

    public class ElasticsearchService : IElasticsearchService
    {
        private readonly IHealthSettings _healthSettings;

        private readonly ILog _logger;

        public ElasticsearchService(IHealthSettings healthSettings, ILog logger)
        {
            _healthSettings = healthSettings;
            _logger = logger;
        }

        public HealthModel GetHealth()
        {
            var messages = new List<string>();
            var d = GetElasticHealth(_healthSettings.ElasticsearchUrls, _healthSettings.Environment);
            var aliases = d.ElasticsearchAliases;
            if (d.Exception != null)
            {
                messages.Add(d.Exception.Message);
            }

            var model = new HealthModel
            {
                ElasticSearchAliases = aliases,
                Errors = messages,
                Status = messages.Any() ? Status.Error : Status.Ok,
                ElasticsearchLog = GetErrorLogs(_healthSettings.ElasticsearchUrls, _healthSettings.Environment)
            };

            return model;
        }

        private ElasticsearchLog GetErrorLogs(IEnumerable<Uri> uriStrings, string environment)
        {
            var connectionSettings = new ConnectionSettings(new StaticConnectionPool(uriStrings));
            connectionSettings.DisableDirectStreaming();
            var elasticClient = new ElasticClient(connectionSettings);

            var hej = DateMath.Now.Subtract(new Time(new TimeSpan(1, 0, 0, 0)));
            var result = elasticClient.Search<ElasticsearchLogHit>(
                s => s
                .Index("logstash-*")
                .Type(Types.All)
                .Query(q => q
                    .Bool(b => b
                        .Must(m => m
                            .Match(ma => ma
                                .Field("Application")
                                .Query("search-index"))
                            , bs2 => bs2
                            .Match(ma => ma
                                .Field(f => f.Level)
                                .Query("error"))
                             , bs2 => bs2
                             .Match(ma => ma
                                .Field("Environment")
                                .Query(environment))
                             , bs2 => bs2
                             .DateRange(r => r.GreaterThan(hej).Field("@timestamp")))
                      )
                  )
                );
            return new ElasticsearchLog
            {
                LogErrors = result.Hits.Select(m => m.Source.Message),
                ErrorCount = result.Total
            };
        }

        private ElasticsearchResponse GetElasticHealth(IEnumerable<Uri> uris, string environment)
        {
            try
            {
                var connectionSettings = new ConnectionSettings(new StaticConnectionPool(uris));
                var elasticClient = new ElasticClient(connectionSettings);
                var catAliases = elasticClient
                    .CatAliases()
                    .Records
                    .Where(m => m.Alias.StartsWith(environment));
                var aliases = new List<ElasticsearchAlias>();

                foreach (var catAlias in catAliases)
                {
                    var index = elasticClient.CatIndices(m => m.Index(catAlias.Index));

                    aliases.Add(new ElasticsearchAlias
                    {
                        AliasName = catAlias.Alias,
                        IndexName = catAlias.Index,
                        Status = index.Records.FirstOrDefault()?.Status,
                        DocumentCount = index.Records.FirstOrDefault()?.DocsCount,
                        Health = index.Records.FirstOrDefault()?.Health
                    });
                }
                return new ElasticsearchResponse { ElasticsearchAliases = aliases };
            }
            catch (Exception exception)
            {
                this._logger.Error(exception, "Not able to get aliases");

                return new ElasticsearchResponse { ElasticsearchAliases = new List<ElasticsearchAlias>(), Exception = exception };
            }
        }
    }
}
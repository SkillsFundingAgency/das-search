namespace Sfa.Das.ApprenticeshipInfoService.Health
{
    using System.Collections.Generic;
    using System.Diagnostics;

    using Elasticsearch;

    using Models;

    public class HealthService : IHealthService
    {
        private readonly IElasticsearchService _elasticsearchService;

        private readonly IHealthSettings _healthSettings;

        private readonly IHttpServer _httpServer;

        public HealthService(
            IElasticsearchService elasticsearchService, 
            IHealthSettings healthSettings,
            IHttpServer httpServer)
        {
            _elasticsearchService = elasticsearchService;
            _healthSettings = healthSettings;
            _httpServer = httpServer;
        }

        public HealthModel CreateModel()
        {
            var timer = Stopwatch.StartNew();
            var elasticserachModel = _elasticsearchService.GetElasticHealth(_healthSettings.ElasticsearchUrls, _healthSettings.Environment);
            var elasticErrorLogs = _elasticsearchService.GetErrorLogs(
                _healthSettings.ElasticsearchUrls,
                _healthSettings.Environment);

            var responseZipFile =_httpServer.ResponseCode(_healthSettings.LarsZipFileUrl);
            var courseDirectoryResponse = _httpServer.ResponseCode(_healthSettings.CourseDirectoryUrl);

            var model = new HealthModel
            {
                Status = Status.Ok,
                Errors = new List<string>(),
                ElasticSearchAliases = elasticserachModel.ElasticsearchAliases,
                ElasticsearchLog = elasticErrorLogs,
                LarsZipFileStatus = responseZipFile,
                CourseDirectoryStatus = courseDirectoryResponse 
            };

            if (elasticserachModel.Exception != null)
            {
                model.Status = Status.Error;
                model.Errors.Add(elasticserachModel.Exception.Message);
            }

            if (elasticserachModel.ElasticsearchAliases.Count < 2)
            {
                model.Status = Status.Error;
                model.Errors.Add($"Missing aliases / indices. Should be 2 but found {elasticserachModel.ElasticsearchAliases.Count}");
            }

            if (model.LarsZipFileStatus != Status.Ok)
            {
                model.Status = Status.Error;
                model.Errors.Add("Cant access hub.imservices.org.uk (LARS)");
            }

            if (model.CourseDirectoryStatus != Status.Ok)
            {
                model.Status = Status.Error;
                model.Errors.Add("Cant access Course Directory");
            }

            timer.Stop();
            model.Took = timer.ElapsedMilliseconds;

            return model;
        }
    }
}

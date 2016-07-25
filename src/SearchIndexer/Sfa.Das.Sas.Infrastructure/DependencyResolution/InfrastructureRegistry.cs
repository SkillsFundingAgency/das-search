using Nest;
using Sfa.Das.Sas.Indexer.ApplicationServices.Http;
using Sfa.Das.Sas.Indexer.ApplicationServices.Infrastructure;
using Sfa.Das.Sas.Indexer.ApplicationServices.MetaData;
using Sfa.Das.Sas.Indexer.ApplicationServices.Provider;
using Sfa.Das.Sas.Indexer.ApplicationServices.Queue;
using Sfa.Das.Sas.Indexer.ApplicationServices.Services;
using Sfa.Das.Sas.Indexer.Core.Logging;
using Sfa.Das.Sas.Indexer.Core.Services;
using Sfa.Das.Sas.Indexer.Infrastructure.Azure;
using Sfa.Das.Sas.Indexer.Infrastructure.CourseDirectory;
using Sfa.Das.Sas.Indexer.Infrastructure.Elasticsearch;
using Sfa.Das.Sas.Indexer.Infrastructure.Elasticsearch.Configuration;
using Sfa.Das.Sas.Indexer.Infrastructure.Services;
using Sfa.Das.Sas.Indexer.Infrastructure.Settings;
using StructureMap;

namespace Sfa.Das.Sas.Indexer.Infrastructure.DependencyResolution
{
    using Sfa.Das.Sas.Indexer.Infrastructure.DapperBD;

    public class InfrastructureRegistry : Registry
    {
        public InfrastructureRegistry()
        {
            For<IMessageQueueService>().Use<AzureCloudQueueService>();
            For<ILarsSettings>().Use<LarsSettings>();
            For<IElasticsearchConfiguration>().Use<ElasticsearchConfiguration>();
            For<IElasticsearchSettings>().Use<ElasticsearchSettings>();
            For<IGetActiveProviders>().Use<FcsActiveProvidersClient>();
            For<IConvertFromCsv>().Use<CsvService>();
            For<IVstsClient>().Use<VstsClient>();
            For<IHttpGetFile>().Use<HttpService>();
            For<IHttpGet>().Use<HttpService>();
            For<IHttpPost>().Use<HttpService>();
            For<IInfrastructureSettings>().Use<InfrastructureSettings>();
            For<ICourseDirectoryProviderDataService>().Use(x => new CourseDirectoryProviderDataService());
            For<ILog>().Use(x => new NLogService(x.ParentType, x.GetInstance<IInfrastructureSettings>())).AlwaysUnique();
            For<IUnzipStream>().Use<ZipFileExtractor>();
            For<IGetApprenticeshipProviders>().Use<CourseDirectoryClient>();
            For<IMaintainApprenticeshipIndex>().Use<ElasticsearchApprenticeshipIndexMaintainer>();
            For<IMaintainProviderIndex>().Use<ElasticsearchProviderIndexMaintainer>();
            For<IElasticsearchMapper>().Use<ElasticsearchMapper>();
            For<IElasticClient>().Use<ElasticClient>();
            For<IElasticsearchCustomClient>().Use<ElasticsearchCustomClient>();
            For<IDatabaseProvider>().Use<DatabaseProvider>();
            For<IAchievementRatesProvider>().Use<AchievementRatesProvider>();
        }
    }
}
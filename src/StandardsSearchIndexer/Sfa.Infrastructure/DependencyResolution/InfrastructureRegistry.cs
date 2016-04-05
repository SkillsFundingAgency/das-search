namespace Sfa.Infrastructure.DependencyResolution
{
    using CourseDirectory;
    using Eds.Das.Indexer.ApplicationServices.Http;
    using Eds.Das.Indexer.ApplicationServices.Infrastructure;
    using Eds.Das.Indexer.ApplicationServices.MetaData;
    using Eds.Das.Indexer.ApplicationServices.Provider;
    using Eds.Das.Indexer.ApplicationServices.Services;
    using Eds.Das.Indexer.Core.Services;
    using Elasticsearch;

    using Nest;

    using Services;
    using Settings;
    using StructureMap;

    public class InfrastructureRegistry : Registry
    {
        public InfrastructureRegistry()
        {
            For<IGetStandardLevel>().Use<LarsClient>();
            For<ILarsSettings>().Use<LarsSettings>();
            For<IGetActiveProviders>().Use<FcsActiveProvidersClient>();
            For<IConvertFromCsv>().Use<CsvService>();
            For<IVstsClient>().Use<VstsClient>();
            For<IHttpGetFile>().Use<HttpService>();
            For<IHttpGet>().Use<HttpService>();
            For<IHttpPost>().Use<HttpService>();
            For<IInfrastructureSettings>().Use<InfrastructureSettings>();
            For<ILog>().Use(x => new NLogService(x.ParentType)).AlwaysUnique();
            For<IUnzipStream>().Use<ZipFileExtractor>();
            For<IGetApprenticeshipProviders>().Use<CourseDirectoryClient>();
            For<IMaintainApprenticeshipIndex>().Use<ElasticsearchApprenticeshipIndexMaintainer>();
            For<IMaintainProviderIndex>().Use<ElasticsearchProviderIndexMaintainer>();
            For<IApprenticeshipIndexDefinitions>().Use<ApprenticeshipIndexDefinitions>();
            For<IElasticsearchMapper>().Use<ElasticsearchMapper>();
            For<IElasticClient>().Use<ElasticClient>();
        }
    }
}
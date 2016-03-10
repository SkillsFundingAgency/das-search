namespace Sfa.Infrastructure.DependencyResolution
{
    using CourseDirectory;
    using Sfa.Deds.Services;
    using Sfa.Deds.Settings;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Http;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Infrastructure;
    using Sfa.Eds.Das.Indexer.ApplicationServices.MetaData;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Provider;
    using Sfa.Eds.Das.Indexer.Core;
    using Sfa.Eds.Das.Indexer.Core.Services;
    using Sfa.Infrastructure.Services;
    using Sfa.Infrastructure.Settings;

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
        }
    }
}
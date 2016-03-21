namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.DependencyResolution
{
    using Indexer.ApplicationServices.MetaData;
    using Indexer.ApplicationServices.Settings;
    using Services;
    using Services.Interfaces;

    using StructureMap;

    public sealed class MetaDataCreationRegistry : Registry
    {
        public MetaDataCreationRegistry()
        {
            For<IAppServiceSettings>().Use<AppServiceSettings>();
            For<ILarsDataService>().Use<LarsDataService>();
            For<IReadStandardsFromCsv>().Use<CsvService>();
            For<IAngleSharpService>().Use<AngleSharpService>();
            For<IVstsService>().Use<VstsService>();
            For<IGitDynamicModelGenerator>().Use<GitDynamicModelGenerator>();
            For<IGetStandardMetaData>().Use<MetaDataManager>();
            For<IGenerateStandardMetaData>().Use<MetaDataManager>();
            For<IGetFrameworkMetaData>().Use<MetaDataManager>();
        }
    }
}
using Sfa.Das.Sas.Indexer.ApplicationServices.MetaData;
using Sfa.Das.Sas.Tools.MetaDataCreationTool.Factories;
using Sfa.Das.Sas.Tools.MetaDataCreationTool.Factories.MetaData;
using Sfa.Das.Sas.Tools.MetaDataCreationTool.Services;
using Sfa.Das.Sas.Tools.MetaDataCreationTool.Services.Interfaces;
using StructureMap;

namespace Sfa.Das.Sas.Tools.MetaDataCreationTool.DependencyResolution
{
    using Sfa.Das.Sas.Indexer.ApplicationServices.Settings;

    public sealed class MetaDataCreationRegistry : Registry
    {
        public MetaDataCreationRegistry()
        {
            For<ILarsDataService>().Use<LarsDataService>();
            For<IReadMetaDataFromCsv>().Use<CsvService>();
            For<IAngleSharpService>().Use<AngleSharpService>();
            For<IMetadataApiService>().Use<MetadataApiService>();
            For<IVstsService>().Use<VstsService>();
            For<IGitDynamicModelGenerator>().Use<GitDynamicModelGenerator>();
            For<IGetStandardMetaData>().Use<MetaDataManager>();
            For<IGenerateStandardMetaData>().Use<MetaDataManager>();
            For<IGetFrameworkMetaData>().Use<MetaDataManager>();
            For<IJsonMetaDataConvert>().Use<JsonMetaDataConvert>();
            For<IGenericMetaDataFactory>().Use<LarsMetaDataFactory>();

            // Meta Data factories
            For<IMetaDataFactory>().Use<FrameworkMetaDataFactory>();
            For<IMetaDataFactory>().Use<FrameworkAimMetaDataFactory>();
            For<IMetaDataFactory>().Use<FrameworkComponentTypeMetaDataFactory>();
            For<IMetaDataFactory>().Use<LearningDeliveryMetaDataFactory>();
            For<IMetaDataFactory>().Use<StandardMetaDataFactory>();
            For<IMetaDataFactory>().Use<FundingMetaDataFactory>();

            For<IAppServiceSettings>().Use<AppServiceSettings>();
        }
    }
}
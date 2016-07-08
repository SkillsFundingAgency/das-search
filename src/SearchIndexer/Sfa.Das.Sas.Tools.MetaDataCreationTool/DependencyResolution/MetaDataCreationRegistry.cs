using System.Collections.Generic;
using Sfa.Das.Sas.Indexer.ApplicationServices.MetaData;
using Sfa.Das.Sas.Tools.MetaDataCreationTool.Factories;
using Sfa.Das.Sas.Tools.MetaDataCreationTool.Factories.MetaData;
using Sfa.Das.Sas.Tools.MetaDataCreationTool.Services;
using Sfa.Das.Sas.Tools.MetaDataCreationTool.Services.Interfaces;
using StructureMap;

namespace Sfa.Das.Sas.Tools.MetaDataCreationTool.DependencyResolution
{
    public sealed class MetaDataCreationRegistry : Registry
    {
        public MetaDataCreationRegistry()
        {
            For<ILarsDataService>().Use<LarsDataService>();
            For<IReadMetaDataFromCsv>().Use<CsvService>();
            For<IAngleSharpService>().Use<AngleSharpService>();
            For<IVstsService>().Use<VstsService>();
            For<IGitDynamicModelGenerator>().Use<GitDynamicModelGenerator>();
            For<IGetStandardMetaData>().Use<MetaDataManager>();
            For<IGenerateStandardMetaData>().Use<MetaDataManager>();
            For<IGetFrameworkMetaData>().Use<MetaDataManager>();
            For<IJsonMetaDataConvert>().Use<JsonMetaDataConvert>();
            For<IGenericMetaDataFactory>().Use<LarsMetaDataFactory>();
            For<IEnumerable<IMetaDataFactory>>().Use(() => new List<IMetaDataFactory>
            {
                new FrameworkMetaDataFactory(),
                new FrameworkAimMetaDataFactory(),
                new FrameworkComponentTypeMetaDataFactory(),
                new LearningDeliveryMetaDataFactory()
            });

        }
    }
}
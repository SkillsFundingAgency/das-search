namespace Sfa.Eds.Das.Indexer.ApplicationServices.Standard
{
    using System.Collections.Generic;
    using Core.Services;
    using Newtonsoft.Json;

    using Sfa.Eds.Das.Indexer.ApplicationServices.MetaData;
    using Sfa.Eds.Das.Indexer.Core.Models;
    using Sfa.Eds.Das.Indexer.Core.Models.Framework;

    public class MetaDataHelper : IMetaDataHelper
    {
        private readonly IGetStandardMetaData _metaDataReader;

        private readonly IGenerateStandardMetaData _metaDataWriter;

        private readonly ILog _log;

        public MetaDataHelper(IGetStandardMetaData metaDataReader, IGenerateStandardMetaData metaDataGenerator, ILog log)
        {
            _metaDataReader = metaDataReader;
            _metaDataWriter = metaDataGenerator;
            _log = log;
        }

        public List<MetaDataItem> GetAllStandardsMetaData()
        {
            var standardsMetaDataJson = _metaDataReader.GetAllAsJson();
            var standardsMetaData = new List<MetaDataItem>();

            foreach (var item in standardsMetaDataJson)
            {
                try
                {
                    standardsMetaData.Add(JsonConvert.DeserializeObject<MetaDataItem>(item.Value));
                }
                catch (JsonReaderException ex)
                {
                    _log.Warn($"Couldn't deserialise meta data for: {item.Key}", ex);
                }
            }

            return standardsMetaData;
        }

        public void UpdateMetadataRepository()
        {
            _metaDataWriter.GenerateStandardMetadataFiles();
        }

        // Frameworks
        public List<FrameworkMetaData> GetAllFrameworkMetaData()
        {
            // ToDo: Missing implementation -> Get data from LARS
            return new List<FrameworkMetaData>
                       {
                           new FrameworkMetaData
                               {
                                   FworkCode = "403",
                                   ProgType = "0",
                                   PwayCode = "2",
                                   PathwayName = "Baking Industry Skills",
                                   NASTitle = "Food and Drink",
                                   IssuingAuthorityTitle = "Food and Drink - Advanced Level Apprenticeship"
                               },
                           new FrameworkMetaData
                               {
                                   FworkCode = "403",
                                   ProgType = "3",
                                   PwayCode = "7",
                                   PathwayName = "Brewing Industry Skills",
                                   NASTitle = "Food and Drink",
                                   IssuingAuthorityTitle = "Food and Drink - Intermediate Level Apprenticeship"
                               },
                           new FrameworkMetaData
                               {
                                   FworkCode = "423",
                                   ProgType = "2",
                                   PwayCode = "4",
                                   PathwayName = "Footwear",
                                   NASTitle = "Fashion and Textiles",
                                   IssuingAuthorityTitle = "Fashion and Textiles - Advanced Level Apprenticeship"
                               }
                       };
        }

    }
}
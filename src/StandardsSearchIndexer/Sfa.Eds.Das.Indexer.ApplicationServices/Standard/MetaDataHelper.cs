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

        private readonly IGetFrameworkMetaData _metaDataFrameworkReader;

        private readonly ILog _log;

        public MetaDataHelper(IGetStandardMetaData metaDataReader, IGenerateStandardMetaData metaDataGenerator, ILog log, IGetFrameworkMetaData metaDataFrameworkReader)
        {
            _metaDataReader = metaDataReader;
            _metaDataWriter = metaDataGenerator;
            _log = log;
            _metaDataFrameworkReader = metaDataFrameworkReader;
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

        public List<FrameworkMetaData> GetAllFrameworkMetaData()
        {
            return _metaDataFrameworkReader.GetAllFrameworks();
        }
    }
}
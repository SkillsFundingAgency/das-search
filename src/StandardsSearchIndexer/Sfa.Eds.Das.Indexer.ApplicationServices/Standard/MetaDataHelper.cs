namespace Sfa.Eds.Das.Indexer.ApplicationServices.Standard
{
    using System.Collections.Generic;

    using Newtonsoft.Json;

    using Sfa.Eds.Das.Indexer.ApplicationServices.Infrastructure;
    using Sfa.Eds.Das.Indexer.ApplicationServices.MetaData;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Services;
    using Sfa.Eds.Das.Indexer.Core.Models;

    public class MetaDataHelper : IMetaDataHelper
    {
        private readonly IGetStandardMetaData _metaDataReader;

        private readonly IGenerateStandardMetaData _metaDataWriter;

        private readonly ILog Log;

        public MetaDataHelper(IGetStandardMetaData metaDataReader, IGenerateStandardMetaData metaDataGenerator, ILog log)
        {
            _metaDataReader = metaDataReader;
            _metaDataWriter = metaDataGenerator;
            Log = log;
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
                    Log.Warn($"Couldn't deserialise meta data for: {item.Key}", ex);
                }
            }

            return standardsMetaData;
        }

        public void UpdateMetadataRepository()
        {
            _metaDataWriter.GenerateStandardMetadataFiles();
        }
    }
}
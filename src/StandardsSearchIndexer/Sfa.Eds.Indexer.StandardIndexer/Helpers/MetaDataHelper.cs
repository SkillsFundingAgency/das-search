namespace Sfa.Eds.Das.StandardIndexer.Helpers
{
    using System.Collections.Generic;
    using System.Linq;

    using Newtonsoft.Json;

    using Sfa.Eds.Das.Indexer.Common.Models;
    using Sfa.Eds.Das.Tools.MetaDataCreationTool;
    using Tools.MetaDataCreationTool.Services.Interfaces;
    using log4net;
    using System.Reflection;

    public class MetaDataHelper : IMetaDataHelper
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IGetStandardMetaData _metaDataReader;
        private readonly IGenerateStandardMetaData _metaDataWriter;

        public MetaDataHelper(IGetStandardMetaData metaDataReader, IGenerateStandardMetaData metaDataGenerator)
        {
            _metaDataReader = metaDataReader;
            _metaDataWriter = metaDataGenerator;
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
                    Log.Warn($"Couldn't deserialise meta data for: {item.Key}");
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

namespace Sfa.Eds.Das.StandardIndexer.Helpers
{
    using System.Collections.Generic;
    using System.Linq;

    using Newtonsoft.Json;

    using Sfa.Eds.Das.Indexer.Common.Models;
    using Sfa.Eds.Das.Tools.MetaDataCreationTool;
    using Tools.MetaDataCreationTool.Services.Interfaces;

    internal class MetaDataHelper : IMetaDataHelper
    {
        private readonly IGetStandardMetaData _metaDataReader;
        private readonly IGenerateStandardMetaData _metaDataWriter;

        public MetaDataHelper(IGetStandardMetaData metaDataReader, IGenerateStandardMetaData metaDataGenerator)
        {
            _metaDataReader = metaDataReader;
            _metaDataWriter = metaDataGenerator;
        }

        public List<MetaDataItem> GetAllStandardsMetaData()
        {
            var standards = _metaDataReader.GetAllAsJson();

            return standards.Select(JsonConvert.DeserializeObject<MetaDataItem>).ToList();
        }

        public void UpdateMetadataRepository()
        {
            _metaDataWriter.GenerateStandardMetadataFiles();
        }
    }
}

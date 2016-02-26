namespace Sfa.Eds.Das.StandardIndexer.Helpers
{
    using System.Collections.Generic;
    using System.Linq;

    using Newtonsoft.Json;

    using Sfa.Eds.Das.Indexer.Common.Models;
    using Sfa.Eds.Das.Tools.MetaDataCreationTool;

    internal class MetaDataHelper : IMetaDataHelper
    {
        private readonly MetaDataManager _metaData;

        public MetaDataHelper()
        {
            _metaData = new MetaDataManager();
        }

        public List<MetaDataItem> GetAllStandardsMetaData()
        {
            var standards = _metaData.GetAllAsJson();

            return standards.Select(JsonConvert.DeserializeObject<MetaDataItem>).ToList();
        }

        public void UpdateMetadataRepository()
        {
            _metaData.GenerateStandardMetadataFiles();
        }
    }
}

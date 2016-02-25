namespace Sfa.Eds.Das.StandardIndexer.Helpers
{
    using System.Collections.Generic;
    using System.Linq;

    using Newtonsoft.Json;

    using Sfa.Eds.Das.Indexer.Common.Models;
    using Sfa.Eds.Das.Tools.MetaDataCreationTool;

    internal class MetaDataHelper : IMetaDataHelper
    {
        private readonly MetaData _metaData;
        public MetaDataHelper()
        {
            _metaData = new MetaData();
        }

        public List<JsonMetadataObject> GetAllStandardsFromGit()
        {
            var standards = _metaData.GetStandards();
            return standards.Select(JsonConvert.DeserializeObject<JsonMetadataObject>).ToList();
        }

        public void UpdateMetadataRepository()
        {
            _metaData.GenerateStandardMetadataFiles();
        }
    }
}

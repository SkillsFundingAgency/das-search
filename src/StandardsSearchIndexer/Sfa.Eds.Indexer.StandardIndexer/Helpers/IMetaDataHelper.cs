using System.Collections.Generic;
using Sfa.Eds.Das.Indexer.Common.Models;

namespace Sfa.Eds.Das.StandardIndexer.Helpers
{
    public interface IMetaDataHelper
    {
        List<MetaDataItem> GetAllStandardsFromGit();

        void UpdateMetadataRepository();
    }
}
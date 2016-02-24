using System.Collections.Generic;
using Sfa.Eds.Das.Indexer.Common.Models;

namespace Sfa.Eds.Das.StandardIndexer.Helpers
{
    internal interface IMetaDataHelper
    {
        List<JsonMetadataObject> GetAllStandardsFromGit();
    }
}
using System.Collections.Generic;
using Sfa.Das.Sas.Indexer.Core.Models;
using Sfa.Das.Sas.Indexer.Core.Models.Framework;

namespace Sfa.Das.Sas.Indexer.ApplicationServices.Standard
{
    public interface IMetaDataHelper
    {
        List<StandardMetaData> GetAllStandardsMetaData();

        void UpdateMetadataRepository();

        List<FrameworkMetaData> GetAllFrameworkMetaData();
    }
}
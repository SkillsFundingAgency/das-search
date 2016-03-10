namespace Sfa.Eds.Das.Indexer.ApplicationServices.Standard
{
    using System.Collections.Generic;

    using Sfa.Eds.Das.Indexer.Core.Models;

    public interface IMetaDataHelper
    {
        List<MetaDataItem> GetAllStandardsMetaData();

        void UpdateMetadataRepository();
    }
}
namespace Sfa.Eds.Das.Indexer.ApplicationServices.Standard
{
    using System.Collections.Generic;

    using Sfa.Eds.Das.Indexer.Core.Models;
    using Sfa.Eds.Das.Indexer.Core.Models.Framework;

    public interface IMetaDataHelper
    {
        List<StandardMetaData> GetAllStandardsMetaData();

        void UpdateMetadataRepository();

        List<FrameworkMetaData> GetAllFrameworkMetaData();
    }
}
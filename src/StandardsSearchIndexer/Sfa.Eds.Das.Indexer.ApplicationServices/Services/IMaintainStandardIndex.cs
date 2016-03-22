namespace Sfa.Eds.Das.Indexer.ApplicationServices.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Sfa.Eds.Das.Indexer.Core.Models;
    using Sfa.Eds.Das.Indexer.Core.Models.Framework;

    // ToDo: Rename Apprentiecship
    public interface IMaintainStandardIndex : IMaintainSearchIndexes
    {
        Task IndexStandards(string indexName, ICollection<MetaDataItem> entries);

        Task IndexFrameworks(string indexName, ICollection<FrameworkMetaData> entries);
    }
}
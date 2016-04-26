using System.Collections.Generic;
using System.Threading.Tasks;
using Sfa.Das.Sas.Indexer.Core.Models;
using Sfa.Das.Sas.Indexer.Core.Models.Framework;

namespace Sfa.Das.Sas.Indexer.ApplicationServices.Services
{
    public interface IMaintainApprenticeshipIndex : IMaintainSearchIndexes
    {
        Task IndexStandards(string indexName, ICollection<StandardMetaData> entries);

        Task IndexFrameworks(string indexName, ICollection<FrameworkMetaData> entries);
    }
}
using System.Collections.Generic;

namespace Sfa.Das.Sas.Indexer.ApplicationServices.MetaData
{
    public interface IVstsService
    {
        IEnumerable<string> GetExistingStandardIds();

        IDictionary<string, string> GetStandards();

        void PushCommit(List<FileContents> items);
    }
}
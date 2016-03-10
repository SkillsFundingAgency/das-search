namespace Sfa.Eds.Das.Indexer.ApplicationServices.MetaData
{
    using System.Collections.Generic;

    public interface IVstsService
    {
        IEnumerable<string> GetExistingStandardIds();

        IDictionary<string, string> GetStandards();

        void PushCommit(List<FileContents> items);
    }
}
using System.Collections.Generic;

namespace Sfa.Das.Sas.Tools.MetaDataCreationTool.Services.Interfaces
{
    using Sfa.Das.Sas.Indexer.Core.Models;
    using Sfa.Das.Sas.Tools.MetaDataCreationTool.Models.Git;

    public interface IVstsService
    {
        IEnumerable<string> GetExistingStandardIds();

        IEnumerable<StandardMetaData> GetStandards();

        IEnumerable<VstsFrameworkMetaData> GetFrameworks();

        void PushCommit(List<FileContents> items);
    }
}

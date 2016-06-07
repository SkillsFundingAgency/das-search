using System.Collections.Generic;

namespace Sfa.Das.Sas.Tools.MetaDataCreationTool.Services.Interfaces
{
    using Sfa.Das.Sas.Tools.MetaDataCreationTool.Models.Git;

    public interface IVstsService
    {
        IEnumerable<string> GetExistingStandardIds();

        IDictionary<string, string> GetStandards();

        IEnumerable<VstsFrameworkMetaData> GetFrameworks();

        void PushCommit(List<FileContents> items);
    }
}

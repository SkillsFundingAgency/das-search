namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.Services.Interfaces
{
    using System.Collections.Generic;

    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Models;

    public interface IVstsService
    {
        IEnumerable<string> GetExistingStandardIds();
        IEnumerable<string> GetStandards();
        void PushCommit(List<StandardObject> items);
    }
}
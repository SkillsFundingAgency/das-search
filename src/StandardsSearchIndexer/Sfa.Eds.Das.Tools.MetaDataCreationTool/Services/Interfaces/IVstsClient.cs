namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Models;

    public interface IVstsClient
    {
        void PushCommit(List<StandardObject> items);
        IEnumerable<string> GetAllFileContents(string path);
    }
}
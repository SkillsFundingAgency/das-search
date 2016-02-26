namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.Services.Interfaces
{
    using System.Collections.Generic;

    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Models;

    public interface IGitDynamicModelGenerator
    {
        string GenerateCommitBody(string branchPath, string oldObjectId, List<StandardObject> items);
    }
}
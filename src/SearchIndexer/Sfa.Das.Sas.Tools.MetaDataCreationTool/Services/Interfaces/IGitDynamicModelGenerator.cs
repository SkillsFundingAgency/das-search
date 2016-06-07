using System.Collections.Generic;

namespace Sfa.Das.Sas.Tools.MetaDataCreationTool.Services.Interfaces
{
    using Sfa.Das.Sas.Tools.MetaDataCreationTool.Models.Git;

    public interface IGitDynamicModelGenerator
    {
        string GenerateCommitBody(string branchPath, string oldObjectId, List<FileContents> items);
    }
}
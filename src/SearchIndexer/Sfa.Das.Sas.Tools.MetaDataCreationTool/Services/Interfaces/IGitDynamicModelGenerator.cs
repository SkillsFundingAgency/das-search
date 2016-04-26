using System.Collections.Generic;
using Sfa.Das.Sas.Indexer.ApplicationServices.MetaData;

namespace Sfa.Das.Sas.Tools.MetaDataCreationTool.Services.Interfaces
{
    public interface IGitDynamicModelGenerator
    {
        string GenerateCommitBody(string branchPath, string oldObjectId, List<FileContents> items);
    }
}
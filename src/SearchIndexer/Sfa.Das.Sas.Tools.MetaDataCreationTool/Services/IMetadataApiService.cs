using System.Collections.Generic;
using Sfa.Das.Sas.Indexer.Core.Models;
using Sfa.Das.Sas.Tools.MetaDataCreationTool.Models.Git;

namespace Sfa.Das.Sas.Tools.MetaDataCreationTool.Services
{
    public interface IMetadataApiService
    {
        List<StandardMetaData> GetStandards();

        List<VstsFrameworkMetaData> GetFrameworks();
    }
}
using System.Collections.Generic;
using Sfa.Das.Sas.Indexer.Core.Models.Framework;
using Sfa.Das.Sas.Tools.MetaDataCreationTool.Models;

namespace Sfa.Das.Sas.Tools.MetaDataCreationTool.Services.Interfaces
{
    public interface ILarsDataService
    {
        IEnumerable<LarsStandard> GetListOfCurrentStandards();

        List<FrameworkMetaData> GetListOfCurrentFrameworks();
    }
}
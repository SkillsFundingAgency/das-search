namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.Services.Interfaces
{
    using System.Collections.Generic;

    using Sfa.Eds.Das.Indexer.Core.Models.Framework;
    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Models;

    public interface ILarsDataService
    {
        IEnumerable<Standard> GetListOfCurrentStandards();

        List<FrameworkMetaData> GetListOfCurrentFrameworks();
    }
}
namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.Services.Interfaces
{
    using System.Collections.Generic;

    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Models;

    public interface ILarsDataService
    {
        IEnumerable<Standard> GetListOfCurrentStandards();
    }
}
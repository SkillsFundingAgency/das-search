namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.Services.interfaces
{
    using System.Collections.Generic;
    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Models;

    public interface ICsvService
    {
        List<Standard> GetAllStandardsFromCsv(string csvFile);
    }
}
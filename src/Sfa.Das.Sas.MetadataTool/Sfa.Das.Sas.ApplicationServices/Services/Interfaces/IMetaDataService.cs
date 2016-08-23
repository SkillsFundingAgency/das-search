namespace Sfa.Das.Sas.ApplicationServices.Services.Interfaces
{
    using System.Collections.Generic;

    using Core.Models;

    public interface IMetaDataService
    {
        IEnumerable<StandardMetaData> GetStandards();

        StandardMetaData GetStandard(int id);

        IEnumerable<FrameworkMetaData> GetFrameworks();

        FrameworkMetaData GetFramework(int id);
    }
}